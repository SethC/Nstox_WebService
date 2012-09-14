using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DAL.Model;
using NSTOX.DAL.Extensions;

namespace NSTOX.DAL.DAL
{
    public class DepartmentDAL : BaseDAL
    {
        private DataErrorDAL DataErrDAL;

        public DepartmentDAL(DataErrorDAL dataErrorDAL)
        {
            DataErrDAL = dataErrorDAL;
        }

        public byte InsertDepartment(Department dept)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_Department", dept);
                return 0;
            }
            catch (Exception ex)
            {
                DataError dataError = new DataError { RetailerId = dept.RetailerId, Source = InputFileType.Departments, ElementId = dept.Id, Note = ex.Message };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public byte UpdateDepartment(Department dept, Department oldDept)
        {
            try
            {
                ExecuteNonQuery("usp_Insert_DepartmentAudit", new { RetailerId = dept.RetailerId, DepartmentId = dept.Id, OldValue = oldDept.ToXML(), NewValue = dept.ToXML() });
                ExecuteNonQuery("usp_Update_Department", dept);
                return 0;
            }
            catch (Exception ex)
            {
                DataError dataError = new DataError { RetailerId = dept.RetailerId, Source = InputFileType.Departments, ElementId = dept.Id, Note = ex.Message };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public byte DeleteDepartment(int retailerId, int id)
        {
            try
            {
                ExecuteNonQuery("usp_Delete_Department", new { RetailerId = retailerId, Id = id });
                return 0;
            }
            catch (Exception ex)
            {
                DataError dataError = new DataError { RetailerId = retailerId, Source = InputFileType.Departments, ElementId = id, Note = ex.Message };
                DataErrDAL.InsertDataError(dataError);
                return 1;
            }
        }

        public Department GetById(int retailerId, int id)
        {
            return GetFromDBAndMapToObjectsList<Department>("usp_Get_DepartmentById", new { RetailerId = retailerId, Id = id }).FirstOrDefault();
        }

        public List<Department> GetAllDepartments(int retailerId)
        {
            return GetFromDBAndMapToObjectsList<Department>("usp_Get_Departments", new { RetailerId = retailerId });
        }
    }
}
