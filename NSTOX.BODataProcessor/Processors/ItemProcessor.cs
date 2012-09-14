using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using NSTOX.BODataProcessor.Model;
using System.IO;
using NSTOX.BODataProcessor.Extensions;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;
using NSTOX.BODataProcessor.Helper;
using NSTOX.DAL.Helper;

namespace NSTOX.BODataProcessor.Processors
{
    public class ItemProcessor
    {
        private ItemDAL ItmDAL;
        private DataErrorDAL DataErrDAL;
        private JobAudit _job;

        public ItemProcessor()
        {
            DataErrDAL = new DataErrorDAL();
            ItmDAL = new ItemDAL(DataErrDAL);
        }

        public void ProcessItems(JobAudit job)
        {
            try
            {
                _job = job;
                _job.JobStatus = BOFileStatus.Processing;
                JobAuditDAL.UpdateJobAudit(_job);

                DateTime start = DateTime.Now;

                int totalItems = 0;
                List<byte[]> files = CompressHelper.DeCompress(_job.FilePath);

                List<Item> fromClient = ProcessItemsFile(_job.RetailerId, files.FirstOrDefault(), out totalItems);
                List<Item> fromDB = ItmDAL.GetAllItems(_job.RetailerId);

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

        private List<Item> ProcessItemsFile(int retailerId, byte[] itemsFileContent, out int fixedItemsProcessed)
        {
            string stringContent = ASCIIEncoding.ASCII.GetString(itemsFileContent);
            // parse the items file received from the win service
            ItemFixed[] fixedItems = FixedFileProcessor<ItemFixed>.ProcessString(stringContent);
            fixedItemsProcessed = fixedItems.Length;
            // convert the fixed items to what we use
            List<Item> items = fixedItems.Select(fi => fi.ToDALItem(retailerId, _job, DataErrDAL)).ToList();
            return items;
        }

        private void UpdateDB(List<Item> fromClient, List<Item> fromDB)
        {
            if (fromClient == null || fromDB == null || fromClient.Count == 0)
            {
                return;
            }

            // we parse all the items and populate the status field
            // we do this if for optimization
            if (fromClient.Count > fromDB.Count)
            {
                foreach (Item item in fromClient)
                {
                    if (item == null)
                        continue;

                    Item dbItem = fromDB.Where(iDB => iDB.SKU == item.SKU).FirstOrDefault();
                    if (dbItem == null)
                    {
                        item.Status = ElementStatus.New;
                        int errorCount = ItmDAL.InsertItem(item);

                        _job.IncrementError(errorCount);
                        _job.IncrementNew(errorCount);
                        continue;
                    }

                    if (item.DifferFrom(dbItem))
                    {
                        dbItem.Status = item.Status = ElementStatus.Update;
                        int errorCount = ItmDAL.UpdateItem(item, dbItem);

                        _job.IncrementError(errorCount);
                        _job.IncrementUpdated(errorCount);
                        continue;
                    }
                    else
                    {
                        dbItem.Status = item.Status = ElementStatus.Unchanged;
                        continue;
                    }
                }

                fromDB
                    .Where(i => i.Status == null)
                    .ToList()
                    .ForEach(i =>
                    {
                        if (i.DateRemoved == null)
                        {
                            i.Status = ElementStatus.Delete;
                            int errorCount = ItmDAL.DeleteItem(i.RetailerId, i.SKU);

                            _job.IncrementError(errorCount);
                            _job.IncrementDeleted(errorCount);
                        }
                    });
            }
            else
            {
                foreach (Item item in fromDB)
                {
                    Item clientItem = fromClient.Where(i => i != null && item != null && i.SKU == item.SKU).FirstOrDefault();

                    if (clientItem == null)
                    {
                        if (item.DateRemoved == null)
                        {
                            item.Status = ElementStatus.Delete;
                            int errorCount = ItmDAL.DeleteItem(item.RetailerId, item.SKU);

                            _job.IncrementError(errorCount);
                            _job.IncrementDeleted(errorCount);
                        }
                        continue;
                    }

                    if (item.DifferFrom(clientItem))
                    {
                        clientItem.Status = item.Status = ElementStatus.Update;
                        int errorCount = ItmDAL.UpdateItem(clientItem, item);

                        _job.IncrementError(errorCount);
                        _job.IncrementUpdated(errorCount);
                        continue;
                    }
                    else
                    {
                        clientItem.Status = item.Status = ElementStatus.Unchanged;
                        continue;
                    }
                }

                fromClient
                    .Where(i => i != null && i.Status == null)
                    .ToList()
                    .ForEach(i =>
                    {
                        i.Status = ElementStatus.New;
                        int errorCount = ItmDAL.InsertItem(i);

                        _job.IncrementError(errorCount);
                        _job.IncrementNew(errorCount);
                    });
            }
        }

    }
}
