using DotNetXxlJob.Model;

namespace DotNetXxlJob
{
    public abstract class AbstractJobHandler : IJobHandler
    {
        public abstract Task<ReturnT> Execute(JobExecuteContext context);

        public virtual void Dispose()
        {
        }
    }

    public interface IJobHandler : IDisposable
    {
        Task<ReturnT> Execute(JobExecuteContext context);
    }
}