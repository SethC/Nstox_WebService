using System;
using System.Collections.Generic;
using NSTOX.BODataProcessor.Model;
using NSTOX.BODataProcessor.Extensions;
using NSTOX.DAL.Extensions;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;
using NSTOX.BODataProcessor.Helper;
using NSTOX.DAL.Helper;

namespace NSTOX.BODataProcessor.Processors
{
    public class TransactionProcessor
    {
        private DataErrorDAL DataErrDAL;
        private TransactionDAL TransDAL;
        private JobAudit _job;

        public TransactionProcessor()
        {
            DataErrDAL = new DataErrorDAL();
            TransDAL = new TransactionDAL(DataErrDAL);
        }

        public bool ProcessTransactions(JobAudit job)
        {
            try
            {
                int totalItems = 0;
                _job = job;
                _job.JobStatus = BOFileStatus.Processing;
                JobAuditDAL.UpdateJobAudit(_job);

                DateTime startTime = DateTime.Now;

                List<byte[]> files = CompressHelper.DeCompress(_job.FilePath);

                foreach (byte[] file in files)
                {
                    Transactions receivedTransactions = file.Deserialize<Transactions>();

                    if (receivedTransactions == null || receivedTransactions.SaleTransactions == null || receivedTransactions.SaleTransactions.Count == 0)
                        continue;

                    foreach (SalesTransaction trans in receivedTransactions.SaleTransactions)
                    {
                        if (trans.TransactionDetails == null)
                            continue;

                        foreach (SalesTransactionDetail item in trans.TransactionDetails)
                        {
                            totalItems++;

                            Transaction transaction = item.ToDALTransaction(job.RetailerId, trans);

                            bool transactionExists = TransDAL.TransactionExists(transaction);

                            if (transaction == null || transactionExists)
                                continue;

                            int errorCount = TransDAL.InsertTransaction(transaction);

                            _job.ErrorCount += errorCount;
                            _job.IncrementNew(errorCount);
                        }
                    }
                }

                _job.ItemsProcessed = totalItems;
                _job.ProcessTime = (int)DateTime.Now.Subtract(startTime).TotalSeconds;
                _job.JobStatus = BOFileStatus.Done;
                JobAuditDAL.UpdateJobAudit(_job);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}
