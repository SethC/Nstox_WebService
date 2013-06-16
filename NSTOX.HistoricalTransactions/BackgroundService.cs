using log4net;
using NSTOX.DataPusher;
using NSTOX.DataPusher.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NSTOX.HistoricalTransactions
{
    class BackgroundService : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundService));

        Timer scheduleTimer = null;

        private DateTime TimeToRun = ConfigurationHelper.TimeToRun;
        private int IntervalBetweenCalls = (int)TimeSpan.FromDays(1).TotalMilliseconds;

        private int GetIntervalToEventFire()
        {
            while (DateTime.Now > TimeToRun)
                TimeToRun = TimeToRun.AddDays(1);

            return (int)TimeToRun.Subtract(DateTime.Now).TotalMilliseconds;
        }

        public void InitializeTimer()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Dispose();
                scheduleTimer = null;
            }

            int timeToFirstExecution = GetIntervalToEventFire();
            log.Debug(string.Format("Background Service firing in {0} milliseconds ({1})", timeToFirstExecution, TimeToRun));

            scheduleTimer = new Timer(scheduleTimer_Elapsed, null, timeToFirstExecution, IntervalBetweenCalls);
        }

        void scheduleTimer_Elapsed(object sender)
        {
            try
            {
                log.Debug("Background Service Triggered");
                Pusher.RunJob();
                log.Debug("Background Service Completed");
            }
            finally
            {
                InitializeTimer();
            }
        }

        void IDisposable.Dispose()
        {
            if (scheduleTimer != null)
                scheduleTimer.Dispose();
        }
    }
}
