using System;
using System.Collections.Generic;
using Bogus;
using Ledger.Models;

namespace Ledger.Test.Mocks
{
    public static class MockModels
    {
        public static Faker<Payment> _paymentFaker = new Faker<Payment>()
              .CustomInstantiator(f => new Payment())
              .RuleFor(o => o.Amount, f => f.Finance.Amount(100, 300))
              .RuleFor(o => o.EmiNumber, f => f.Random.Number(1, 9));

        public static Faker<LoanDetail> _loanDetailFaker = new Faker<LoanDetail>()
               .CustomInstantiator(f => new LoanDetail())
               .RuleFor(o => o.BankName, f => f.Finance.Bic())
               .RuleFor(o => o.BorrowerName, f => f.Name.FullName())
               .RuleFor(o => o.CreateDate, DateTime.UtcNow)
               .RuleFor(o => o.LoanTenure, f => f.Random.Number(1, 4))
               .RuleFor(o => o.PrincipalAmount, f => f.Finance.Amount(10000, 20000))
               .RuleFor(o => o.RateOfInterest, f => f.Random.Decimal(5, 10))
               .RuleFor(o => o.Payments, f => _paymentFaker.Generate(1))
               .FinishWith((f, o) => _loanGeneratedIds.Add(new Tuple<string, string>(o.BankName, o.BorrowerName)));

        public static List<Tuple<string, string>> _loanGeneratedIds = new List<Tuple<string, string>>();

        public static Dictionary<Tuple<string, string>, LoanDetail> GetLoadDetailMock()
        {
            Dictionary<Tuple<string, string>, LoanDetail> loadDetailRecords = new Dictionary<Tuple<string, string>, LoanDetail>();
            var loanDetails = GetLoanDetail();
            foreach (var loanDetail in loanDetails)
            {
                var recordId = new Tuple<string, string>(loanDetail.BankName, loanDetail.BorrowerName);
                loadDetailRecords.Add(recordId, loanDetail);
            }
            return loadDetailRecords;
        }

        public static List<LoanDetail> GetLoanDetail()
        {
            var loanDetails = _loanDetailFaker.Generate(5);
            return loanDetails;
        }

        public static List<Payment> GetLumpSumPayments()
        {
            var payments = _paymentFaker.Generate(2);
            return payments;
        }
    }
}