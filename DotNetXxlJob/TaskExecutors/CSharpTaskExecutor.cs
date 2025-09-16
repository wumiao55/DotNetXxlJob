using DotNetXxlJob.Internal;
using DotNetXxlJob.Model;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// C#任务执行器
    /// </summary>
    public class CSharpTaskExecutor : ScriptTaskExecutor
    {
        public CSharpTaskExecutor(IJobLogger jobLogger)
            : base(jobLogger)
        {
        }

        public override string GlueType { get; } = Constants.GlueType.GLUE_CSHARP;

        public override string ExecuteCmd => "dotnet run";

        public override string ExecuteDir => "CSharp";

        public override string ExecuteFileExtension => ".cs";

        public override async Task<ReturnT> Execute(TriggerParam triggerParam, CancellationToken cancellationToken)
        {
            var executeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", ExecuteDir);
            var executeFile = Path.Combine(executeDir, $"{triggerParam.JobId}_{triggerParam.GlueUpdateTime}{ExecuteFileExtension}");
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

            // dotnet new console -n ConsoleApp_1
            executeResult = await ScriptExecutor.ExecuteCommandAsync($"dotnet", $"new console -n ConsoleApp_{triggerParam.JobId} --force", executeDir);
            if (!executeResult.Success)
            {
                return ReturnT.Failed($"'dotnet new console' script execute fail: {executeResult.Error}");
            }
            _jobLogger.Log("dotnet new console app success");
            // cp ../1_1757987700.cs Program.cs 
            executeResult = await ScriptExecutor.ExecuteCommandAsync($"cp", $"-f {executeFile} Program.cs", Path.Combine(executeDir, $"ConsoleApp_{triggerParam.JobId}"));
            if (!executeResult.Success)
            {
                return ReturnT.Failed($"'cp' script execute fail: {executeResult.Error}");
            }
            _jobLogger.Log("copy script file(*.cs) success");
            // dotnet run
            executeResult = await ScriptExecutor.ExecuteCommandAsync($"dotnet", $"run {triggerParam.ExecutorParams}", Path.Combine(executeDir, $"ConsoleApp_{triggerParam.JobId}"));
            if (!executeResult.Success)
            {
                return ReturnT.Failed($"'dotnet run' script execute fail: {executeResult.Error}");
            }
            _jobLogger.Log("dotnet run success");

            if (!string.IsNullOrEmpty(executeResult.Output))
            {
                _jobLogger.Log(Environment.NewLine + executeResult.Output);
            }
            return ReturnT.SUCCESS;
        }
    }
}
