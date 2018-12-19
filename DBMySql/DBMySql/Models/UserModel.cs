using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace DBMySql.Models
{
    [TableName("usrUser_usr")]
    public partial class UserModel : IDatabaseModel
    {
        #region private properties

        #endregion

        #region public properties

        [TableFieldName("usrID")]
        [TableFieldExcludeFromUpdate(true)]
        [TableFieldExcludeFromInsert(true)]
        public int ID;
        [TableFieldName("usrUID")]
        public string UID;
        [TableFieldName("usrFirstName")]
        public string FirstName;
        [TableFieldName("usrLastName")]
        public string LastName;
        [TableFieldName("usrEmailAddress")]
        public string EmailAddress;

        #endregion

        #region Constructors

        public UserModel() { }

            public UserModel(int id)
        {
            DataTable DataTable = MySQLDBStateless.ExecDataTable($"SELECT * FROM {this.TableName()} WHERE usrID = {id}");

            if (DataTable.Rows.Count == 1)
            {
                LoadByUserModelDataRow(DataTable.Rows[0]);
            }
        }

        private void LoadByUserModelDataRow(DataRow dataRow)
        {
            this.ID = MySQLDBCommon.GetValueIntFromSql(dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this.GetType().GetField(nameof(this.ID)))]);
            this.UID = MySQLDBCommon.GetValueStringFromSql(dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this.GetType().GetField(nameof(this.UID)))]); ;
            this.FirstName = MySQLDBCommon.GetValueStringFromSql(dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this.GetType().GetField(nameof(this.FirstName)))]);
            this.LastName = MySQLDBCommon.GetValueStringFromSql(dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this.GetType().GetField(nameof(this.LastName)))]);
            this.EmailAddress = MySQLDBCommon.GetValueStringFromSql(dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this.GetType().GetField(nameof(this.EmailAddress)))]);
        }

        #endregion

        #region Additional Methods

        private List<string> ModelFields
        {
            get { return MySQLDBStateless.ModelFieldNames(typeof(UserModel)); }
        }

        private List<object> ModelValues
        {
            get { return MySQLDBStateless.ModelFieldValues(this); }        
        }

        #endregion

        #region CRUD Methods

        public void Save()
        {
            if (this.ID.IsEmpty())
                Insert();
            else
                Update();

        }

        private void Insert()
        {
            this.ID = (int)MySQLDBStateless.
                ExecInsertNonQueryReturnID($"INSERT INTO {this.TableName()} {MySQLDBStateless.GenerateInsertFields(this)}");
        }

        private void Update()
        {
            MySQLDBStateless.ExecNonQuery($"UPDATE {this.TableName()} SET {MySQLDBStateless.GenerateUpdateFields(this)} WHERE usrID = {this.ID}");
        }

        private void Delete()
        {
            MySQLDBStateless.
                ExecInsertNonQueryReturnID($"DELETE FROM {this.TableName()} WHERE usrID = {this.ID}");
        }

        #endregion
    }
}
