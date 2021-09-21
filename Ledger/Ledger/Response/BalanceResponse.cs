namespace Ledger.Response
{
    public class BalanceResponse : BaseResponse
    {
        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public decimal AmountPaid { get; set; }
        public int RemainingEmis { get; set; }
    }
}