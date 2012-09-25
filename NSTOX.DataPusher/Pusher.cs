using System;
using System.Collections.Generic;
using System.Text;
using NSTOX.DataPusher.Helper;
using NSTOX.DataPusher.BOService;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using NSTOX.DataPusher.Model;

namespace NSTOX.DataPusher
{
    public class Pusher
    {
        private static BOServiceClient _proxy;
        private static BOServiceClient Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new BOService.BOServiceClient();
                }
                return _proxy;
            }
        }

        public static void RunJob()
        {
            try
            {
                Logger.LogInfo("******************* Application START *******************");

                Logger.LogInfo("Extracting items from Back Office");
                ExtractUtilityHelper.RunExtractUtility(ConfigurationHelper.ItemsParam);

                Logger.LogInfo("Extracting departments from Back Office");
                ExtractUtilityHelper.RunExtractUtility(ConfigurationHelper.DepartmentsParam);

                Logger.LogInfo("Extracting transactions from Back Office");
                string transactionPath = ExtractUtilityHelper.ExtractTransactionXML();

                var departments = GetBOFile(BOFileType.Departments);

                if (departments != null && departments.FileContent != null)
                {
                    Logger.LogInfo("Pushing departments file to the web service!");
                    var result = Proxy.PushBOFile(departments);
                }

                var items = GetBOFile(BOFileType.Items);

                if (items != null && items.FileContent != null)
                {
                    Logger.LogInfo("Pushing items file to the web service!");
                    Proxy.PushBOFile(items);
                }

                var transactions = GetBOFile(BOFileType.Transactions, ConfigurationHelper.TransactionDate, transactionPath);

                if (transactions != null)
                {
                    Logger.LogInfo("Pushing transaction files to the web service!");
                    Proxy.PushBOFile(transactions);
                }

                Logger.LogInfo("Triggering BO files processor!");
                Proxy.ProcessBOFilesForRetailer(ConfigurationHelper.RetailerId);

                Proxy.Close();
                _proxy = null;

                Logger.LogInfo("+++++++++++++++++++ Done! +++++++++++++++++++");
                Logger.Flush();
            }
            catch (Exception ex)
            {
                Logger.Flush();
                Logger.LogException(ex);
            }
        }

        public static void PushHistoricalTransactions(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                Logger.LogInfo("End date should be greater than or equal to start date!");
                return;
            }

            if (ConfigurationHelper.RetailerId <= 0)
            {
                Logger.LogInfo("Please set the retailer Id before processing transactions!");
                return;
            }

            if (string.IsNullOrEmpty(ConfigurationHelper.RetailerName))
            {
                Logger.LogInfo("Please set the retailer name before processing transactions!");
                return;
            }

            DeleteAllTransactionXMLs();

            int count = 0;

            for (DateTime date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
            {
                string transPath = Path.Combine(Constants.TransactionsPath, string.Format("trans_{0}.xml", date.ToString("yyyyMMdd")));

                bool transXMLCreated = ExtractUtilityHelper.QDXTransactionToXML(date, transPath);

                if (!transXMLCreated)
                {
                    continue;
                }

                count++;

                var transactions = GetBOFile(BOFileType.Transactions, date, transPath);
                if (transactions != null)
                {
                    Logger.LogInfo(string.Format("PUSHING: Transaction for {0}", date.ToString(Constants.DateFormat)));
                    Proxy.PushBOFile(transactions);

                    ExtractUtilityHelper.SafelyDeleteFile(transPath);
                }
            }

            if (count == 0)
            {
                Logger.LogInfo("No transactions were processed.");
                return;
            }

            Logger.LogInfo(string.Format("Pushed {0} transactions!", count));
            Logger.LogInfo("Triggering BO files processor!");
            Proxy.ProcessBOFilesForRetailer(ConfigurationHelper.RetailerId);
            Proxy.Close();
            _proxy = null;
        }

        public static void PushBOFileFromZip(BOFileType type, Stream stream, DateTime? fileDate, int? retailerID = null, string retailerName = null)
        {
            using (var ms = new MemoryStream())
            {
                StreamUtils.Copy(stream, ms, new byte[4096]);
                var result = new LocalFile()
                {
                    BOFile = new BOFile()
                    {
                        RetailerId = retailerID ?? ConfigurationHelper.RetailerId,
                        RetailerName = retailerName ?? ConfigurationHelper.RetailerName,
                        FileType = type,
                        FileDate = fileDate ?? DateTime.Now,
                    },
                    FileContent = ms.ToArray()
                };
                Proxy.PushBOFile(result);
            }
        }

        private static LocalFile GetBOFile(BOFileType type, DateTime? fileDate = null, string filePath = null)
        {
            List<string> filesPath = BOFilesHelper.GetFilePathByType(type, filePath);

            if (filesPath != null && filesPath.Count > 0)
            {
                LocalFile lf = new LocalFile();

                BOFile result = new BOFile();

                result.RetailerId = ConfigurationHelper.RetailerId;
                result.RetailerName = ConfigurationHelper.RetailerName;
                result.FileType = type;
                result.FileDate = fileDate ?? DateTime.Now;
                lf.BOFile = result;

                lf.FileContent = BOFilesHelper.Compress(filesPath);

                return lf;
            }
            else
            {
                Logger.LogInfo(string.Format("Couldn't find today's {0} file(s).", type), System.Diagnostics.EventLogEntryType.Warning);
                return null;
            }
        }

        private static void DeleteAllTransactionXMLs()
        {
            // delete all transaction files
            DirectoryInfo dirInfo = new DirectoryInfo(Constants.TransactionsPath);
            FileInfo[] files = dirInfo.GetFiles("*.XML");
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    ExtractUtilityHelper.SafelyDeleteFile(file.FullName);
                }
            }
        }
    }
}
