using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using NSTOX.DataPusher;
using NSTOX.DataPusher.Helper;
//using System.Timers;

namespace NSTOX.BOWinService
{
    public partial class BOWinService : ServiceBase
    {
        private DateTime TimeToRun = ConfigurationHelper.TimeToRun;
        private DateTime DateTimeNow { get { return DateTime.Now; } }
        private int IntervalBetweenCalls = (int)new TimeSpan(24, 0, 0).TotalMilliseconds;
        //private int IntervalBetweenCalls = (int)new TimeSpan(0, 0, 20).TotalMilliseconds;

        Timer scheduleTimer;

        public BOWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].ToUpper() == "DEBUG")
            {
                Debugger.Launch();
            }

            if (ConfigurationHelper.RetailerId <= 0)
            {
                throw new ArgumentException("Retailer Id is not set!");
            }
            InitilizeTimer();
        }

        void scheduleTimer_Elapsed(object sender)
        {
            Pusher.RunJob();
            // we reinitialize the timer
            InitilizeTimer();
        }

        protected override void OnStop()
        {
            scheduleTimer.Dispose();
        }

        private int GetIntervalToEventFire()
        {
            if (DateTimeNow > TimeToRun)
                TimeToRun = TimeToRun.AddDays(1);

            return (int)TimeToRun.Subtract(DateTimeNow).TotalMilliseconds;
        }

        private void InitilizeTimer()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Dispose();
                scheduleTimer = null;
            }

            int timeToFirstExecution = GetIntervalToEventFire();

            scheduleTimer = new Timer(scheduleTimer_Elapsed, null, timeToFirstExecution, IntervalBetweenCalls);
        }

    }
}
