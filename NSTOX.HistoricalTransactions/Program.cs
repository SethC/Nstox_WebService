using log4net;
using log4net.Core;
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

        static ILog log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            log = LogManager.GetLogger(typeof(Program));

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

            log.Debug("Starting HistoricalTransactions");

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
            log.Error("AppDomain Unhandler", e.ExceptionObject as Exception);
            MessageBox.Show(e.ExceptionObject.ToString());
        }
    }
}
