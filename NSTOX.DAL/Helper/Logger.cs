using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NSTOX.DAL.Helper
{
    public class Logger
    {
        private const string eventSource = "NSTOX";
        private const string eventLogName = "Application";

        private static EventLog eventLog = null;

        private static StringBuilder messageSB;
        private static EventLogEntryType logType = EventLogEntryType.Information;

        static Logger()
        {
            messageSB = new StringBuilder();
        }

        public static void LogException(Exception ex)
        {
            if (ex == null)
                return;

            if (eventLog != null)
                eventLog.WriteEntry(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace), EventLogEntryType.Error);

            Trace.WriteLine(string.Format("[ERROR] {0} {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), ex.Message));
        }

        public static void LogInfo(string message, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            messageSB.AppendLine(string.Format("{0} -> {1}", DateTime.Now.ToString("HH:mm:ss"), message));

            Trace.WriteLine(string.Format("[{0}] {1} {2}", entryType, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message));

            if (entryType == EventLogEntryType.Warning)
            {
                logType = EventLogEntryType.Warning;
            }
        }

        public static void Flush()
        {
            if (eventLog != null)
                eventLog.WriteEntry(messageSB.ToString(), logType);

            messageSB = new StringBuilder();
            logType = EventLogEntryType.Information;
        }
    }
}
