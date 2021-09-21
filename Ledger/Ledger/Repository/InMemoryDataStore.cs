using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Models;

namespace Ledger.Repository
{
    public class InMemoryDataStore : IDataStore
    {
        private static readonly Dictionary<Tuple<string, string>, LoanDetail> _loanRecords = new Dictionary<Tuple<string, string>, LoanDetail>();

        public async Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName)

        {
            var loanRecordKey = GetLoanRecordKey(bankName, borrowerName);
            var existingLoanDetails = await GetLoanRecordAsync(loanRecordKey);
            return existingLoanDetails;
        }

        public async Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail)
        {
            var loanRecordKey = GetLoanRecordKey(loanDetail.BankName, loanDetail.BorrowerName);
            return _loanRecords.TryAdd(loanRecordKey, loanDetail);
        }

        private Tuple<string, string> GetLoanRecordKey(string bankName, string borrowerName)
        {
            var loanRecordKey = new Tuple<string, string>(bankName, borrowerName);
            return loanRecordKey;
        }

        public async Task<bool> SavePaymentAsync(string bankName, string borrowerName, Payment payment)
        {
            var loanRecordKey = GetLoanRecordKey(bankName, borrowerName);
            var existingLoanDetails = await GetLoanRecordAsync(loanRecordKey);

            if (existingLoanDetails == null)
                return false;

            if (existingLoanDetails.Payments == null)
                existingLoanDetails.Payments = new List<Payment>();

            existingLoanDetails.Payments.Add(payment);
            return true;
        }

        public Task<LoanDetail> GetLoanRecordAsync(Tuple<string, string> loanRecordKey)
        {
            LoanDetail existingLoanDetails;
            if (_loanRecords.TryGetValue(loanRecordKey, out existingLoanDetails))
            {
                return Task.FromResult(existingLoanDetails);
            }
            return Task.FromResult(existingLoanDetails);
        }
    }
}