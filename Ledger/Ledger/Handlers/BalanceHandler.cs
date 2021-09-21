using System;
using System.Threading.Tasks;
using Ledger.Repository;
using Ledger.Request;
using Ledger.Resources;
using Ledger.Response;

namespace Ledger.Handlers
{
    public class BalanceHandler : IRequestHandler
    {
        private readonly BalanceRequest _balanceRequest;
        private readonly IDataStore _dataStore;

        public BalanceHandler(BalanceRequest balanceRequest, IDataStore dataStore)
        {
            _balanceRequest = balanceRequest;
            _dataStore = dataStore;
        }

        public async Task<BaseResponse> HandleAsync()
        {
            var existingLoanRecord = await _dataStore.GetLoanDetailsAsync(_balanceRequest.BankName, _balanceRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new ArgumentException(ErrorMessages.LoanRecordNotFound);

            var emiAmount = existingLoanRecord.EmiAmount();
            var totalAmountByEmi = _balanceRequest.Emi * emiAmount;
            var totalLumpSumPaid = existingLoanRecord.LumpSumPaidTillEmiNumber(_balanceRequest.Emi);

            var totalAmountPaidTillEmi = totalAmountByEmi + totalLumpSumPaid;

            var amountPending = existingLoanRecord.TotalAmountToBeRepaid() - totalAmountPaidTillEmi;
            var remainingEmis = Math.Ceiling(amountPending / emiAmount);

            BalanceResponse balanceResponse = new BalanceResponse()
            {
                AmountPaid = totalAmountPaidTillEmi,
                BankName = existingLoanRecord.BankName,
                BorrowerName = existingLoanRecord.BorrowerName,
                RemainingEmis = remainingEmis > 0 ? (int)remainingEmis : 0,
                Success = true
            };
            return balanceResponse;
        }
    }
}