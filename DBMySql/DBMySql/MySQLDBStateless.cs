using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;
using System.IO;

namespace DBMySql
{
    public class MySQLDBStateless
    {
        private string _ConnectionString { get; set; }

        public MySQLDBStateless(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        private MySqlConnection OpenConnection()
        {
            MySqlConnection MySqlConnection = new MySqlConnection(_ConnectionString);

            MySqlConnection.Open();

            return MySqlConnection;
        }

        private static MySqlConnection OpenConnection(string connectionString)
        {
            MySqlConnection MySqlConnection = new MySqlConnection(connectionString);

            MySqlConnection.Open();

            return MySqlConnection;
        }

        private MySqlTransaction BeginTransaction()
        {
            return OpenConnection().BeginTransaction();
        }

        private static MySqlTransaction BeginTransaction(string connectionString)
        {
            return OpenConnection(connectionString).BeginTransaction();
        }

        private static MySqlCommand Command(string sql, MySqlConnection mysqlConnection)
        {
            return new MySqlCommand(sql, mysqlConnection);
        }

        private static MySqlCommand Command(string sql, MySqlTransaction mysqlTransaction)
        {
            return new MySqlCommand(sql, mysqlTransaction.Connection, mysqlTransaction);
        }

        private static MySqlDataAdapter DataAdapter(string sql, MySqlConnection mysqlConnection)
        {
            return new MySqlDataAdapter(sql, mysqlConnection);
        }

        public int ExecNonQuery(string sql)
        {
            int Results = -1;
            MySqlConnection MySqlConnection = null;

            try
            {
                MySqlConnection = OpenConnection();

                Results = Command(sql, MySqlConnection).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (MySqlConnection != null)
                    MySqlConnection.Close();
            }

            return Results;
        }

        public static int ExecNonQuery(string sql, string connectionString)
        {
            int Results = -1;
            MySqlConnection MySqlConnection = null;

            try
            {
                MySqlConnection = OpenConnection(connectionString);

                Results = Command(sql, MySqlConnection).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (MySqlConnection != null)
                    MySqlConnection.Close();
            }

            return Results;
        }

        public bool ExecNonQueryTransaction(List<string> sqlStatements)
        {
            MySqlTransaction MySqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                MySqlTransaction = BeginTransaction();

                foreach (string sqlStatement in sqlStatements)
                {
                    Command(sqlStatement, MySqlTransaction).ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Rollback();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                    return false;
                }
            }
            finally
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Commit();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }

            return true;
        }

