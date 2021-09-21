using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ledger.Handlers;
using Ledger.Repository;
using Ledger.Request;
using Ledger.Resources;
using Ledger.Test.Mocks;
using Ledger.Translators;
using Moq;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class BalanceHandlerTest
    {
        private IRequestHandler _balanceHandler;
        private Mock<IDataStore> _dataStore;
        private BalanceRequest _balanceRequest;
        private LoanRequest _loanRequest;

        public BalanceHandlerTest()
        {
            InitializeTest();
        }

        private void InitializeTest()
        {
            _dataStore = new Mock<IDataStore>();
            _loanRequest = MockRequests._loanFaker.Generate(1).First();
            _balanceRequest = MockRequests._balanceFaker.Generate(1).First();
            _balanceHandler = new BalanceHandler(_balanceRequest, _dataStore.Object);
        }

        [Fact]
        public void HandleAsync_RequestWithInvalidKey_Throws_Exception()
        {
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_balanceRequest.BankName, _balanceRequest.BorrowerName)).ThrowsAsync(new ArgumentException(ErrorMessages.LoanRecordNotFound));
            Func<Task> func = async () => { await _balanceHandler.HandleAsync(); };
            func.Should().ThrowAsync<ArgumentException>().WithMessage(ErrorMessages.LoanRecordNotFound);
        }

        [Fact]
        public async Task HandleAsync_ValidBalanceRequest_Returns_ValidBalanceReponse()
        {
            var bankName = "IDIDI";
            var borrowerName = "Dale";
            var loanRequest = new LoanRequest(bankName, borrowerName, 10000, 5, 4);
            var loanDetail = loanRequest.ToLoanDetailModel();
            var balanceRequest = new BalanceRequest(bankName, borrowerName, 5);
            _balanceHandler = new BalanceHandler(balanceRequest, _dataStore.Object);
            var validBalanceResponse = MockResponse.GetValidBalanceResponseWithoutPayment();
            _dataStore.Setup(x => x.GetLoanDetailsAsync(bankName, borrowerName)).ReturnsAsync(loanDetail);
            var response = await _balanceHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(true);
            response.Should().BeEquivalentTo(validBalanceResponse);
        }

        [Fact]
        public async Task HandleAsync_ValidBalanceRequestWithLumpsumPayment_Returns_ValidBalanceReponse()
        {
            var bankName = "IDIDI";
            var borrowerName = "Dale";
            var loanRequest = new LoanRequest(bankName, borrowerName, 5000, 1, 6);
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanDetail.Payments = new List<Models.Payment>();
            loanDetail.Payments.Add(new Models.Payment() { Amount = 1000, EmiNumber = 5 });

            var balanceRequest = new BalanceRequest(bankName, borrowerName, 6);
            _balanceHandler = new BalanceHandler(balanceRequest, _dataStore.Object);
            var validBalanceResponse = MockResponse.GetValidBalanceResponseWithPayment();
            _dataStore.Setup(x => x.GetLoanDetailsAsync(bankName, borrowerName)).ReturnsAsync(loanDetail);
            var response = await _balanceHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(true);
            response.Should().BeEquivalentTo(validBalanceResponse);
        }
    }
}