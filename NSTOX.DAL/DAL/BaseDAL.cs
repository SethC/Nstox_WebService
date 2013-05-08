using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using NSTOX.DAL.Helper;
using System.Configuration;
using NSTOX.DAL.Model;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;

namespace NSTOX.DAL.DAL
{
    public class BaseDAL
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["NSTOXConnectionString"].ConnectionString; } }

        protected static IDbConnection CreateConnection()
        {
            IDbConnection connection = null;
            // Get an instance of the RetryManager class.
            //var retryManager = EnterpriseLibraryContainer.Current.GetInstance<RetryManager>();

            // Create a retry policy that uses a default retry strategy from the 
            // configuration.
            //var retryPolicy = retryManager.GetDefaultSqlConnectionRetryPolicy();
            //connection = new ReliableSqlConnection(ConnectionString, retryPolicy, retryPolicy);
            connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// This method will call a stored procedure and try to Map the results to a list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="namedParams"></param>
        /// <returns></returns>
        protected static List<T> GetFromDBAndMapToObjectsList<T>(string storedProcedure, object namedParams) where T : new()
        {
            List<T> result = new List<T>();

            DataTable table = new DataTable();
            using (var connection = CreateConnection())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    using (IDataReader reader = ExecuteReader(storedProcedure, namedParams, connection, command))
                    {
                        PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        // build a data table that will be used by the debug window
                        int fieldCount = reader.FieldCount;
                        for (int i = 0; i < fieldCount; i++)
                        {
                            table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }
                        table.BeginLoadData();
                        object[] values = new object[fieldCount];


                        while (reader.Read())
                        {
                            // add the row to the table
                            reader.GetValues(values);
                            table.LoadDataRow(values, true);

                            T item = new T();

                            foreach (PropertyInfo p in props)
                            {
                                if (!ColumnExists(reader, p.Name))
                                    continue;

                                if (p.PropertyType == typeof(DateTime))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToDateTime(), null);
                                }
                                if (p.PropertyType == typeof(DateTime?))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToNullableDateTime(), null);
                                }
                                else if (p.PropertyType == typeof(Int32))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToInt(), null);
                                }
                                else if (p.PropertyType == typeof(int?))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToNullableInt(), null);
                                }
                                else if (p.PropertyType == typeof(long))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToLong(), null);
                                }
                                else if (p.PropertyType == typeof(string))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToStringDefaultEmpty(), null);
                                }
                                else if (p.PropertyType == typeof(double))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToDouble(), null);
                                }
                                else if (p.PropertyType == typeof(bool))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToBool(), null);
                                }
                                else if (p.PropertyType == typeof(InputFileType))
                                {
                                    p.SetValue(item, reader[p.Name].SafelyToEnum<InputFileType>(), null);
                                }
                            }

                            result.Add(item);
                        }

                        table.EndLoadData();
                    }
                }
            }
            return result;
        }

        protected static List<T> GetFromDBAndMapToList<T>(string storedProcedure, string columnName, object namedParams)
        {
            List<T> result = new List<T>();

            using (var connection = CreateConnection())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    using (IDataReader reader = ExecuteReader(storedProcedure, namedParams, connection, command))
                    {
                        if (!ColumnExists(reader, columnName))
                            return result;

                        while (reader.Read())
                        {
                            if (typeof(T) == typeof(string))
                            {
                                result.Add((T)Convert.ChangeType(reader[columnName].SafelyToStringDefaultEmpty().Trim(), typeof(T)));
                            }
                            else if (typeof(T) == typeof(Int32))
                            {
                                result.Add((T)Convert.ChangeType(reader[columnName].SafelyToInt(), typeof(T)));
                            }
                            else if (typeof(T) == typeof(Int64))
                            {
                                result.Add((T)Convert.ChangeType(reader[columnName].SafelyToLong(), typeof(T)));
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Executes a stored procedure and returns a DataReader instance
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's naame</param>
        /// <param name="namedParams">use the new anonymous dictionary: new {name = "value", name2 = "value2" }</param>
        /// <param name="connectionStringStringType">Connection string type</param>
        /// <returns></returns>
        protected static IDataReader ExecuteReader(string storedProcedure, object namedParams,
            IDbConnection connection, IDbCommand command)
        {
            return ExecuteReader(storedProcedure, namedParams.ToDictionary(), connection, command);
        }

        /// <summary>
        /// Executes a stored procedure that doesn't return data
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's naame</param>
        /// <param name="namedParams">use the new anonymous dictionary: new {name = "value", name2 = "value2" }</param>
        /// <returns></returns>
        protected static int ExecuteNonQuery(string storedProcedure, object namedParams, bool excludeFlaggedOnInsert = false, bool excludeFlaggedOnUpdate = false)
        {
            return ExecuteNonQuery(storedProcedure, namedParams.ToDictionary(excludeFlaggedOnInsert, excludeFlaggedOnUpdate));
        }

        protected static void ExecuteQuery(string query)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var e = new DalException(query, ex);
                Logger.LogException(e);
                throw e;
            }
        }

        protected static object ExecuteScalar(string query)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                var e = new DalException(query, ex);
                Logger.LogException(e);
                throw e;
            }
        }

        public string GetVersion()
        {
            return ExecuteScalar("select @@version").ToString();
        }

        /// <summary>
        /// Execute a stored procedure and returns a DataReader instance
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's name</param>
        /// <param name="namedParams">The parameters dictionary</param>
        /// <param name="connectionStringStringType">The connection string type</param>
        /// <returns></returns>
        private static IDataReader ExecuteReader(string storedProcedure, Dictionary<string, object> namedParams,
            IDbConnection connection, IDbCommand command)
        {
            try
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedure;

                AddParamethers(command, namedParams);

                command.Prepare();
                return command.ExecuteReader();


            }
            catch (Exception ex)
            {
                var e = new DalException(storedProcedure, ex);
                Logger.LogException(e);
                throw e;
            }
        }

        /// <summary>
        /// This method executes a stored procedure that does not return any data
        /// </summary>
        /// <param name="connectionStringStringType">The connection string type</param>
        /// <param name="storedProcedure">Stored procedure's name</param>
        /// <param name="namedParams">Parameter values</param>
        private static int ExecuteNonQuery(string storedProcedure, Dictionary<string, object> namedParams)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storedProcedure;

                        AddParamethers(command, namedParams);

                        var param = command.CreateParameter();
                        param.DbType = DbType.Int32;
                        param.Direction = ParameterDirection.ReturnValue;
                        param.ParameterName = "@Return";
                        command.Parameters.Add(param);

                        command.Prepare();
                        command.ExecuteNonQuery();

                        param = (IDbDataParameter)command.Parameters["@Return"];
                        return (int)param.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                var e = new DalException(storedProcedure, ex);
                Logger.LogException(e);
                throw e;
            }
        }

        /// <summary>
        /// Add paramathers to the command instance specifed as paramather
        /// </summary>
        /// <param name="command">SqlCommand instance</param>
        /// <param name="namedParams">Named paramethers</param>
        private static void AddParamethers(IDbCommand command, Dictionary<string, object> namedParams)
        {
            if (namedParams == null) return;

            foreach (var item in namedParams)
            {
                var param = command.CreateParameter();
                param.ParameterName = item.Key;
                param.Value = item.Value == null ? DBNull.Value : item.Value;
                command.Parameters.Add(param);
            }

        }


        /// <summary>
        /// This method verifies if the datareader contains a given column name
        /// </summary>
        /// <param name="reader">The DataReader</param>
        /// <param name="columnName">The column nave to verify</param>
        /// <returns></returns>
        private static bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
