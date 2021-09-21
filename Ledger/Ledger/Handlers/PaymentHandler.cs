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
    public class PaymentHandler : IRequestHandler
    {
        private readonly PaymentRequest _paymentRequest;
        private readonly IDataStore _dataStore;

        public PaymentHandler(PaymentRequest paymentRequest, IDataStore dataStore)
        {
            _paymentRequest = paymentRequest;
            _dataStore = dataStore;
        }

        public async Task<BaseResponse> HandleAsync()
        {
            var existingLoanRecord = await _dataStore.GetLoanDetailsAsync(_paymentRequest.BankName, _paymentRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new ArgumentException(ErrorMessages.LoanRecordNotFound);
            var totalValidEmis = existingLoanRecord.LoanTenure * 12;
            if (_paymentRequest.Emi > totalValidEmis)
                throw new ArgumentException(ErrorMessages.InvalidEmi);
            var payment = _paymentRequest.ToPaymentModel();
            var isSaved = await _dataStore.SavePaymentAsync(_paymentRequest.BankName, _paymentRequest.BorrowerName, payment);
            return new BaseResponse() { Success = isSaved };
        }
    }
}