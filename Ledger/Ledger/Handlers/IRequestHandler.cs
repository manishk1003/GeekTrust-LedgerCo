using System.Threading.Tasks;
using Ledger.Response;

namespace Ledger.Handlers
{
    public interface IRequestHandler
    {
        Task<BaseResponse> HandleAsync();
    }
}