        public static bool ExecNonQueryTransaction(List<string> sqlStatements, string connectionString)
        {
            MySqlTransaction MySqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                MySqlTransaction = BeginTransaction(connectionString);

                foreach (string sqlStatement in sqlStatements)
                {
                    Command(sqlStatement, MySqlTransaction).ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Rollback();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                    return false;
                }
            }
            finally
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Commit();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }

            return true;
        }

        public long ExecInsertNonQueryReturnID(string sql)
        {
            long RowID = MySQLDBCommon.EmptyLong;
            MySqlTransaction MySqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                MySqlTransaction = BeginTransaction();

                MySqlCommand MySqlCommand = Command(sql, MySqlTransaction);
                MySqlCommand.ExecuteNonQuery();

                RowID = MySqlCommand.LastInsertedId;
            }
            catch (Exception exception)
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Rollback();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }
            finally
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Commit();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }

            return RowID;
        }

        public static long ExecInsertNonQueryReturnID(string sql, string connectionString)
        {
            long RowID = MySQLDBCommon.EmptyLong;
            MySqlTransaction MySqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                MySqlTransaction = BeginTransaction(connectionString);

                MySqlCommand MySqlCommand = Command(sql, MySqlTransaction);
                MySqlCommand.ExecuteNonQuery();

                RowID = MySqlCommand.LastInsertedId;
            }
            catch (Exception exception)
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Rollback();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }
            finally
            {
                if (MySqlTransaction != null)
                {
                    MySqlTransaction.Commit();
                    MySqlTransaction.Dispose();
                    MySqlTransaction = null;
                }
            }

            return RowID;
        }

        public object ExecScalar(string sql)
        {
            {
                MySqlConnection MySqlConnection = null;
                object ScalarReturnObject = null;

                try
                {
                    MySqlConnection = OpenConnection();

                    ScalarReturnObject = Command(sql, MySqlConnection).ExecuteScalar();
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (MySqlConnection != null)
                        MySqlConnection.Close();
                }

                return ScalarReturnObject;
            }
        }

        public static object ExecScalar(string sql, string connectionString)
        {
            {
                MySqlConnection MySqlConnection = null;
                object ScalarReturnObject = null;

                try
                {
                    MySqlConnection = OpenConnection(connectionString);

                    ScalarReturnObject = Command(sql, MySqlConnection).ExecuteScalar();
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (MySqlConnection != null)
                        MySqlConnection.Close();
                }

                return ScalarReturnObject;
            }
        }

        public MySqlDataReader ExecDataReader(string sql)
        {
            MySqlConnection MySqlConnection = null;
            MySqlDataReader MySqlDataReader = null;

            try
            {
                MySqlConnection = OpenConnection();

                MySqlDataReader = Command(sql, MySqlConnection).ExecuteReader();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (MySqlConnection != null)
                    MySqlConnection.Close();
            }

            return MySqlDataReader;
        }

        public static MySqlDataReader ExecDataReader(string sql, string connectionString)
        {
            MySqlConnection MySqlConnection = null;
            MySqlDataReader MySqlDataReader = null;

            try
            {
                MySqlConnection = OpenConnection(connectionString);

                MySqlDataReader = Command(sql, MySqlConnection).ExecuteReader();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (MySqlConnection != null)
                    MySqlConnection.Close();
            }

            return MySqlDataReader;
        }

        public DataTable ExecDataTable(string sql)
        {
            {
                MySqlConnection MySqlConnection = null;
                MySqlDataAdapter MySqlDataAdapter = null;
                DataTable DataTable = new DataTable();

                try
                {
                    MySqlConnection = OpenConnection();

                    MySqlDataAdapter = DataAdapter(sql, MySqlConnection);
                    MySqlDataAdapter.Fill(DataTable);
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (MySqlConnection != null)
                        MySqlConnection.Close();
                }

                return DataTable;
            }
        }

        public static DataTable ExecDataTable(string sql, string connectionString)
        {
            {
                MySqlConnection MySqlConnection = null;
                MySqlDataAdapter MySqlDataAdapter = null;
                DataTable DataTable = new DataTable();

                try
                {
                    MySqlConnection = OpenConnection(connectionString);

                    MySqlDataAdapter = DataAdapter(sql, MySqlConnection);
                    MySqlDataAdapter.Fill(DataTable);
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (MySqlConnection != null)
                        MySqlConnection.Close();
                }

                return DataTable;
            }
        }

        public static string GetDatabaseTableFieldName(object model, string fieldName)
        {
            return (string)model.GetType().GetProperty(fieldName)
                .CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute))
                .First()
                .ConstructorArguments
                .First()
                .Value;
        }

        public static string GetDatabaseTableFieldName(FieldInfo fieldInfo)
        {
            return (string)fieldInfo
                .CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute))
                .First()
                .ConstructorArguments
                .First()
                .Value;
        }

        public static List<string> ModelFieldNames(Type model)
        {
            List<String> ModelFieldNames = new List<string>();

            foreach (PropertyInfo property in model.GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                string Value = (string)property.CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute)).First().ConstructorArguments.First().Value;
                ModelFieldNames.Add(Value);
            }

            return ModelFieldNames;
        }

        public static List<object> ModelFieldValues(object model)
        {
            List<object> ModelFieldValues = new List<object>();

            foreach (var properties in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                object Value = properties.GetValue(model);
                ModelFieldValues.Add(MySQLDBCommon.SetValueForSql(Value));
            }

            return ModelFieldValues;
        }

        public static string GenerateInsertFields(DatabaseModel model)
        {
            StringBuilder StringBuilderFields = new StringBuilder();
            StringBuilder StringBuilderValues = new StringBuilder();

            foreach (PropertyInfo property in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromInsertAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = property.GetValue(model);

                StringBuilderFields.Append(StringBuilderFields.Length == 0 ? $"({FieldName}" : $", {FieldName}");
                StringBuilderValues.Append(StringBuilderValues.Length == 0 ? $"{MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }

            return StringBuilderFields.Append($") VALUES ({StringBuilderValues.ToString()})").ToString();
        }

        public static string GenerateUpdateFields(DatabaseModel model)
        {
            StringBuilder StringBuilder = new StringBuilder();

            foreach (PropertyInfo property in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                  | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromUpdateAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = property.GetValue(model);
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }

            return StringBuilder.ToString();
        }

        public static string GenerateStandardInsertStatement(DatabaseModel model)
        {
            return $"INSERT INTO {model.TableName()} {GenerateInsertFields(model)}";
        }

        public static string GenerateStandardUpdateStatement(DatabaseModel model, string primaryKeyFieldName, object primaryKeyValue)
        {
            return $"UPDATE {model.TableName()} SET {GenerateUpdateFields(model)} WHERE {GetDatabaseTableFieldName(model, primaryKeyFieldName)} = {MySQLDBCommon.SetValueForSql(primaryKeyValue)}";
        }

        public static string GenerateStandardDeleteStatement(DatabaseModel model, string primaryKeyFieldName, object primaryKeyValue)
        {
            return $"DELETE FROM {model.TableName()} WHERE {GetDatabaseTableFieldName(model, primaryKeyFieldName)} = {MySQLDBCommon.SetValueForSql(primaryKeyValue)}";
        }
    }
}