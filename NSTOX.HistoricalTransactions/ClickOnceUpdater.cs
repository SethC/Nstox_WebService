using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NSTOX.HistoricalTransactions
{
    class ClickOnceUpdater
    {
        System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromDays(1).TotalMilliseconds);

        public ClickOnceUpdater()
        {
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Update())
            {
                Application.Restart();
            }
        }

        public void UpdateAsync()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var deployment = ApplicationDeployment.CurrentDeployment;
                deployment.UpdateAsync();
            }
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
