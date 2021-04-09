
using System;
using System.Globalization;

namespace StripePayemnt
{
    public class Payment
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        private decimal _Amount;
        private bool _bAmount=false;
        public decimal Amount 
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
                _bAmount = true;
            }
        }

        private decimal _AmountRefunded;
        private bool _bAmountRefunded = false;
        public decimal AmountRefunded
        {
            get
            {
                return _AmountRefunded;
            }
            set
            {
                _AmountRefunded = value;
                _bAmountRefunded = true;
            }
        }
        public string Currency { get; set; }

        private decimal _ConvertedAmount;
        private bool _bConvertedAmount = false;
        public decimal ConvertedAmount
        {
            get
            {
                return _ConvertedAmount;
            }
            set
            {
                _ConvertedAmount = value;
                _bConvertedAmount = true;
            }
        }

        private decimal _ConvertedAmountRefunded;
        private bool _bConvertedAmountRefunded = false;
        public decimal ConvertedAmountRefunded
        {
            get
            {
                return _ConvertedAmountRefunded;
            }
            set
            {
                _ConvertedAmountRefunded = value;
                _bConvertedAmountRefunded = true;
            }
        }

        private decimal _Fee;
        private bool _bFee = false;
        public decimal Fee
        {
            get
            {
                return _Fee;
            }
            set
            {
                _Fee = value;
                _bFee = true;
            }
        }

        private decimal _Tax;
        private bool _bTax = false;
        public decimal Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                _Tax = value;
                _bTax = true;
            }
        }
        public string ConvertedCurrency { get; set; }
        public string Status { get; set; }
        public string StatementDescriptor { get; set; }
        public string CustomerId { get; set; }
        public string CustomerDescription { get; set; }
        public string CustomerEmail { get; set; }

        private bool _Captured;
        private bool _bCaptured = false;
        public bool Captured
        {
            get
            {
                return _Captured;
            }
            set
            {
                _Captured = value;
                _bCaptured = true;
            }
        }
        public string CardId { get; set; }
        public string InvoiceId { get; set; }
        //public string Transfer { get; set; }

        public string[] ToArray()
        {
            string[] record = new string[19];
            CultureInfo ci = new CultureInfo("en-us");

            record[0] = Id;
            record[1] = "\""+Description+"\"";
            record[2] = Created.ToString("yyyy-MM-dd HH:mm");
            record[3] = _bAmount?Amount.ToString("0.00", ci):"";
            record[4] = _bAmountRefunded?AmountRefunded.ToString("0.00", ci):"";
            record[5] = Currency;
            record[6] = _bConvertedAmount ? ConvertedAmount.ToString("0.00", ci) : "";
            record[7] = _bConvertedAmountRefunded ? ConvertedAmountRefunded.ToString("0.00", ci) : "";
            record[8] = _bFee ? Fee.ToString("0.00", ci) : "";
            record[9] = _bTax ? Tax.ToString("0.00", ci) : "";
            record[10] = ConvertedCurrency;
            record[11] = FormatStatusString(Status);
            record[12] = "\""+StatementDescriptor+ "\"";
            record[13] = CustomerId;
            record[14] = "\""+CustomerDescription+ "\"";
            record[15] = CustomerEmail;
            record[16] = _bCaptured ? Captured.ToString():"";
            record[17] = CardId;
            record[18] = InvoiceId;
            //record[18] = Transfer;

            return record;
        }

        private string FormatStatusString(string status)
        {
            if(status== "succeeded")
            {
                return "Paid";
            }
            else if(status== "processing")
            {
                return "Processing";
            }
            else
            {
                return "Incomplete";
            }
        }
    }
}
