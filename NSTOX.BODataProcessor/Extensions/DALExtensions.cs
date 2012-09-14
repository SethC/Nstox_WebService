using System;
using NSTOX.DAL.Model;
using NSTOX.BODataProcessor.Model;
using NSTOX.DAL.Helper;
using NSTOX.DAL.DAL;

namespace NSTOX.BODataProcessor.Extensions
{
    public static class DALExtensions
    {
        public static Item ToDALItem(this ItemFixed item, int retailerId, JobAudit job, DataErrorDAL dataErrorDAL)
        {
            if (item == null)
                return null;
            try
            {
                string sDepIp = item.DepartmentID.RemoveTrailComma();
                int depId = 0;
                if (!string.IsNullOrEmpty(sDepIp))
                    depId = Convert.ToInt32(sDepIp);

                Item result = new Item
                {
                    RetailerId = retailerId,
                    SKU = item.ItemID.ToLong(),
                    Description = item.ReceiptDescription.RemoveTrailCommaAndSanitize(),
                    DepartmentId = item.DepartmentID.ToNullableInt(),
                    PriceGroup = item.PriceGroupId.ToInt(),
                    ProductGroup = item.MixMatchIdFiveDigit.ToInt(),
                    Category7 = item.FoodStampFlag.ToBool(),
                    Category8 = item.NoneMerchandiseID.ToBool(),
                    Category9 = item.StoreCouponFlag.ToBool(),
                    Category10 = item.WICFlag.ToBool(),
                    Size = item.Size.RemoveTrailComma(),
                    Unit = item.Unit.RemoveTrailComma(),
                    CurrentUnitPrice = item.CenterPrice.ToDouble(),
                    QtyPrice = item.RetailPrice.ToDouble(),
                    QtyBreak = item.UnitQuantity.ToInt()
                };

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                DataError dataError = new DataError { RetailerId = retailerId, Source = InputFileType.Items, ElementId = item.ItemID.ToLong(), Note = "Items file parse error! Row is wrongly formatted!" };
                dataErrorDAL.InsertDataError(dataError);
                job.IncrementError(1);
                return null;
            }
        }

        public static bool DifferFrom(this Item item, Item itemToCompare)
        {
            if (item == null || itemToCompare == null)
                return false;

            if (item.SKU != itemToCompare.SKU)
                return true;
            if (item.Description != itemToCompare.Description)
                return true;
            if (item.DepartmentId != itemToCompare.DepartmentId)
                return true;
            if (item.PriceGroup != itemToCompare.PriceGroup)
                return true;
            if (item.ProductGroup != itemToCompare.ProductGroup)
                return true;
            if (item.Category7 != itemToCompare.Category7)
                return true;
            if (item.Category8 != itemToCompare.Category8)
                return true;
            if (item.Category9 != itemToCompare.Category9)
                return true;
            if (item.Category10 != itemToCompare.Category10)
                return true;
            if (item.Size != itemToCompare.Size)
                return true;
            if (item.Unit != itemToCompare.Unit)
                return true;
            if (item.CurrentUnitPrice != itemToCompare.CurrentUnitPrice)
                return true;
            if (item.QtyPrice != itemToCompare.QtyPrice)
                return true;
            if (item.QtyBreak != itemToCompare.QtyBreak)
                return true;
            return false;
        }

        public static Department ToDALDepartment(this DepartmentFixed department, int retailerId, DataErrorDAL dataErrorDAL)
        {
            if (department == null)
                return null;

            try
            {
                Department result = new Department
                {
                    RetailerId = retailerId,
                    Id = department.DepartmentId.ToInt(),
                    Description = department.Description.RemoveTrailCommaAndSanitize()
                };

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                DataError dataError = new DataError { RetailerId = retailerId, Source = InputFileType.Items, ElementId = department.DepartmentId.ToInt() };
                dataErrorDAL.InsertDataError(dataError);
                return null;
            }
        }

        public static bool DifferFrom(this Department dept, Department itemToCompare)
        {
            if (dept == null || itemToCompare == null)
                return false;

            if (dept.Description != itemToCompare.Description)
                return true;

            return false;
        }

        public static Transaction ToDALTransaction(this SalesTransactionDetail transDetail, int retailerId, SalesTransaction saleTransaction)
        {
            if (transDetail == null || saleTransaction == null || transDetail.Details == null || transDetail.Details.Item == null)
                return null;

            Transaction trans = new Transaction
            {
                RetailerId = retailerId,
                Number = saleTransaction.TransactionNumber,
                Date = saleTransaction.StartDateTime,
                TerminalNo = saleTransaction.TillID,
                EmployeeNo = saleTransaction.CashierID,
                Line = transDetail.SequenceNumber,
                SKU = transDetail.Details.Item.ItemID == 0 ? null : (long?)transDetail.Details.Item.ItemID,
                DepartmentId = transDetail.Details.Item.MerchandiseHierarchy == 0 ? null : (int?)transDetail.Details.Item.MerchandiseHierarchy,
                QuantitySold = transDetail.Details.Item.Quantity,
                UnitSell = transDetail.Details.Item.UnitSalesPrice,
                UnitCost = transDetail.Details.Item.UnitCostPrice,
                ExtendedPrice = transDetail.Details.Item.ExtendedAmount,
                ExtendedCost = transDetail.Details.Item.ExtendedCost,
                SellMargin = transDetail.Details.Item.SellMargin,
                TaxAmount = transDetail.Details.Item.TaxAmount
            };
            return trans;
        }
    }
}
