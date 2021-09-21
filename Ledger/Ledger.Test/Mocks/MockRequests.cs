using System;
using System.Collections.Generic;
using Bogus;
using Ledger.Request;

namespace Ledger.Test.Mocks
{
    public static class MockRequests
    {
        public static List<Tuple<string, string>> _requestedLoanIds = new List<Tuple<string, string>>();

        public static Faker<LoanRequest> _loanFaker = new Faker<LoanRequest>()
                                        .CustomInstantiator(f => new LoanRequest(
                                            f.Finance.Bic(),
                                            f.Name.FullName(),
                                            f.Finance.Amount(10000, 20000),
                                            f.Random.Number(1, 4),
                                            f.Random.Decimal(5, 10)
                                        ))
                                        .FinishWith((f, o) => _requestedLoanIds.Add(new Tuple<string, string>(o.BankName, o.BorrowerName)));

        public static Faker<PaymentRequest> _paymentFaker = new Faker<PaymentRequest>()
                                            .CustomInstantiator(f => new PaymentRequest(null, null, 0, 0))
                                            .RuleFor(o => o.BankName, f => f.PickRandom(_requestedLoanIds).Item1)
                                            .RuleFor(o => o.BorrowerName, (f, o) => _requestedLoanIds.Find(x => x.Item1 == o.BankName).Item2)
                                            .RuleFor(o => o.LumpsumAmount, f => f.Finance.Amount(100, 300))
                                            .RuleFor(o => o.Emi, f => f.Random.Number(1, 9));

        public static Faker<BalanceRequest> _balanceFaker = new Faker<BalanceRequest>()
                                           .CustomInstantiator(f => new BalanceRequest(null, null, 0))
                                           .RuleFor(o => o.BankName, f => f.PickRandom(_requestedLoanIds).Item1)
                                           .RuleFor(o => o.BorrowerName, (f, o) => _requestedLoanIds.Find(x => x.Item1 == o.BankName).Item2)
                                           .RuleFor(o => o.Emi, f => f.Random.Number(1, 9));

        public static List<LoanRequest> GetLoanRequests()
        {
            var loanRequests = _loanFaker.Generate(5);
            return loanRequests;
        }

        public static List<PaymentRequest> GetPaymentRequests()
        {
            var paymentRqsts = _paymentFaker.Generate(5);
            return paymentRqsts;
        }

        public static List<BalanceRequest> GetBalanceRequests()
        {
            var balanceRequests = _balanceFaker.Generate(5);
            return balanceRequests;
        }
    }
}