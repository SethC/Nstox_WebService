using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NSTOX.WebService.Helper;
using NSTOX.BODataProcessor.Processors;
using System.Threading.Tasks;
using System.IO;
using NSTOX.WebService.Model;
using System.Text;
using NSTOX.BODataProcessor.DALWrapper;
using NSTOX.BODataProcessor.Helper;

namespace NSTOX.WebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    public class BOService : IBOService
    {
        private int _retailerId = 0;

        public bool PushBOFile(BOFile file)
        {
            if (file == null)
            {
                Logger.LogInfo("PushBOFile(null)");
                return false;
            }

            Logger.LogInfo(string.Format("PushBOFile (RetailerId = {0}, RetailerName = {1}, FileType = {2}, FileDate = {3}, FileContentLength = {4}",
                file.RetailerId,
                file.RetailerName,
                file.FileType,
                file.FileDate,
                file.FileContent == null ? 0 : file.FileContent.Length));

            string filePath = BOFilesHelper.SaveBOFileToDisk(file);

            RetailerProcessor retProcessor = new RetailerProcessor();
            retProcessor.EnsureRetailerExists(file.RetailerId, file.RetailerName);

            _retailerId = file.RetailerId;

            JobAuditWrapper.AddBOFile(file.RetailerId, file.FileType, filePath);

            file = null;
            retProcessor = null;
            return true;
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

        public bool ProcessBOFiles()
        {
            return ProcessBOFilesForRetailer(_retailerId);
        }

    }
}
