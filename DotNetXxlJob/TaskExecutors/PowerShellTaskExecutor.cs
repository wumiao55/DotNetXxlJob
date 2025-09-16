using System.Runtime.InteropServices;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// PowerShell任务执行器
    /// </summary>
    public class PowerShellTaskExecutor : ScriptTaskExecutor
    {
        public PowerShellTaskExecutor(IJobLogger jobLogger)
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_POWERSHELL;

        public override string ExecuteCmd => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell" : "pwsh";

        public override string CmdArguments => "-File";

        public override string ExecuteDir => "PowerShell";

        public override string ExecuteFileExtension => ".ps1";
    }
}
