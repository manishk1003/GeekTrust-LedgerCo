using System;
using System.Threading.Tasks;
using Ledger.Models;
using Ledger.Repository;
using Ledger.Request;
using Ledger.Resources;
using Ledger.Response;
using Ledger.Translators;

namespace Ledger.Handlers
{
    public class LoanHandler : IRequestHandler
    {
        private readonly LoanRequest _loanRequest;
        private readonly IDataStore _dataStore;

        public LoanHandler(LoanRequest loanRequest, IDataStore dataStore)
        {
            _loanRequest = loanRequest;
            _dataStore = dataStore;
        }

        public async Task<BaseResponse> HandleAsync()
        {
            var existingLoanDetails = await _dataStore.GetLoanDetailsAsync(_loanRequest.BankName, _loanRequest.BorrowerName);
            if (existingLoanDetails != null)
            {
                throw new ArgumentException(ErrorMessages.LoanRecodExists);
            }

            var loanDetail = _loanRequest.ToLoanDetailModel();
            var isSaved = await _dataStore.SaveLoanDetailsAsync(loanDetail);
            return new BaseResponse() { Success = isSaved };
        }
    }
}