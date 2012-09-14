using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;

namespace NSTOX.DAL.DAL
{
    public class RetailerDAL : BaseDAL
    {
        public Retailer GetRetailerById(int retailerId)
        {
            List<Retailer> retailers = GetFromDBAndMapToObjectsList<Retailer>("usp_Get_RetailerById", new { Id = retailerId });

            return retailers.FirstOrDefault();
        }

        public void UpdateRetailer(Retailer retailer)
        {
            ExecuteNonQuery("usp_Update_Retailer", retailer);
        }

        public void AddNewRetailer(Retailer retailer)
        {
            ExecuteNonQuery("usp_Insert_Retailer", retailer);
            string query = string.Format(Resources.SQLScripts.NewRetailer, retailer.Id);
            ExecuteQuery(query);
        }
    }
}
