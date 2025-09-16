using DotNetXxlJob.Internal;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// Shell任务执行器
    /// </summary>
    public class ShellTaskExecutor : ScriptTaskExecutor
    {
        public ShellTaskExecutor(IJobLogger jobLogger)
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_SHELL;

        public override string ExecuteCmd => "sh";

        public override string ExecuteDir => "Shell";

        public override string ExecuteFileExtension => ".sh";
    }
}
