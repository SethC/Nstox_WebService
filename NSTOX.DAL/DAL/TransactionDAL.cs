using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.Helper;

namespace NSTOX.DAL.DAL
{
    public class TransactionDAL : BaseDAL
    {
        private DataErrorDAL DataErrDAL;

        public TransactionDAL(DataErrorDAL dataErrorDAL)
        {
            DataErrDAL = dataErrorDAL;
        }

        public byte InsertTransaction(Transaction transaction)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_Transaction", transaction);
                return 0;
            }
            catch (Exception ex)
            {
                string note = ex.Message;
                if (ex.Message.Contains("dbo.Item_"))
                {
                    note = string.Format("Error inserting transaction. Transaction number: {0}. Item with SKU: {1} does not exist!", transaction.Number, transaction.SKU);
                }
                else if (ex.Message.Contains("dbo.Department_"))
                {
                    note = string.Format("Error inserting transaction. Transaction number: {0}. DepartmentId: {1} does not exist!", transaction.Number, transaction.DepartmentId);
                }
                DataError dataError = new DataError { RetailerId = transaction.RetailerId, Source = InputFileType.Transactions, ElementId = transaction.SKU ?? -1, Note = note };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public bool TransactionExists(Transaction transaction)
        {
            if (transaction == null)
                return false;

            try
            {
                List<Int64> transactionIds = GetFromDBAndMapToList<Int64>("usp_Get_Transaction", "Id",
                                                    new
                                                    {
                                                        @RetailerId = transaction.RetailerId,
                                                        @Number = transaction.Number,
                                                        @Date = transaction.Date,
                                                        @TerminalNo = transaction.TerminalNo,
                                                        @EmployeeNo = transaction.EmployeeNo,
                                                        @Line = transaction.Line,
                                                        @SKU = transaction.SKU,
                                                        @DepartmentId = transaction.DepartmentId
                                                    });
                return transactionIds != null && transactionIds.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}
