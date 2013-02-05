using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NSTOX.DataPusher.Helper
{
    public static class ConfigurationHelper
    {
        public static string UtilityPath { get { return GetConfigValue("UtilityPath"); } }

        public static string TFServerPath { get { return GetConfigValue("TFServerPath"); } }

        public static string BAKPath { get { return GetConfigValue("BAKPath"); } }

        public static string ItemsParam { get { return GetConfigValue("ItemsParam"); } }

        public static string DepartmentsParam { get { return GetConfigValue("DepartmentsParam"); } }

        public static string ItemAndDeptFilesPath { get { return GetConfigValue("ItemAndDeptFilesPath"); } }

        public static DateTime TransactionDate
        {
            get
            {
                string transactionDayAlter = GetConfigValue("TransactionDayAlter");
                int days = 0;
                if (Int32.TryParse(transactionDayAlter, out days))
                {
                    return DateTime.Now.AddDays(days);
                }
                return DateTime.Now;
            }
        }

        public static int RetailerId { get { return Convert.ToInt32(GetConfigValue("RetailerId")); } }

        public static string RetailerName { get { return GetConfigValue("RetailerName"); } }

        public static int ZipCompressionLevel { get { return Math.Min(9, Math.Max(0, Convert.ToInt32(GetConfigValue("ZipCompressionLevel")))); } }

        public static DateTime TimeToRun { get { return Convert.ToDateTime(GetConfigValue("TimeToRun")); } }

        private static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static string GetConfigValue(string key)
        {
            if (config.AppSettings.Settings.AllKeys.Contains(key))
                return config.AppSettings.Settings[key].Value;
            else
                return null;
        }

        public static void SetConfigValue(string key, string value)
        {
            try
            {
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
            }
            catch { }
        }
    }
}
