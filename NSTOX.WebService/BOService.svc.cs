using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NSTOX.WebService.Helper;
using NSTOX.BODataProcessor.Processors;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using NSTOX.BODataProcessor.DALWrapper;
using NSTOX.BODataProcessor.Helper;
using NSTOX.BODataProcessor.Model;

namespace NSTOX.WebService
{
    public class BOService : IBOService
    {
        public AzureUploadFile PushBOFile(BOFile file)
        {
            if (file == null)
            {
                Logger.LogInfo("PushBOFile(null)");
                return null;
            }

            Logger.LogInfo(string.Format("PushBOFile (RetailerId = {0}, RetailerName = {1}, FileType = {2}, FileDate = {3}, FileContentLength = {4}",
                file.RetailerId,
                file.RetailerName,
                file.FileType,
                file.FileDate,
                file.FileLength));

            string filePath = BOFilesHelper.GetFileName(file);
            var signature = BOFilesHelper.GetSignature(filePath);

            RetailerProcessor retProcessor = new RetailerProcessor();
            retProcessor.EnsureRetailerExists(file.RetailerId, file.RetailerName);

            JobAuditWrapper.AddBOFile(file.RetailerId, file.FileType, filePath);

            file = null;
            retProcessor = null;

            return signature;
        }

        public bool Uploaded(int retailerId, string filePath)
        {
            return JobAuditWrapper.UploadedBOFile(retailerId, filePath);
        }

        public bool ProcessBOFilesForRetailer(int retailerId)
        {
            if (retailerId <= 0)
                return false;

            Task task = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning)
                .StartNew(() =>
                    {
                        JobsProcessor.ProcessJobs(retailerId);
                    });

            return true;
        }
    }
}
