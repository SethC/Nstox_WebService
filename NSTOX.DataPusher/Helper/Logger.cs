﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using log4net;

namespace NSTOX.DataPusher.Helper
{
    public delegate void OnLog(string msg);

    public static class Logger
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        private const string eventSource = "NSTOX";
        private const string eventLogName = "NSTOXLog";

        private static EventLog eventLog;

        private static StringBuilder messageSB;
        private static EventLogEntryType logType = EventLogEntryType.Information;

        public static event OnLog OnLogTriggered;

        static Logger()
        {
            try
            {
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, eventLogName);
                }
                eventLog = new EventLog(eventLogName);
                eventLog.Source = eventSource;
            }
            catch { }

            messageSB = new StringBuilder();
        }

        public static void LogException(Exception ex)
        {
            if (ex == null)
                return;
            if (eventLog != null)
                try
                {
                    log.Error("Exception", ex);
                    string msg = string.Format("{0}{2}{1}", ex.Message, ex.StackTrace, Environment.NewLine);
                    FireEventLogEvent(msg);

                    eventLog.WriteEntry(msg, EventLogEntryType.Error);
                }
                catch { }
        }

        public static void LogInfo(string message, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            string msg = string.Format("{0} -> {1}", DateTime.Now.ToString("HH:mm:ss"), message);
            log.Debug(msg);

            FireEventLogEvent(msg);

            messageSB.AppendLine(msg);

            if (entryType == EventLogEntryType.Warning)
            {
                logType = EventLogEntryType.Warning;
            }
        }

        public static void Flush()
        {
            if (eventLog != null)
                try
                {
                    eventLog.WriteEntry(messageSB.ToString(), logType);
                }
                catch { }

            messageSB = new StringBuilder();
            logType = EventLogEntryType.Information;
        }

        private static void FireEventLogEvent(string msg)
        {
            if (OnLogTriggered != null)
                OnLogTriggered(msg);
        }
    }
}
