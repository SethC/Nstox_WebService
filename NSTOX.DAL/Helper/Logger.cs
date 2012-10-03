using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NSTOX.DAL.Helper
{
    public class Logger
    {
        public static void LogException(Exception ex)
        {
            if (ex == null)
                return;

            Trace.WriteLine(string.Format("[ERROR] {0}", ex.ToString()));
        }

        public static void LogInfo(string message, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            Trace.WriteLine(string.Format("[{0}] {1}", entryType, message));
        }
    }
}
