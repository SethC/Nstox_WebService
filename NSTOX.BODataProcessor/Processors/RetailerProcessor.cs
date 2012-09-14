using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.DAL;

namespace NSTOX.BODataProcessor.Processors
{
    public class RetailerProcessor
    {
        private RetailerDAL retailerDAL;

        public RetailerProcessor()
        {
            retailerDAL = new RetailerDAL();
        }

        public void EnsureRetailerExists(int retailerId, string retailerName)
        {
            if (retailerId <= 0)
                return;

            Retailer retailer = retailerDAL.GetRetailerById(retailerId);

            if (retailer == null)
            {
                DateTime startTime = DateTime.Now;
                retailerDAL.AddNewRetailer(new Retailer { Id = retailerId, Name = retailerName });
            }
            else
            {
                if (retailer.Name != retailerName)
                {
                    retailer.Name = retailerName ?? string.Empty;
                    retailerDAL.UpdateRetailer(retailer);
                }
            }
        }
    }
}
