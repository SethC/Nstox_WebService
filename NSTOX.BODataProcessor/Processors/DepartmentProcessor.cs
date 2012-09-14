using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.BODataProcessor.Model;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;
using NSTOX.BODataProcessor.Extensions;
using NSTOX.BODataProcessor.Helper;
using NSTOX.DAL.Helper;

namespace NSTOX.BODataProcessor.Processors
{
    public class DepartmentProcessor
    {
        private DepartmentDAL DeptDAL;
        private DataErrorDAL DataErrDAL;
        private JobAudit _job;

        public DepartmentProcessor()
        {
            DataErrDAL = new DataErrorDAL();
            DeptDAL = new DepartmentDAL(DataErrDAL);
        }

        public void ProcessDepartments(JobAudit job)
        {
            try
            {
                _job = job;
                _job.JobStatus = BOFileStatus.Processing;
                JobAuditDAL.UpdateJobAudit(_job);

                DateTime start = DateTime.Now;

                List<byte[]> files = CompressHelper.DeCompress(_job.FilePath);

                int totalItems = 0;
                List<Department> fromClient = ProcessDepartmentsFile(job.RetailerId, files.FirstOrDefault(), out totalItems);
                List<Department> fromDB = DeptDAL.GetAllDepartments(job.RetailerId);

                UpdateDB(fromClient, fromDB);

                _job.ItemsProcessed = totalItems;
                _job.ProcessTime = (int)DateTime.Now.Subtract(start).TotalSeconds;
                _job.JobStatus = BOFileStatus.Done;
                JobAuditDAL.UpdateJobAudit(_job);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private List<Department> ProcessDepartmentsFile(int retailerId, byte[] itemsFileContent, out int totalItems)
        {
            string stringContent = ASCIIEncoding.ASCII.GetString(itemsFileContent);
            // parse the items file received from the win service
            DepartmentFixed[] fixedItems = FixedFileProcessor<DepartmentFixed>.ProcessString(stringContent);
            totalItems = fixedItems.Length;

            // convert the fixed items to what we use
            List<Department> items = fixedItems.Select(fi => fi.ToDALDepartment(retailerId, DataErrDAL)).ToList();
            return items;
        }

        private void UpdateDB(List<Department> fromClient, List<Department> fromDB)
        {
            if (fromClient == null || fromDB == null || fromClient.Count == 0)
            {
                return;
            }

            // we parse all the items and populate the status field
            // we do this if for optimization
            if (fromClient.Count > fromDB.Count)
            {
                foreach (Department dept in fromClient)
                {
                    if (dept == null)
                        continue;

                    Department dbDept = fromDB.Where(dDB => dDB.Id == dept.Id).FirstOrDefault();
                    if (dbDept == null)
                    {
                        dept.Status = ElementStatus.New;
                        int errorCount = DeptDAL.InsertDepartment(dept);

                        _job.IncrementError(errorCount);
                        _job.IncrementNew(errorCount);
                        continue;
                    }

                    if (dept.DifferFrom(dbDept))
                    {
                        dbDept.Status = dept.Status = ElementStatus.Update;
                        int errorCount = DeptDAL.UpdateDepartment(dept, dbDept);

                        _job.IncrementError(errorCount);
                        _job.IncrementUpdated(errorCount);
                        continue;
                    }
                    else
                    {
                        dbDept.Status = dept.Status = ElementStatus.Unchanged;
                        continue;
                    }
                }

                fromDB
                    .Where(d => d.Status == null)
                    .ToList()
                    .ForEach(d =>
                    {
                        if (d.DateRemoved == null)
                        {
                            d.Status = ElementStatus.Delete;
                            int errorCount = DeptDAL.DeleteDepartment(d.RetailerId, d.Id);

                            _job.IncrementError(errorCount);
                            _job.IncrementUpdated(errorCount);
                        }
                    });
            }
            else
            {
                foreach (Department dept in fromDB)
                {
                    Department clientDept = fromClient.Where(d => d.Id == dept.Id).FirstOrDefault();

                    if (clientDept == null)
                    {
                        if (dept.DateRemoved == null)
                        {
                            dept.Status = ElementStatus.Delete;
                            int errorCount = DeptDAL.DeleteDepartment(dept.RetailerId, dept.Id);

                            _job.IncrementError(errorCount);
                            _job.IncrementDeleted(errorCount);
                        }
                        continue;
                    }

                    if (dept.DifferFrom(clientDept))
                    {
                        clientDept.Status = dept.Status = ElementStatus.Update;
                        int errorCount = DeptDAL.UpdateDepartment(clientDept, dept);

                        _job.IncrementError(errorCount);
                        _job.IncrementUpdated(errorCount);
                        continue;
                    }
                    else
                    {
                        clientDept.Status = dept.Status = ElementStatus.Unchanged;
                        continue;
                    }
                }

                fromClient
                    .Where(d => d.Status == null)
                    .ToList()
                    .ForEach(d =>
                    {
                        d.Status = ElementStatus.New;
                        int errorCount = DeptDAL.InsertDepartment(d);

                        _job.IncrementError(errorCount);
                        _job.IncrementNew(errorCount);
                    });
            }
        }
    }
}
