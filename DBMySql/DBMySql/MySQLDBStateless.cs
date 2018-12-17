using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DBMySql
{
    public static class MySQLDBStateless
    {
        private static MySqlConnection OpenConnection()
        {
            MySqlConnection MySqlConnection = new MySqlConnection("connectionString");

            MySqlConnection.Open();

            return MySqlConnection;
        }

        private static MySqlTransaction BeginTransaction()
        {
            return OpenConnection().BeginTransaction();
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

        public static int ExecNonQuery(string sql)
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

        public static bool ExecNonQueryTransaction(List<string> sqlStatements)
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
                }
            }

            return true;
        }

        public static long ExecInsertNonQueryReturnID(string sql)
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
                }
            }

            return RowID;
        }

        public static object ExecScalar(string sql)
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

        public static MySqlDataReader ExecDataReader(string sql)
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

        public static DataTable ExecDataTable(string sql)
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

        public static string ModelFieldNames(Type model)
        {
            StringBuilder StringBuilder = new StringBuilder();

            foreach (FieldInfo Field in model.GetFields(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetField | BindingFlags.Instance))
            {
                string Value = (string)Field.CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute)).First().ConstructorArguments.First().Value;
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{Value}" : $", {Value}");
            }

            return StringBuilder.ToString();
        }

        public static string ModelFieldValues(object model)
        {
            StringBuilder StringBuilder = new StringBuilder();

            foreach (var properties in model.GetType().GetFields(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetField | BindingFlags.Instance))
            {
                object Value = properties.GetValue(model);
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{MySQLDBCommon.SetValueForSql(Value)}" : $", {MySQLDBCommon.SetValueForSql(Value)}");
            }

            return StringBuilder.ToString();
        }

        public static string GenerateInsertFields(IDatabaseModel model)
        {
            StringBuilder StringBuilderFields = new StringBuilder();
            StringBuilder StringBuilderValues = new StringBuilder();

            foreach (FieldInfo Field in model.GetType().GetFields(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetField | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = Field.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromUpdateAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = Field.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = Field.GetValue(model);

                StringBuilderFields.Append(StringBuilderFields.Length == 0 ? $"({FieldName}" : $", {FieldName}");
                StringBuilderValues.Append(StringBuilderValues.Length == 0 ? $"{MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }

            return StringBuilderFields.Append($") VALUES ({StringBuilderValues.ToString()})").ToString();
        }

        public static string GenerateUpdateFields(IDatabaseModel model)
        {
            StringBuilder StringBuilder = new StringBuilder();

            foreach (FieldInfo Field in model.GetType().GetFields(System.Reflection.BindingFlags.Public
                  | System.Reflection.BindingFlags.GetField | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = Field.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromUpdateAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = Field.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = Field.GetValue(model);
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }

            return StringBuilder.ToString();
        }
    }
}