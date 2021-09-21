using System.Threading.Tasks;

namespace Ledger.Processors
{
    public interface IProcessor
    {
        Task ProcessAsync();
    }
}