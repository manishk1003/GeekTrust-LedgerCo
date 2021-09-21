using Ledger.Response;

namespace Ledger.Test.Mocks
{
    public static class MockResponse
    {
        public static BalanceResponse GetValidBalanceResponseWithoutPayment()
        {
            return new BalanceResponse()
            {
                AmountPaid = 1000,
                BankName = "IDIDI",
                BorrowerName = "Dale",
                Message = null,
                RemainingEmis = 55,
                Success = true
            };
        }

        public static BalanceResponse GetValidBalanceResponseWithPayment()
        {
            return new BalanceResponse()
            {
                AmountPaid = 3652,
                BankName = "IDIDI",
                BorrowerName = "Dale",
                Message = null,
                RemainingEmis = 4,
                Success = true
            };
        }
    }
}