using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ledger.Handlers;
using Ledger.Models;
using Ledger.Repository;
using Ledger.Request;
using Ledger.Resources;
using Ledger.Test.Mocks;
using Ledger.Translators;
using Moq;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class PaymentHandlerTest
    {
        private IRequestHandler _paymentHandler;
        private Mock<IDataStore> _dataStore;
        private PaymentRequest _paymentRequest;
        private LoanRequest _loanRequest;

        public PaymentHandlerTest()
        {
            InitializeTest();
        }

        private void InitializeTest()
        {
            _dataStore = new Mock<IDataStore>();
            _loanRequest = MockRequests._loanFaker.Generate(1).First();
            _paymentRequest = MockRequests._paymentFaker.Generate(1).First();
            _paymentHandler = new PaymentHandler(_paymentRequest, _dataStore.Object);
        }

        [Fact]
        public void HandleAsync_RequestWithInvalidKey_Throws_Exception()
        {
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_paymentRequest.BankName, _paymentRequest.BorrowerName)).ThrowsAsync(new ArgumentException(ErrorMessages.LoanRecordNotFound));
            Func<Task> func = async () => { await _paymentHandler.HandleAsync(); };
            func.Should().ThrowAsync<ArgumentException>().WithMessage(ErrorMessages.LoanRecordNotFound);
        }

        [Fact]
        public void HandleAsync_RequestWithInvalidEmi_Throws_Exception()
        {
            _paymentRequest.Emi = 10000000;
            _paymentHandler = new PaymentHandler(_paymentRequest, _dataStore.Object);
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_paymentRequest.BankName, _paymentRequest.BorrowerName)).ThrowsAsync(new ArgumentException(ErrorMessages.LoanRecordNotFound));
            Func<Task> func = async () => { await _paymentHandler.HandleAsync(); };
            func.Should().ThrowAsync<ArgumentException>().WithMessage(ErrorMessages.InvalidEmi);
        }

        [Fact]
        public async Task HandleAsync_ValidPaymentRequest_Returns_True()
        {
            var loanDetail = _loanRequest.ToLoanDetailModel();
            _dataStore.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync(loanDetail);
            _dataStore.Setup(x => x.SavePaymentAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsNotNull<Payment>())).ReturnsAsync(true);
            var response = await _paymentHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(true);
        }

        [Fact]
        public async Task HandleAsync_ValidPaymentRequest_Returns_False()
        {
            var loanDetail = _loanRequest.ToLoanDetailModel();
            _dataStore.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync(loanDetail);
            _dataStore.Setup(x => x.SavePaymentAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsNotNull<Payment>())).ReturnsAsync(false);
            var response = await _paymentHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(false);
        }
    }
}