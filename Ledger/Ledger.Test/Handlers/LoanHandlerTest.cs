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
using Moq;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class LoanHandlerTest
    {
        private IRequestHandler _loanHandler;
        private Mock<IDataStore> _dataStore;
        private LoanRequest _loanRequest;

        public LoanHandlerTest()
        {
            InitializeTet();
        }

        private void InitializeTet()
        {
            _dataStore = new Mock<IDataStore>();
            _loanRequest = MockRequests._loanFaker.Generate(1).First();
            _loanHandler = new LoanHandler(_loanRequest, _dataStore.Object);
        }

        [Fact]
        public void HandleAsync_RequestWithExistingKey_Throws_Exception()
        {
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_loanRequest.BankName, _loanRequest.BorrowerName)).ThrowsAsync(new ArgumentException(ErrorMessages.LoanRecodExists));
            Func<Task> func = async () => { await _loanHandler.HandleAsync(); };
            func.Should().ThrowAsync<ArgumentException>().WithMessage(ErrorMessages.LoanRecodExists);
        }

        [Fact]
        public async Task HandleAsync_ValidLoanRequest_Returns_True()
        {
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_loanRequest.BankName, _loanRequest.BorrowerName)).ReturnsAsync((LoanDetail)null);
            _dataStore.Setup(x => x.SaveLoanDetailsAsync(It.IsNotNull<LoanDetail>())).ReturnsAsync(true);
            var response = await _loanHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(true);
        }

        [Fact]
        public async Task HandleAsync_ValidLoanRequest_Returns_False()
        {
            _dataStore.Setup(x => x.GetLoanDetailsAsync(_loanRequest.BankName, _loanRequest.BorrowerName)).ReturnsAsync((LoanDetail)null);
            _dataStore.Setup(x => x.SaveLoanDetailsAsync(It.IsNotNull<LoanDetail>())).ReturnsAsync(false);
            var response = await _loanHandler.HandleAsync();
            response.Should().NotBeNull();
            response.Success.Should().Be(false);
        }
    }
}