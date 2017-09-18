namespace DutyFree.Platform.Monitor
{
    using System.Diagnostics;
    using Util;
    using Microsoft.Extensions.Logging;
    using System;
    using Setting;

    //记录函数调用时间
    public class TimeMonitor : Stopwatch, IDisposable
    {
        private string methodName;
        private readonly string prefix;

        public const string RAW = "Raw";
        private static ILogger _logger = LoggerUtil.CreateLogger<TimeMonitor>();

        public TimeMonitor(string prefix = RAW)
        {
            if(bool.Parse(Setting.Instance.Get("IsMonitorServiceTime")))
            {
                Start();
                // get call stack
                StackTrace stackTrace = new StackTrace(null, false);

                // get calling method name
                methodName = stackTrace.GetFrames()[1].GetMethod().Name;

                this.prefix = prefix;
            }
        }

        public void Dispose()
        {
            if (bool.Parse(Setting.Instance.Get("IsMonitorServiceTime")))
            {
                Stop();
                var ts = Elapsed;
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                _logger.LogInformation(string.Format("{0}_{1}: {2}", methodName, prefix, elapsedTime));
            }
        }
    }
}
