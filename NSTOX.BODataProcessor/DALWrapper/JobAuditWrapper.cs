using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.BODataProcessor.Model;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;
using NSTOX.DAL.Helper;

namespace NSTOX.BODataProcessor.DALWrapper
{
    public class JobAuditWrapper
    {
        public static void AddBOFile(int retailerId, BOFileType fileType, string filePath)
        {
            try
            {
                JobAudit job = new JobAudit { RetailerId = retailerId, FileType = (InputFileType)fileType, FilePath = filePath, JobStatus = BOFileStatus.NotUploaded };
                JobAuditDAL.InsertJobAudit(job);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static bool UploadedBOFile(int retailerId, string filePath)
        {
            var job = JobAuditDAL.GetJobAuditByFilePath(retailerId, filePath);
            if (job != null)
            {
                job.JobStatus = BOFileStatus.New;
                JobAuditDAL.UpdateJobAudit(job);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
