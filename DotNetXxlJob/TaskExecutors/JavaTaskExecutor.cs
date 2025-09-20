using DotNetXxlJob.Internal;
using DotNetXxlJob.Model;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// Java任务执行器
    /// </summary>
    public class JavaTaskExecutor : ScriptTaskExecutor
    {
        private readonly string _className = "Application";

        public JavaTaskExecutor(IJobLogger jobLogger)
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_GROOVY;

        public override string ExecuteCmd => "java";

        public override string ExecuteDir => "Java";

        public override string ExecuteFileExtension => ".java";

        public override async Task<ReturnT> Execute(TriggerParam triggerParam, CancellationToken cancellationToken)
        {
            var executeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", ExecuteDir, $"{triggerParam.JobId}_{triggerParam.GlueUpdateTime}");
            var executeFile = Path.Combine(executeDir, $"{_className}{ExecuteFileExtension}");
            if (!Directory.Exists(executeDir))
            {
                Directory.CreateDirectory(executeDir);
            }
            if (!File.Exists(executeFile))
            {
                await File.WriteAllTextAsync(executeFile, triggerParam.GlueSource, cancellationToken);
            }
            _jobLogger.Log($"----------- script file:{executeFile} -----------");

            ScriptExecutionResult executeResult;

            // javac Application.java
            executeResult = await ScriptExecutor.ExecuteCommandAsync($"javac", $"{_className}{ExecuteFileExtension}", executeDir);
            if (!executeResult.Success)
            {
                return ReturnT.Failed($"'javac' script execute fail: {executeResult.Error}");
            }
            _jobLogger.Log("java compile success");
            // java Application
            executeResult = await ScriptExecutor.ExecuteCommandAsync(ExecuteCmd, $"{_className} {triggerParam.ExecutorParams}", executeDir);
            if (!executeResult.Success)
            {
                return ReturnT.Failed($"'java' script execute fail: {executeResult.Error}");
            }
            _jobLogger.Log("java run success");

            if (!string.IsNullOrEmpty(executeResult.Output))
            {
                _jobLogger.Log(Environment.NewLine + executeResult.Output);
            }
            return ReturnT.SUCCESS;
        }
    }
}
