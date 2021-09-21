namespace Ledger.Request
{
    public class BalanceRequest : BaseRequest
    {
        public BalanceRequest(string bankName, string borrowerName, int emi)
        {
            BankName = bankName;
            BorrowerName = borrowerName;
            Emi = emi;
        }

        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public int Emi { get; set; }

        public override bool Validate()
        {
            // Write Validation Rules here
            // Or can use Validation Framework For validating requests
            return true;
        }
    }
}