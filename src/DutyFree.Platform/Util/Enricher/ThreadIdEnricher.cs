﻿namespace DutyFree.Platform.Util.Enricher
{
    using System.Threading;
    using Serilog.Core;
    using Serilog.Events;

    public class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
