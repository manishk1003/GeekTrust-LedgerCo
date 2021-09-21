using System.Linq;
using Ledger.Handlers;
using Ledger.Repository;
using Ledger.Request;

namespace Ledger.Factories
{
    public static class RequestHandlerFactory
    {
        public static IRequestHandler GetRequestHandler(string command)
        {
            IRequestHandler request = null;
            IDataStore dataStore = DataStoreFactory.GetDataStore(Enums.DataStoreType.InMemoryStore);
            string[] args = command.Split(" ");
            if (args != null && args.Any())
            {
                request = args[0] switch
                {
                    Constants.Actions.Loan => GetLoanHandler(args, dataStore),
                    Constants.Actions.Balance => GetBalanceHandler(args, dataStore),
                    Constants.Actions.Payment => GetPaymentHandler(args, dataStore),
                    _ => null
                };
            }
            return request;
        }

        private static LoanHandler GetLoanHandler(string[] args, IDataStore dataStore)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var principal = args.ElementAtOrDefault(3);
            decimal.TryParse(principal, out decimal principalAmount);
            var tenure = args.ElementAtOrDefault(4);
            int.TryParse(tenure, out int loanTenure);
            var rateOfInterest = args.ElementAtOrDefault(5);
            decimal.TryParse(rateOfInterest, out decimal roi);

            LoanRequest loanRequest = new LoanRequest(bankName, borrowerName, principalAmount, loanTenure, roi);
            LoanHandler loanHandler = new LoanHandler(loanRequest, dataStore);
            return loanHandler;
        }

        private static PaymentHandler GetPaymentHandler(string[] args, IDataStore dataStore)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var amount = args.ElementAtOrDefault(3);
            decimal.TryParse(amount, out decimal lumpSumAmount);
            var emi = args.ElementAtOrDefault(4);
            int.TryParse(emi, out int emiNo);

            PaymentRequest paymentRequest = new PaymentRequest(bankName, borrowerName, lumpSumAmount, emiNo);
            PaymentHandler loanHandler = new PaymentHandler(paymentRequest, dataStore);
            return loanHandler;
        }

        private static BalanceHandler GetBalanceHandler(string[] args, IDataStore dataStore)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var emi = args.ElementAtOrDefault(3);
            int.TryParse(emi, out int emiNo);

            BalanceRequest balanceRequest = new BalanceRequest(bankName, borrowerName, emiNo);
            BalanceHandler loanHandler = new BalanceHandler(balanceRequest, dataStore);
            return loanHandler;
        }
    }
}