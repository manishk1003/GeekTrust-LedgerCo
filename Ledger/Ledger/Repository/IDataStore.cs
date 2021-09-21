using System.Threading.Tasks;
using Ledger.Models;

namespace Ledger.Repository
{
    public interface IDataStore
    {
        Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail);

        Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName);

        Task<bool> SavePaymentAsync(string bankName, string borrowerName, Payment payment);
    }
}