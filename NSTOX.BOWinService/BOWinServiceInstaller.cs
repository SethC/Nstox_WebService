using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace NSTOX.BOWinService
{
    [RunInstaller(true)]
    public class BOWinServiceInstaller : Installer
    {
        private const string ServiceName = "NSTOX Service";
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public BOWinServiceInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = ServiceName;
            serviceInstaller.Description = "This service will run daily and collect data from BackOffice.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = ServiceName;

            serviceInstaller.AfterInstall += new InstallEventHandler(serviceInstaller_AfterInstall);

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

        void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            StartService();
        }

        private void StopService()
        {
            ServiceController sc = new ServiceController(ServiceName);
            if (sc != null && sc.Status == ServiceControllerStatus.Running)
                try
                {
                    sc.Stop();
                }
                catch { }
        }

        private void StartService()
        {
            ServiceController sc = new ServiceController(ServiceName);
            if (sc != null && sc.Status != ServiceControllerStatus.Running)
                try
                {
                    sc.Start();
                }
                catch { }
        }
    }
}
