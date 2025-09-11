using System.Threading;
using System.Threading.Tasks;

namespace DotNetXxlJob
{
    public interface IExecutorRegistry
    {
        
        Task RegistryAsync(CancellationToken cancellationToken);

     
    }
}