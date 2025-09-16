using DotNetXxlJob.Internal;
using DotNetXxlJob.Model;

namespace DotNetXxlJob.TaskExecutors
{
    /// <summary>
    /// 脚本任务执行器
    /// </summary>
    public abstract class ScriptTaskExecutor : ITaskExecutor
    {
        private protected IJobLogger _jobLogger;

        public ScriptTaskExecutor(IJobLogger jobLogger)
        {
            this._jobLogger = jobLogger;
        }

        public virtual string GlueType => throw new NotImplementedException();

        public virtual string ExecuteCmd => throw new NotImplementedException();

        public virtual string CmdArguments => string.Empty;

        public virtual string ExecuteDir => throw new NotImplementedException();

        public virtual string ExecuteFileExtension => throw new NotImplementedException();

        public virtual async Task<ReturnT> Execute(TriggerParam triggerParam, CancellationToken cancellationToken)
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
            string arguments;
            if (!string.IsNullOrEmpty(CmdArguments))
            {
                arguments = $"{CmdArguments} {executeFile} {triggerParam.ExecutorParams}";
            }
            else
            {
                arguments = $"{executeFile} {triggerParam.ExecutorParams}";
            }
            var result = await ScriptExecutor.ExecuteCommandAsync(ExecuteCmd, arguments, executeDir);
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Output))
                {
                    _jobLogger.Log(Environment.NewLine + result.Output);
                }
                return ReturnT.SUCCESS;
            }
            if (!string.IsNullOrEmpty(result.Error))
            {
                _jobLogger.Log(result.Error);
            }
            if (result.ExitCode != 0)
            {
                return ReturnT.Failed($"script exit value({result.ExitCode}) is failed");
            }
            return ReturnT.Failed($"script execute fail: {result.Error}");
        }
    }
}
