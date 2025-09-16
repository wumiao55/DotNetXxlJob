using System.Diagnostics;
using System.Text;

namespace DotNetXxlJob.Internal
{
    /// <summary>
    /// 脚本执行器（支持 sh, python, cmd, exe 等）
    /// </summary>
    internal static class ScriptExecutor
    {
        /// <summary>
        /// 通用异步执行外部命令（支持 sh, python, cmd, exe 等）
        /// </summary>
        /// <param name="executable">可执行文件名或路径，如 "sh", "python", "cmd"</param>
        /// <param name="arguments">参数数组，如 new[] { "backup.sh", "admin" }</param>
        /// <param name="workingDirectory">工作目录（脚本所在目录）</param>
        /// <param name="timeoutMs">超时毫秒数，默认 3 分钟</param>
        /// <param name="encoding">输出编码，默认 UTF-8</param>
        /// <returns>执行结果</returns>
        public static async Task<ScriptExecutionResult> ExecuteCommandAsync(
            string executable,
            string[] arguments,
            string? workingDirectory = null,
            int timeoutMs = 180_000,
            Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var startInfo = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = EscapeArguments(arguments),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = encoding,
                StandardErrorEncoding = encoding,
                WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory
            };

            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var process = Process.Start(startInfo);
                if (process == null)
                    return new ScriptExecutionResult { Success = false, Error = "无法启动进程" };

                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();
                var timeoutTask = Task.Delay(timeoutMs);
                var completedTask = await Task.WhenAny(
                    process.WaitForExitAsync(),
                    timeoutTask
                );

                if (completedTask == timeoutTask)
                {
                    process.Kill();
                    return new ScriptExecutionResult
                    {
                        Success = false,
                        Error = "执行超时",
                        Duration = stopwatch.Elapsed
                    };
                }

                string output = await outputTask;
                string error = await errorTask;

                stopwatch.Stop();

                return new ScriptExecutionResult
                {
                    Success = process.ExitCode == 0,
                    Output = output.Trim(),
                    Error = error.Trim(),
                    ExitCode = process.ExitCode,
                    Duration = stopwatch.Elapsed
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new ScriptExecutionResult
                {
                    Success = false,
                    Error = $"{ex.Message}",
                    Duration = stopwatch.Elapsed
                };
            }
        }

        /// <summary>
        /// 兼容重载：支持字符串参数（自动分割）
        /// </summary>
        /// <param name="executable">可执行文件名或路径，如 "sh", "python", "cmd"</param>
        /// <param name="arguments">参数数组，如 new[] { "backup.sh", "admin" }</param>
        /// <param name="workingDirectory">工作目录（脚本所在目录）</param>
        /// <param name="timeoutMs">超时毫秒数，默认 3 分钟</param>
        /// <param name="encoding">输出编码，默认 UTF-8</param>
        /// <returns>执行结果</returns>
        public static Task<ScriptExecutionResult> ExecuteCommandAsync(
            string executable,
            string arguments,
            string? workingDirectory = null,
            int timeoutMs = 180_000,
            Encoding? encoding = null)
        {
            // 简单按空格分割（不适用于含空格路径！推荐使用数组版本）
            var argsArray = arguments.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return ExecuteCommandAsync(executable, argsArray, workingDirectory, timeoutMs, encoding);
        }

        /// <summary>
        /// 安全拼接命令行参数（处理含空格路径）
        /// </summary>
        private static string EscapeArguments(string[] args)
        {
            return string.Join(" ", args.Select(arg =>
                arg.Contains(' ') ? $"\"{arg.Replace("\"", "\\\"")}\"" : arg));
        }
    }

    internal class ScriptExecutionResult
    {
        public bool Success { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public int ExitCode { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
