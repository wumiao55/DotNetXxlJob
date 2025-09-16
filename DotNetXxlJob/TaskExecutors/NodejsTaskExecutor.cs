using DotNetXxlJob.Internal;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// Nodejs任务执行器
    /// </summary>
    public class NodejsTaskExecutor : ScriptTaskExecutor
    {
        public NodejsTaskExecutor(IJobLogger jobLogger)
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_NODEJS;

        public override string ExecuteCmd => "node";

        public override string ExecuteDir => "Nodejs";

        public override string ExecuteFileExtension => ".js";
    }
}
