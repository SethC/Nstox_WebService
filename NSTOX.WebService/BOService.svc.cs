using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NSTOX.BODataProcessor.Processors;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using NSTOX.BODataProcessor.DALWrapper;
using NSTOX.BODataProcessor.Helper;
using NSTOX.BODataProcessor.Model;
using System.Diagnostics;

namespace NSTOX.WebService
{
    public class BOService : IBOService
    {
        public AzureUploadFile PushBOFile(BOFile file)
        {
            if (file == null)
            {
                Trace.WriteLine("PushBOFile(null)");
                return null;
            }

            Trace.WriteLine(string.Format("PushBOFile (RetailerId = {0}, RetailerName = {1}, FileType = {2}, FileDate = {3}, FileContentLength = {4}",
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

            Trace.WriteLine("Returning " + signature.ToString());
            return signature;
        }

        public bool Uploaded(int retailerId, string filePath)
        {
            Trace.WriteLine("Uploaded " + retailerId + " - " + filePath);
            return JobAuditWrapper.UploadedBOFile(retailerId, filePath);
        }

        public bool ProcessBOFilesForRetailer(int retailerId)
        {
            if (retailerId <= 0)
                return false;

            Trace.WriteLine("Processing for " + retailerId);

            Task task = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning)
                .StartNew(() =>
                    {
                        JobsProcessor.ProcessJobs(retailerId);
                    });

            return true;
        }
    }
}
