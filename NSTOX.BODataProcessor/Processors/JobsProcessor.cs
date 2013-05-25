using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;
using System.IO;
using NSTOX.BODataProcessor.Helper;
using NSTOX.DAL.Helper;
using System.Diagnostics;

namespace NSTOX.BODataProcessor.Processors
{
    public class JobsProcessor
    {
        public static void ProcessJobs(int retailerId)
        {
            Trace.WriteLine("Checking for available jobs - " + retailerId);
            List<JobAudit> jobs = JobAuditDAL.GetNewJobAudits(retailerId);

            if (jobs == null || jobs.Count == 0)
            {
                Trace.WriteLine("No Jobs Waiting");
                return;
            }

            Trace.WriteLine(jobs.Count + " jobs found");

            DepartmentProcessor deptProcessor = new DepartmentProcessor();
            ItemProcessor itmProcessor = new ItemProcessor();
            TransactionProcessor transProcessor = new TransactionProcessor();

            while (jobs != null && jobs.Count > 0)
            {
                JobAudit job = jobs.First();

                if (!BOFilesHelper.Exists(job.FilePath))
                {
                    Trace.WriteLine("Job Not Found - " + job.Id);
                    job.JobStatus = BOFileStatus.FileNotFound;
                    JobAuditDAL.UpdateJobAudit(job);
                    jobs.Remove(job);
                    continue;
                }

                Trace.WriteLine(job.FileType + " Job Found" + "(" + job.Id + ")");

                switch (job.FileType)
                {
                    case InputFileType.Items:
                        itmProcessor.ProcessItems(job);
                        break;
                    case InputFileType.Departments:
                        deptProcessor.ProcessDepartments(job);
                        break;
                    case InputFileType.Transactions:
                        transProcessor.ProcessTransactions(job);
                        break;
                    default:
                        break;
                }

                Trace.WriteLine("Job Processed");

                jobs = JobAuditDAL.GetNewJobAudits(retailerId);
            }
            deptProcessor = null;
            itmProcessor = null;
            transProcessor = null;
        }
    }
}
