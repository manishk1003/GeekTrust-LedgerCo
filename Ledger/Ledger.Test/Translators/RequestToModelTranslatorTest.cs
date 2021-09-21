using System.Linq;
using FluentAssertions;
using Ledger.Request;
using Ledger.Test.Mocks;
using Ledger.Translators;
using Xunit;

namespace Ledger.Test.Translators
{
    public class RequestToModelTranslatorTest
    {
        [Fact]
        public void ToLoanDetailModel_WithNullInput_Returns_Null()
        {
            LoanRequest loanRequest = null;
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanDetail.Should().BeNull();
        }

        [Fact]
        public void ToLoanDetailModel_WithValidInput_Returns_ValidResponse()
        {
            LoanRequest loanRequest = MockRequests._loanFaker.Generate(1).First();
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanDetail.Should().NotBeNull();
            loanDetail.BankName.Should().Be(loanRequest.BankName);
            loanDetail.BorrowerName.Should().Be(loanRequest.BorrowerName);
            loanDetail.LoanTenure.Should().Be(loanRequest.LoanTenure);
            loanDetail.RateOfInterest.Should().Be(loanRequest.RateOfInterest);
        }

        [Fact]
        public void ToPaymentModel_WithNullInput_Returns_Null()
        {
            PaymentRequest paymentRequest = null;
            var payment = paymentRequest.ToPaymentModel();
            payment.Should().BeNull();
        }

        [Fact]
        public void ToPaymentModel_WithValidInput_Returns_ValidResponse()
        {
            PaymentRequest paymentRequest = MockRequests._paymentFaker.Generate(1).First();
            var payment = paymentRequest.ToPaymentModel();
            payment.Should().NotBeNull();
            payment.Amount.Should().Be(paymentRequest.LumpsumAmount);
            payment.EmiNumber.Should().Be(paymentRequest.Emi);
        }
    }
}