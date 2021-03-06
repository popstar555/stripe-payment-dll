using Stripe;
using StripePayemnt;
using System;
using System.Collections.Generic;
using System.Text;

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

        public InvoiceInfo GeneratePayNowLink(
            string customerEmail,
            decimal amountToPay,
            string currency,
            string description = "")
        {
            try
            {
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

                try
                {
                    var paymentIntentService = new PaymentIntentService();

                    var paymentIntent = paymentIntentService.Get(invoice.PaymentIntentId);
                    var paymentIntentUpdateOptions = new PaymentIntentUpdateOptions
                    {
                        Description = description
                    };
                    paymentIntentService.Update(paymentIntent.Id, paymentIntentUpdateOptions);
                }
                catch (Exception)
                {
                    //continue
                }

                var result = new InvoiceInfo
                {
                    Url = invoice.HostedInvoiceUrl,
                    Id = invoice.Id
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        public string SendInvoice(
            string customerEmail,
            decimal amountToPay,
            string currency,
            string description = "",
            bool sendInvoice = true)
        {
            try
            {
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
                    Description = description,
                    AutoAdvance = true
                };

                var service = new InvoiceService();
                var invoice = service.Create(invoiceOptions);
                invoice = service.FinalizeInvoice(invoice.Id);

                try
                {
                    var paymentIntentService = new PaymentIntentService();

                    var paymentIntent = paymentIntentService.Get(invoice.PaymentIntentId);
                    var paymentIntentUpdateOptions = new PaymentIntentUpdateOptions
                    {
                        Description = description
                    };
                    paymentIntentService.Update(paymentIntent.Id, paymentIntentUpdateOptions);
                }
                catch (Exception)
                {
                    //continue
                }

                if (sendInvoice)
                    invoice = service.SendInvoice(invoice.Id);

                return invoice.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool CheckInvoicePayment(string invoiceId)
        {
            var service = new InvoiceService();
            var invoice = service.Get(invoiceId);
            return invoice.Status == "paid";
        }

        private List<Payment> ExportPayment(DateTime start)
        {
            var range = new DateRangeOptions
            {
                GreaterThanOrEqual = start,
            };
            return _ExportPayment(range);
        }
        private List<Payment> ExportPayment(DateTime start, DateTime end)
        {
            var range = new DateRangeOptions
            {
                GreaterThanOrEqual = start,
                LessThanOrEqual = end
            };
            return _ExportPayment(range);
        }

        private List<Payment> _ExportPayment(DateRangeOptions range)
        {
            var payments = new List<Payment>();

            try
            {
                var options = new PaymentIntentListOptions
                {
                    Limit = 100,
                    Created = range,
                };
                var paymentIntentService = new PaymentIntentService();
                var transactionService = new BalanceTransactionService();
                var customerService = new CustomerService();
                var chargeService = new ChargeService();

                StripeList<PaymentIntent> pis = paymentIntentService.List(options);

                for (int i = 0; i < pis.Data.Count; i++)
                {
                    var pi = pis.Data[i];

                    var payment = new Payment()
                    {
                        Description = pi.Description,
                        Created = pi.Created,
                        Amount = Convert.ToDecimal(pi.Amount) / 100,
                        Currency = pi.Currency,
                        Status = pi.Status,
                        StatementDescriptor = pi.StatementDescriptor,
                        CustomerId = pi.CustomerId,
                        CardId = pi.PaymentMethodId,
                        InvoiceId = pi.InvoiceId
                    };

                    if (pi.Charges.Data.Count > 0)
                    {
                        var charge = pi.Charges.Data[0];
                        try
                        {
                            charge.BalanceTransaction = transactionService.Get(charge.BalanceTransactionId);
                            payment.Id = charge.Id;
                            payment.ConvertedAmount = Convert.ToDecimal(charge.BalanceTransaction.Amount) / 100;
                            payment.AmountRefunded = Convert.ToDecimal(charge.AmountRefunded) / 100;
                            payment.Fee = Convert.ToDecimal(charge.BalanceTransaction.Fee) / 100;
                            payment.ConvertedCurrency = charge.BalanceTransaction.Currency;
                            payment.Tax = 0;
                            payment.Captured = charge.Captured;
                            payment.Transfer = charge.TransferId;
                            try
                            {
                                if (charge.Refunds.Data.Count > 0)
                                {
                                    var refundTx = transactionService.Get(charge.Refunds.Data[0].BalanceTransactionId);
                                    payment.ConvertedAmountRefunded = Convert.ToDecimal(refundTx.Amount) / 100;
                                }
                            }
                            catch (Exception) { }
                        }
                        catch (Exception) { }

                    }

                    try
                    {
                        pi.Customer = customerService.Get(pi.CustomerId);
                        payment.CustomerDescription = pi.Customer.Description;
                        payment.CustomerEmail = pi.Customer.Email;

                    }
                    catch (Exception) { }


                    payment.Description = pi.Description;
                    payments.Add(payment);
                }

                var optionsC = new ChargeListOptions
                {
                    Limit = 100,
                    Created = range,
                };
                StripeList<Charge> chs = chargeService.List(optionsC);
                for (int i = 0; i < chs.Data.Count; i++)
                {
                    var ch = chs.Data[i];
                    if(FindPayment(payments, ch.Id))
                    {
                        continue;
                    }

                    var payment = new Payment()
                    {
                        Id = ch.Id,
                        Description = ch.Description,
                        Created = ch.Created,
                        Amount = Convert.ToDecimal(ch.Amount) / 100,
                        Currency = ch.Currency,
                        Status = ch.Status,
                        StatementDescriptor = ch.StatementDescriptor,
                        CustomerId = ch.CustomerId,
                        Captured = ch.Captured,
                        CardId = ch.PaymentMethod,
                        InvoiceId = ch.InvoiceId,
                        Transfer = ch.TransferId
                    };
                    try
                    {
                        ch.BalanceTransaction = transactionService.Get(ch.BalanceTransactionId);
                        payment.ConvertedAmount = Convert.ToDecimal(ch.BalanceTransaction.Amount) / 100;
                        payment.AmountRefunded = Convert.ToDecimal(ch.AmountRefunded) / 100;
                        payment.Fee = Convert.ToDecimal(ch.BalanceTransaction.Fee) / 100;
                        payment.ConvertedCurrency = ch.BalanceTransaction.Currency;
                        payment.Tax = 0;
                    }
                    catch (Exception) { }
                    try
                    {
                        ch.Customer = customerService.Get(ch.CustomerId);
                        payment.CustomerDescription = ch.Customer.Description;
                        payment.CustomerEmail = ch.Customer.Email;
                    }
                    catch (Exception) { }

                    payments.Add(payment);
                }
            }
            catch (Exception) { }

            payments.Sort(
                delegate(Payment a, Payment b)
                {
                    return b.Created.CompareTo(a.Created) ;
                }
            );
            return payments;
        }

        private bool FindPayment(List<Payment> payments, string id)
        {
            for(int i=0; i<payments.Count; i++)
            {
                if(payments[i].Id!="" && payments[i].Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Payment> ExportPaymentToday()
        {
            var today = DateTime.Today;
            var start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            return ExportPayment(start);
        }

        public List<Payment> ExportPaymentCurrentMonth()
        {
            var today = DateTime.Today;
            var start = new DateTime(today.Year, today.Month, 1, 0, 0, 0);
            return ExportPayment(start);
        }
        public List<Payment> ExportPaymentLast7days()
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(-7);
            var start = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            return ExportPayment(start);
        }
        public List<Payment> ExportPaymentCustomRange(DateTime start, DateTime end)
        {
            return ExportPayment(start, end);
        }

        public bool WritePaymentToCSV(List<Payment> payments, string filePath)
        {
            string[] header =
            {
                "id",
                "Description",
                "Created",
                "Amount",
                "Amount Refunded",
                "Curency",
                "Converted Amount",
                "Converted Amount Refunded",
                "Fee",
                "Tax",
                "Converted Currency",
                "Status",
                "Statement Descriptor",
                "Customer ID",
                "Customer Description",
                "Customer Email",
                "Captured",
                "Card ID",
                "Invoice ID",
                "Transfer"
            };

            var csv = new StringBuilder();
            csv.AppendLine(String.Join(",", header));
            
            for(int i=0; i<payments.Count; i++)
            {
                string[] record = payments[i].ToArray();
                csv.AppendLine(String.Join(",", record));
            }

            try
            {
                System.IO.File.WriteAllText(filePath, csv.ToString());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
