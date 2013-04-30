using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace NSTOX.DataPusher.Helper
{
    public static class ExtractUtilityHelper
    {
        private static string AppPath { get { return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); } }

        public static void RunExtractUtility(string arguments)
        {
            RunCommand(ConfigurationHelper.UtilityPath, arguments);
        }

        public static string ExtractTransactionXML()
        {
            DateTime transactionDate = ConfigurationHelper.TransactionDate;

            string dataFolder = Path.Combine(AppPath, "Data");
            string transXML = Path.Combine(dataFolder, "transactions.xml");

            SafelyDeleteFile(transXML);

            QDXTransactionToXML(transactionDate, transXML);
            return transXML;
        }

        public static bool QDXTransactionToXML(DateTime date, string destFile)
        {
            string year = date.ToString("yyyy").Substring(3);

            string qdxZipFile = Path.Combine(ConfigurationHelper.BAKPath, string.Format("TRB{0}{1}.ZIP", year, date.ToString("MMdd")));

            if (File.Exists(qdxZipFile))
            {
                Logger.LogInfo(string.Format("PROCESSING: Transaction for {0}", date.ToString(Constants.DateFormat)));

                List<byte[]> qdxFile = BOFilesHelper.DeCompress(qdxZipFile);

                if (qdxFile != null && qdxFile.Count > 0)
                {
                    string dataFolder = Path.GetDirectoryName(destFile);

                    if (!Directory.Exists(dataFolder))
                        Directory.CreateDirectory(dataFolder);

                    string transQDX = Path.Combine(dataFolder, "transactions.qdx");
                    string transXML = destFile;

                    SafelyDeleteFile(transQDX);

                    File.WriteAllBytes(transQDX, qdxFile[0]);

                    string tfParam = string.Format("/REPROCESS OUTPUTFILEPATH=\"{0}\" INPUTFILEPATH=\"{1}\" TV=1 UNIQUE=11 WORKINGDATE=01/01/2000 STORENUMBER=12 CIMGFILEPATH=\"{1}\"", transXML, transQDX);

                    RunCommand(ConfigurationHelper.TFServerPath, tfParam);

                    SafelyDeleteFile(transQDX);

                    return true;
                }
            }
            else
            {
                Logger.LogInfo(string.Format("NOT FOUND: Transaction for {0}.", date.ToString(Constants.DateFormat)));
            }
            return false;
        }

        private static void RunCommand(string fileName, string arguments)
        {
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = fileName;
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.Arguments = arguments;

                    p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                    p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

                    p.Start();
                    p.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw ex;
            }
        }

        public static void SafelyDeleteFile(string file)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch { }
        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.LogInfo(e.Data);
            Logger.Flush();
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.LogInfo(e.Data);
            Logger.Flush();
        }
    }
}
