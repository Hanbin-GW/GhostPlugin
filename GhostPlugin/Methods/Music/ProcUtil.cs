using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace GhostPlugin.Methods.Music
{
    public static class ProcUtil
    {
        public static async Task<(int Exit, string Out, string Err)> RunAsync(
            string fileName, string args, string? workDir = null, int timeoutMs = 180_000)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                WorkingDirectory = workDir ?? Environment.CurrentDirectory,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var p = new Process { StartInfo = psi, EnableRaisingEvents = true };
            var sbOut = new StringBuilder();
            var sbErr = new StringBuilder();

            var tcs = new TaskCompletionSource<int>();
            p.OutputDataReceived += (_, e) => { if (e.Data != null) sbOut.AppendLine(e.Data); };
            p.ErrorDataReceived  += (_, e) => { if (e.Data != null) sbErr.AppendLine(e.Data); };
            p.Exited += (_, __) => tcs.TrySetResult(p.ExitCode);

            if (!p.Start()) throw new InvalidOperationException($"Failed to start: {fileName}");
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            using var cts = new CancellationTokenSource(timeoutMs);
            using (cts.Token.Register(() => tcs.TrySetResult(int.MinValue)))
            {
                var exit = await tcs.Task.ConfigureAwait(false);
                if (exit == int.MinValue)
                {
                    try { if (!p.HasExited) p.Kill(); } catch {}
                    throw new TimeoutException($"{fileName} timed out after {timeoutMs}ms");
                }
                return (exit, sbOut.ToString(), sbErr.ToString());
            }
        }
    }
}