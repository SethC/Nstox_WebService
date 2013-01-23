using NSTOX.DataPusher;
using NSTOX.DataPusher.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NSTOX.HistoricalTransactions
{
    class BackgroundService
    {
        Timer scheduleTimer;

        private DateTime TimeToRun = ConfigurationHelper.TimeToRun;
        private int IntervalBetweenCalls = (int)TimeSpan.FromDays(1).TotalMilliseconds;

        private int GetIntervalToEventFire()
        {
            while (DateTime.Now > TimeToRun)
                TimeToRun = TimeToRun.AddDays(1);

            return (int)TimeToRun.Subtract(DateTime.Now).TotalMilliseconds;
        }

        private void InitializeTimer()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Dispose();
                scheduleTimer = null;
            }

            int timeToFirstExecution = GetIntervalToEventFire();
            
            scheduleTimer = new Timer(scheduleTimer_Elapsed, null, timeToFirstExecution, IntervalBetweenCalls);
        }

        void scheduleTimer_Elapsed(object sender)
        {
            try
            {
                Pusher.RunJob();
            }
            finally
            {
                InitializeTimer();
            }
        }
    }
}
