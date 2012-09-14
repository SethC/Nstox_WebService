using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;

namespace NSTOX.DAL.DAL
{
    public delegate void DataErrorDelegate();
    public class DataErrorDAL : BaseDAL
    {
        public event DataErrorDelegate OnDataError;

        public void InsertDataError(DataError error)
        {
            ExecuteNonQuery("usp_Insert_DataError", error);

            if (OnDataError != null)
            {
                OnDataError();
            }
        }
    }
}
