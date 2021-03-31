# stripe-payment-dll
DLL for Stripe Payment

## How to Use

        using Payment;
        using Payment.Stripe;

        ...

        StripeGateway stripe = new StripeGateway("Your publishable key");

        // method 1
        CreditCard card = new CreditCard("4242424242424", 2021, 5, "034");
        Money payout = new Money(50.50m, "usd");

        bool result = stripe.Pay(card, payout);
        Console.WriteLine("result = ", result);

        // method 2
        result = stripe.Pay("4242**********", 2015, 5, "034", 50.50m, "eur", "test@stripe.com", "Pay for testing");
        Console.WriteLine("result = ", result);

        // method 3: using stripe token of card
        result = stripe.Pay("card_1Ib6KCB0XzTzRcrcIHseF9hK", 50.50m, "eur", "test@stripe.com", "Pay for testing");
        Console.WriteLine("result = ", result);
