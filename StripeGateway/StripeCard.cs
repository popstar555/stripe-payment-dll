
namespace StripePayment
{
    public class StripeCard
    {
        public string CardNumber { get; set; }
        public int ExpiredYear { get; set; }
        public int ExpiredMonth { get; set; }
        public string CVV { get; set; }

        public StripeCard(string cardNumber, int expiredYear, int expiredMonth, string cvv)
        {
            CardNumber = cardNumber;
            ExpiredYear = expiredYear;
            ExpiredMonth = expiredMonth;
            CVV = cvv;
        }
    }
}
