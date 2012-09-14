using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.Extensions;

namespace NSTOX.DAL.DAL
{
    public class ItemDAL : BaseDAL
    {
        private DataErrorDAL DataErrDAL;

        public ItemDAL(DataErrorDAL dataErrorDAL)
        {
            DataErrDAL = dataErrorDAL;
        }

        public byte InsertItem(Item item)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_Item", item);
                return 0;
            }
            catch (Exception ex)
            {
                string note = ex.Message;
                if (ex.Message.Contains("dbo.Department_"))
                {
                    note = string.Format("Error inserting Item. Department with ID: {0} does not exist.", item.DepartmentId);
                }
                DataError dataError = new DataError { RetailerId = item.RetailerId, Source = InputFileType.Items, ElementId = item.SKU, Note = note };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public byte UpdateItem(Item item, Item oldItem)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_ItemAudit", new { RetailerId = item.RetailerId, SKU = item.SKU, OldValue = oldItem.ToXML(), NewValue = item.ToXML() });
                ExecuteNonQuery("usp_Update_Item", item);
                return 0;
            }
            catch (Exception ex)
            {
                DataError dataError = new DataError { RetailerId = item.RetailerId, Source = InputFileType.Items, ElementId = item.SKU, Note = ex.Message };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public byte DeleteItem(int retailerId, Int64 sku)
        {
            try
            {
                ExecuteNonQuery("usp_Delete_Item", new { RetailerId = retailerId, SKU = sku });
                return 0;
            }
            catch (Exception ex)
            {
                DataError dataError = new DataError { RetailerId = retailerId, Source = InputFileType.Items, ElementId = sku, Note = ex.Message };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public Item GetItemBySKU(int retailerId, int sku)
        {
            return GetFromDBAndMapToObjectsList<Item>("usp_Get_ItemBySKU", new { RetailerId = retailerId, SKU = sku }).FirstOrDefault();
        }

        public List<Item> GetAllItems(int retailerId)
        {
            return GetFromDBAndMapToObjectsList<Item>("usp_Get_Items", new { RetailerId = retailerId });
        }
    }
}
