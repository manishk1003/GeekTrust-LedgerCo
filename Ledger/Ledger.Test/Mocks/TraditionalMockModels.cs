using System;
using System.Collections.Generic;
using Ledger.Models;

namespace Ledger.Test.Mocks
{
    public static class TraditionalMockModels
    {
        public const string _bankName = "ICICI";
        public const string _borrowerName = "John Doe";

        public static Dictionary<Tuple<string, string>, LoanDetail> GetLoadDetailMock()
        {
            Dictionary<Tuple<string, string>, LoanDetail> loadDetailRecords = new Dictionary<Tuple<string, string>, LoanDetail>();

            Tuple<string, string> key = new Tuple<string, string>(_bankName, _borrowerName);
            LoanDetail loanDetail = GetLoanDetail(_bankName, _borrowerName);
            loadDetailRecords.Add(key, loanDetail);
            return loadDetailRecords;
        }

        private static LoanDetail GetLoanDetail(string bankName, string borrowerName)
        {
            return new LoanDetail()
            {
                BankName = bankName,
                BorrowerName = borrowerName,
                CreateDate = DateTime.UtcNow,
                LoanTenure = 2,
                PrincipalAmount = 10000,
                RateOfInterest = 10,
                Payments = GetPayments()
            };
        }

        public static List<Payment> GetPayments()
        {
            List<Payment> payments = new List<Payment>
            {
               GetPayment()
            };
            return payments;
        }

        public static Payment GetPayment()
        {
            return new Payment()
            {
                Amount = 200,
                EmiNumber = 3
            };
        }
    }
}