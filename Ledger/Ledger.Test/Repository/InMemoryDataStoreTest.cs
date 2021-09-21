using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ledger.Models;
using Ledger.Repository;
using Ledger.Test.Mocks;
using Xunit;

namespace Ledger.Test.Repository
{
    public class InMemoryDataStoreTest
    {
        private Dictionary<Tuple<string, string>, LoanDetail> _loanRecords;
        private IDataStore _inMemoryDataStoreStore;

        public InMemoryDataStoreTest()
        {
            IntitalizeTest().GetAwaiter().GetResult();
        }

        private async Task IntitalizeTest()
        {
            _loanRecords = MockModels.GetLoadDetailMock();
            _inMemoryDataStoreStore = new InMemoryDataStore();
            foreach (var record in _loanRecords)
            {
                await _inMemoryDataStoreStore.SaveLoanDetailsAsync(record.Value);
            }
        }

        [Fact]
        public async Task GetLoanDetailsAsync_ValidRequest_Returns_ValidLoanDetail()
        {
            var loanRecordKey = _loanRecords.Keys.First();
            var bankName = loanRecordKey.Item1;
            var borrowerName = loanRecordKey.Item2;
            var expectedLoanDetail = _loanRecords[loanRecordKey];
            var actualOutput = await _inMemoryDataStoreStore.GetLoanDetailsAsync(bankName, borrowerName);
            actualOutput.Should().NotBeNull();
            actualOutput.Should().BeEquivalentTo(expectedLoanDetail);
        }

        [Fact]
        public async Task GetLoanDetailsAsync_InValidRequest_Returns_NullResponse()
        {
            var actualOutput = await _inMemoryDataStoreStore.GetLoanDetailsAsync("ABC", "XYZ");
            actualOutput.Should().BeNull();
        }

        [Fact]
        public async Task SaveLoanDetailsAsync_ValidRequest_Returns_True()
        {
            var loanDetailToSave = MockModels._loanDetailFaker.Generate(1).First();
            var isSaved = await _inMemoryDataStoreStore.SaveLoanDetailsAsync(loanDetailToSave);
            isSaved.Should().Be(true);
        }

        [Fact]
        public async Task SavePaymentAsync_ValidRequest_Returns_True()
        {
            var existingLoanDetail = _loanRecords.First().Value;
            var initialNoOfPayments = existingLoanDetail.Payments.Count;
            var paymentToSave = MockModels._paymentFaker.Generate(1).First();
            var isSaved = await _inMemoryDataStoreStore.SavePaymentAsync(existingLoanDetail.BankName, existingLoanDetail.BorrowerName, paymentToSave);
            isSaved.Should().Be(true);
            var updatedLoanDetail = await _inMemoryDataStoreStore.GetLoanDetailsAsync(existingLoanDetail.BankName, existingLoanDetail.BorrowerName);
            updatedLoanDetail.Payments.Count.Should().Be(initialNoOfPayments + 1);
        }

        [Fact]
        public async Task SavePaymentAsync_InValidRequest_Returns_False()
        {
            var paymentToSave = MockModels._paymentFaker.Generate(1).First();
            var isSaved = await _inMemoryDataStoreStore.SavePaymentAsync("InvalidBankName", "InvalidBorrowerName", paymentToSave);
            isSaved.Should().Be(false);
        }
    }
}