using DotNetXxlJob.Model;

namespace DotNetXxlJob.DefaultHandlers
{
    public abstract class AbsJobHandler:IJobHandler
    {
        public virtual void Dispose()
        {
           
        }

        public abstract Task<ReturnT> Execute(JobExecuteContext context);

    }
}