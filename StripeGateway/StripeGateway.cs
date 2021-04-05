using Stripe;
using System;

namespace StripePayment
{
    public class StripeGateway
    {

        public StripeGateway(string ApiKey)
        {
            StripeConfiguration.ApiKey = ApiKey;
        }

        public bool Pay(StripeCard card, Money payout, string receiptEmail="", string description="")
        {
            return Pay(
                card.CardNumber, card.ExpiredYear, card.ExpiredMonth, card.CVV, 
                payout.Amount, payout.Currency, 
                receiptEmail, description);
        }
        public bool Pay(
            string CardNo,
            int ExpiredYear, int ExpiredMonth,
            string CVV,
            decimal amount, string currency,
            string receiptEmail="", string description="")
        {
            try
            {
                TokenCreateOptions stripeCard = new TokenCreateOptions 
                {
                    Card = new TokenCardOptions
                    {
                        Number = CardNo,
                        ExpMonth = Convert.ToInt64(ExpiredMonth),
                        ExpYear = Convert.ToInt64(ExpiredYear),
                        Cvc = CVV,
                    },
                };

                TokenService service = new TokenService();
                Token newToken = service.Create(stripeCard);

                var cardOption = new SourceCreateOptions
                {
                    Type = SourceType.Card,
                    Currency = currency,
                    Token = newToken.Id
                };


                var sourceService = new SourceService();
                Source source = sourceService.Create(cardOption);

                /*
                CustomerCreateOptions customerInfo = new CustomerCreateOptions
                {
                    Name = "SP Tutorial",
                    Email = stripeEmail,
                    Description = "Paying 10 Rs",
                    Address = new AddressOptions { 
                        City = "Kolkata", 
                        Country = "India", 
                        Line1 = "Sample Address", 
                        Line2 = "Sample Address 2", 
                        PostalCode = "700030", 
                        State = "WB" 
                    }
                };

                //var customerService = new CustomerService();
                //var customer = customerService.Create(customerInfo);
                */

                var chargeoption = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(amount * 100),
                    Currency = currency,
                    Description = description,
                    ReceiptEmail = receiptEmail,
                    Source = source.Id
                };

                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeoption);
                if (charge.Status == "succeeded")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool Pay(
            string stripeToken,
            decimal amount, string currency,
            string receiptEmail = "", string description = "")
        {
            try
            {
                var cardOption = new SourceCreateOptions
                {
                    Type = SourceType.Card,
                    Currency = currency,
                    Token = stripeToken
                };

                var sourceService = new SourceService();
                Source source = sourceService.Create(cardOption);

                var chargeoption = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(amount * 100),
                    Currency = currency,
                    Description = description,
                    ReceiptEmail = receiptEmail,
                    Source = source.Id
                };

                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeoption);
                if (charge.Status == "succeeded")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public string GeneratePayNowLink(
            string customerEmail,
            decimal amountToPay,
            string currency,
            string description = "",
            bool sendInvoice=false)
        {
            try {
                CustomerCreateOptions customerInfo = new CustomerCreateOptions
                {
                    Email = customerEmail,
                    //PaymentMethod = "card",
                };
                var customerService = new CustomerService();
                var customer = customerService.Create(customerInfo);

                var invoiceItemOption = new InvoiceItemCreateOptions
                {
                    Customer = customer.Id,
                    Amount = Convert.ToInt32(amountToPay * 100),
                    Currency = currency,
                };
                var invoiceItemService = new InvoiceItemService();
                var invoiceItem = invoiceItemService.Create(invoiceItemOption);

                var invoiceOptions = new InvoiceCreateOptions
                {
                    Customer = customer.Id,
                    CollectionMethod = "send_invoice",
                    DaysUntilDue = 30,
                    Description = description
                };

                var service = new InvoiceService();
                var invoice = service.Create(invoiceOptions);
                invoice = service.FinalizeInvoice(invoice.Id);
                if(sendInvoice)
                   invoice = service.SendInvoice(invoice.Id);
                return invoice.HostedInvoiceUrl;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
