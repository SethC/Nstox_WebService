using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading;

namespace NSTOX.HistoricalTransactions
{
    class ClickOnceUpdater
    {
        System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromDays(2).TotalMilliseconds);

        public ClickOnceUpdater()
        {
            t.Elapsed += t_Elapsed;
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Update();
        }

        public void UpdateAsync()
        {
            ThreadPool.QueueUserWorkItem((a) => { Update(); });
        }

        public bool Update()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var deployment = ApplicationDeployment.CurrentDeployment;
                return deployment.Update();
            }
            else
                return false;
        }
    }
}
