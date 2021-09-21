using FluentAssertions;
using Ledger.Factories;
using Ledger.Handlers;
using Xunit;

namespace Ledger.Test.Factories
{
    public class RequestHandlerFactoryTest
    {
        [Fact]
        public void GetRequestHandler_WithLoanCommand_Returns_LoanHandler()
        {
            string command = $"{Constants.Actions.Loan} IDIDI Dale 5000 1 6";
            var handler = RequestHandlerFactory.GetRequestHandler(command);
            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(LoanHandler));
        }

        [Fact]
        public void GetRequestHandler_WithPaymentCommand_Returns_PaymentHandler()
        {
            string command = $"{Constants.Actions.Payment} IDIDI Dale 1000 5";
            var handler = RequestHandlerFactory.GetRequestHandler(command);
            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(PaymentHandler));
        }

        [Fact]
        public void GetRequestHandler_WithBalanceCommand_Returns_BalanceHandler()
        {
            string command = $"{Constants.Actions.Balance} IDIDI Dale 3";
            var handler = RequestHandlerFactory.GetRequestHandler(command);
            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(BalanceHandler));
        }
    }
}