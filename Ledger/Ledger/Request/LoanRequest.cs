namespace Ledger.Request
{
    public class LoanRequest : BaseRequest
    {
     

        public LoanRequest(string bankName, string borrowerName, decimal principalAmount, int loanTenure, decimal rateOfInterest)
        {
            BankName = bankName;
            BorrowerName = borrowerName;
            PrincipalAmount = principalAmount;
            LoanTenure = loanTenure;
            RateOfInterest = rateOfInterest;
        }

        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public decimal PrincipalAmount { get; set; }
        public int LoanTenure { get; set; }
        public decimal RateOfInterest { get; set; }

        public override bool Validate()
        {
            // Write Validation Rules here
            // Or can use Validation Framework For validating requests
            return true;
        }
    }
}