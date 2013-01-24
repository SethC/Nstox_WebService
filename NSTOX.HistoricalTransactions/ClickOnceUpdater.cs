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
        public void UpdateAsync()
        {
            ThreadPool.QueueUserWorkItem((a) => { Update(); });
        }

        public bool Update()
        {
            var deployment = ApplicationDeployment.CurrentDeployment;
            return deployment.Update();
        }
    }
}
