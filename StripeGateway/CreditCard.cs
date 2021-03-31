using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment
{
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public int ExpiredYear { get; set; }
        public int ExpiredMonth { get; set; }
        public string CVV { get; set; }

        public CreditCard(string cardNumber, int expiredYear, int expiredMonth, string cvv)
        {
            CardNumber = cardNumber;
            ExpiredYear = expiredYear;
            ExpiredMonth = expiredMonth;
            CVV = cvv;
        }
    }
}
