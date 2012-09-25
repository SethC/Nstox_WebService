using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.Helper;

namespace NSTOX.DAL.DAL
{
    public class JobAuditDAL : BaseDAL
    {
        public static void InsertJobAudit(JobAudit job)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_JobAudit", job, true);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static List<JobAudit> GetNewJobAudits(int retailerId)
        {
            try
            {
                return GetFromDBAndMapToObjectsList<JobAudit>("usp_Get_NewJobAudits", new { RetailerId = retailerId });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public static JobAudit GetJobAuditByFilePath(string filePath)
        {
            try
            {
                return GetFromDBAndMapToObjectsList<JobAudit>("usp_Get_JobAuditByFilePath", new { FilePath = filePath }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public static void UpdateJobAudit(JobAudit job)
        {
            try
            {
                ExecuteNonQuery("usp_Update_JobAudit", job, false, true);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
