using DotNetXxlJob.Model;

namespace DotNetXxlJob
{
    public interface ITaskExecutor
    {
        string GlueType { get; }

        Task<ReturnT> Execute(TriggerParam triggerParam, CancellationToken cancellationToken);
    }
}