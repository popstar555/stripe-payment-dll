# stripe-payment-dll
DLL for Stripe Payment
## Requirement
        .Net Framework 4.6.1 +
## How to Use

>        using StripePayment;
>
>        ...
>
>        StripeGateway stripe = new StripeGateway("Your publishable key");
>
>        // method 1
>        StripeCard card = new StripeCard("4242424242424", 2021, 5, "034");
>        Money payout = new Money(50.50m, "usd");
>
>        bool result = stripe.Pay(card, payout);
>        Console.WriteLine("result = ", result);
>
>        // method 2
>        result = stripe.Pay("4242**********", 2015, 5, "034", 50.50m, "eur", "test@stripe.com", "Pay for testing");
>        Console.WriteLine("result = ", result);
>
>        // method 3: using stripe token of card
>        result = stripe.Pay("card_1Ib6KCB0XzTzRcrcIHseF9hK", 50.50m, "eur", "test@stripe.com", "Pay for testing");
>        Console.WriteLine("result = ", result);
>
>        // method to get PayNow link and check if paid
>        InvoiceInfo invoice = stripe.GeneratePayNowLink("test@stripe.com", 5, "usd", "Request to pay for service");
>        string PayNowLink = invoice.Url;
>        bool bPaid = stripe.CheckInvoicePayment(invoice.Id);
>
>        // method to send invoice and check if paid
>        string InvoiceId = stripe.SendInvoice("test@stripe.com", 5, "usd", "Request to pay for service");
>        bPaid = stripe.CheckInvoicePayment(InvoiceId);