using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NSTOX.DataPusher.Helper
{
    public static class Constants
    {
        public static string LocalPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
        }

        private static string transactionsPath;
        public static string TransactionsPath
        {
            get
            {
                if (string.IsNullOrEmpty(transactionsPath))
                {
                    transactionsPath = Path.Combine(LocalPath, "Transactions");
                    if (!Directory.Exists(transactionsPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(transactionsPath);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            transactionsPath = null;
                        }
                    }
                }
                return transactionsPath;
            }
        }

        public static string DateFormat = "MM/dd/yyyy";
    }
}
