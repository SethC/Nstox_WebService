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
                JobAudit job = new JobAudit { RetailerId = retailerId, FileType = (InputFileType)fileType, FilePath = filePath, JobStatus = BOFileStatus.New };
                JobAuditDAL.InsertJobAudit(job);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
