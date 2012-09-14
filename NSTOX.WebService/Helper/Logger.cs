using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace NSTOX.WebService.Helper
{
    public class Logger
    {
        private const string eventSource = "NSTOX";
        private const string eventLogName = "Application";

        private static EventLog eventLog;

        private static StringBuilder messageSB;
        private static EventLogEntryType logType = EventLogEntryType.Information;

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
            catch (Exception ex)
            {
                LogException(ex);
            }

            messageSB = new StringBuilder();
        }

        public static void LogException(Exception ex)
        {
            if (ex == null)
                return;

            if (eventLog != null)
                try
                {
                    eventLog.WriteEntry(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace), EventLogEntryType.Error);
                }
                catch { }
            try
            {
                Trace.WriteLine(string.Format("{0} [ERROR] {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), ex.Message));
            }
            catch { }
        }

        public static void LogInfo(string message, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            messageSB.AppendLine(string.Format("{0} -> {1}", DateTime.Now.ToString("HH:mm:ss"), message));

            try
            {
                Trace.WriteLine(string.Format("{0} [{1}] {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), entryType, message));
            }
            catch { }

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

            messageSB.Clear();
            logType = EventLogEntryType.Information;
        }
    }
}