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
>        // method 1 to pay
>        var card = new StripeCard("4242424242424", 2021, 5, "034");
>        var payout = new Money(50.50m, "usd");
>
>        bool result = stripe.Pay(card, payout);
>        Console.WriteLine("result = "+result);
>
>        // method 2 to pay
>        result = stripe.Pay("4242**********", 2015, 5, "034", 50.50m, "eur", "test@stripe.com", "Pay for testing");
>        Console.WriteLine("result = "+result);
>
>        // method 3 to pay using stripe token of card
>        result = stripe.Pay("card_1Ib6KCB0XzTzRcrcIHseF9hK", 50.50m, "eur", "test@stripe.com", "Pay for testing");
>        Console.WriteLine("result = "+result);
>
>        // method to get PayNow link
>        string url_of_PayNow = stripe.GeneratePayNowLink("test@stripe.com", 5, "usd", "Request to pay for service");
