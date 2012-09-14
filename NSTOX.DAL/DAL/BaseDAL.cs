using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using NSTOX.DAL.Helper;
using System.Configuration;
using NSTOX.DAL.Model;

namespace NSTOX.DAL.DAL
{
    public class BaseDAL
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["NSTOXConnectionString"].ConnectionString; } }

        private static SqlConnection connection;
        protected static SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(ConnectionString);
                }
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
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

            using (SqlDataReader reader = ExecuteReader(storedProcedure, namedParams))
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

            return result;
        }

        protected static List<T> GetFromDBAndMapToList<T>(string storedProcedure, string columnName, object namedParams)
        {
            List<T> result = new List<T>();

            using (SqlDataReader reader = ExecuteReader(storedProcedure, namedParams))
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

            return result;
        }

        /// <summary>
        /// Executes a stored procedure and returns a DataReader instance
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's naame</param>
        /// <param name="namedParams">use the new anonymous dictionary: new {name = "value", name2 = "value2" }</param>
        /// <param name="connectionStringStringType">Connection string type</param>
        /// <returns></returns>
        protected static SqlDataReader ExecuteReader(string storedProcedure, object namedParams)
        {
            return ExecuteReader(storedProcedure, namedParams.ToDictionary());
        }

        /// <summary>
        /// Executes a stored procedure using the parameter values and returns a DataReader instance
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's name</param>
        /// <param name="parameterValues">Parameter values</param>
        /// <param name="connectionStringStringType">The type of the connection string</param>
        /// <returns></returns>
        protected static SqlDataReader ExecuteReader(string storedProcedure, params object[] parameterValues)
        {
            try
            {
                return SqlHelper.ExecuteReader(Connection, storedProcedure, parameterValues);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// This method executes a stored procedure that does not return any data
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's name</param>
        /// <param name="parameterValues">Parameter values</param>
        /// <param name="connectionStringStringType">The type of the connection string</param>
        protected static int ExecuteNonQuery(string storedProcedure, params object[] parameterValues)
        {
            try
            {
                Logger.LogInfo(string.Format("Running ExecuteNonQuery: SP = {0}, Params = {1}", storedProcedure, string.Join(",", parameterValues)));
                return SqlHelper.ExecuteNonQuery(Connection, storedProcedure, parameterValues);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
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
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Execute a stored procedure and returns a DataReader instance
        /// </summary>
        /// <param name="storedProcedure">Stored procedure's name</param>
        /// <param name="namedParams">The parameters dictionary</param>
        /// <param name="connectionStringStringType">The connection string type</param>
        /// <returns></returns>
        private static SqlDataReader ExecuteReader(string storedProcedure, Dictionary<string, object> namedParams)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    AddParamethers(command, namedParams);

                    command.Prepare();
                    return command.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
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
                using (SqlCommand command = new SqlCommand(storedProcedure, Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    AddParamethers(command, namedParams);
                    command.Parameters.Add("@Return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    command.Prepare();
                    command.ExecuteNonQuery();
                    return (int)command.Parameters["@Return"].Value;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Add paramathers to the command instance specifed as paramather
        /// </summary>
        /// <param name="command">SqlCommand instance</param>
        /// <param name="namedParams">Named paramethers</param>
        private static void AddParamethers(SqlCommand command, Dictionary<string, object> namedParams)
        {
            if (namedParams == null) return;

            namedParams.ToList().ForEach(p => command.Parameters.AddWithValue(p.Key, p.Value == null ? DBNull.Value : p.Value));
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
