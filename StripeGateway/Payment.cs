
using System;
using System.Globalization;

namespace StripePayemnt
{
    public class Payment
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountRefunded { get; set; }
        public string Currency { get; set; }
        public decimal ConvertedAmountRefunded { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }
        public string ConvertedCurrency { get; set; }
        public string Status { get; set; }
        public string StatementDescriptor { get; set; }
        public string CustomerId { get; set; }
        public string CustomerDescription { get; set; }
        public string CustomerEmail { get; set; }
        public bool Captured { get; set; }
        //public string CardId { get; set; }
        public string InvoiceId { get; set; }
        //public string Transfer { get; set; }

        public string[] ToArray()
        {
            string[] record = new string[17];
            CultureInfo ci = new CultureInfo("en-us");

            record[0] = Id;
            record[1] = "\""+Description+"\"";
            record[2] = Created.ToString("yyyy-MM-dd HH:mm");
            record[3] = "\""+Amount.ToString("0.00", ci) + "\"";
            record[4] = "\"" + AmountRefunded.ToString("0.00", ci) + "\"";
            record[5] = Currency;
            record[6] = "\"" + ConvertedAmountRefunded.ToString("0.00", ci) + "\"";
            record[7] = "\"" + Fee.ToString("0.00", ci) + "\"";
            record[8] = "\"" + Tax.ToString("0.00", ci) + "\"";
            record[9] = ConvertedCurrency;
            record[10] = Status;
            record[11] = "\""+StatementDescriptor+ "\"";
            record[12] = CustomerId;
            record[13] = "\""+CustomerDescription+ "\"";
            record[14] = CustomerEmail;
            record[15] = Captured.ToString();
            record[16] = InvoiceId;

            return record;
        }
    }
}
