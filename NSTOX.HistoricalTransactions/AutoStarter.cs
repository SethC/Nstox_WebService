using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSTOX.HistoricalTransactions
{
    class AutoStarter
    {
        public AutoStarter()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                // The path to the key where Windows looks for startup applications
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(
                                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                //Path to launch shortcut
                string startPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                                   + @"\NStox\NStox Uploader.appref-ms";

                rkApp.SetValue("NStox", startPath);
            }
        }
    }
}
