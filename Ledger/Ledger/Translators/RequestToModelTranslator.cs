using System;
using Ledger.Models;
using Ledger.Request;

namespace Ledger.Translators
{
    public static class RequestToModelTranslator
    {
        public static LoanDetail ToLoanDetailModel(this LoanRequest loanRequest)
        {
            if (loanRequest == null)
                return null;
            return new LoanDetail()
            {
                BankName = loanRequest.BankName,
                BorrowerName = loanRequest.BorrowerName,
                LoanTenure = loanRequest.LoanTenure,
                PrincipalAmount = loanRequest.PrincipalAmount,
                RateOfInterest = loanRequest.RateOfInterest,
                CreateDate = DateTime.UtcNow
            };
        }

        public static Payment ToPaymentModel(this PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
                return null;
            return new Payment()
            {
                Amount = paymentRequest.LumpsumAmount,
                EmiNumber = paymentRequest.Emi
            };
        }
    }
}