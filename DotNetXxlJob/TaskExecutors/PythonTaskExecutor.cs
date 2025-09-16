using System.Runtime.InteropServices;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// Python任务执行器
    /// </summary>
    public class PythonTaskExecutor : ScriptTaskExecutor
    {
        public PythonTaskExecutor(IJobLogger jobLogger) 
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_PYTHON;

        public override string ExecuteCmd => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "py" : "python";

        public override string ExecuteDir => "Python";

        public override string ExecuteFileExtension => ".py";
    }
}
