using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace NSTOX.HistoricalTransactions
{
    static class Program
    {
        static Mutex instanceMutex = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ApplicationExit += Application_ApplicationExit;
            bool createdNew;
            instanceMutex = new Mutex(true, @"Local\" + Assembly.GetExecutingAssembly().GetType().GUID, out createdNew);
            if (!createdNew)
            {
                instanceMutex = null;
                SingleProgramInstance spi = new SingleProgramInstance();
                spi.RaiseOtherProcess();
                return;
            }

            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form menuForm = new MenuForm();
            Application.AddMessageFilter(new MyMessageFilter(menuForm));
            Application.Run(menuForm);
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (instanceMutex != null)
            {
                instanceMutex.Close();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }
    }
}
