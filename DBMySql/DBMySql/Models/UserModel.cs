using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace DBMySql.Models
{
    [TableName("usrUser_usr")]
    public partial class UserModel : DatabaseModel
    {
        #region private properties

        #endregion

        #region public properties

        [TableFieldName("usrID")]
        [TableFieldExcludeFromUpdate(true)]
        [TableFieldExcludeFromInsert(true)]
        public int ID { get; set; }
        [TableFieldName("usrUID")]
        public string UID { get; set; }
        [TableFieldName("usrFirstName")]
        public string FirstName { get; set; }
        [TableFieldName("usrLastName")]
        public string LastName { get; set; }
        [TableFieldName("usrEmailAddress")]
        public string EmailAddress { get; set; }

        #endregion

        #region Constructors

        public UserModel() : base(MySqlConnectionString.ConnectionString) { }

        public UserModel(int id) : base(MySqlConnectionString.ConnectionString)
        {
            DataTable DataTable = _MySQLDBStateless.ExecDataTable($"SELECT * FROM {this.TableName()} WHERE {MySQLDBStateless.GetDatabaseTableFieldName(this, nameof(this.ID))} = {MySQLDBCommon.SetValueForSql(id)}");

            if (DataTable.Rows.Count == 1)
            {
                LoadByUserModelDataRow(DataTable.Rows[0]);
            }
        }

        public UserModel(DataRow dataRow) : base(MySqlConnectionString.ConnectionString)
        {
            LoadByUserModelDataRow(dataRow);
        }

        private void LoadByUserModelDataRow(DataRow dataRow)
        {
            this.ID = MySQLDBCommon.GetValueIntFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.ID)));
            this.UID = MySQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.UID)));
            this.FirstName = MySQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.FirstName)));
            this.LastName = MySQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.LastName)));
            this.EmailAddress = MySQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.EmailAddress)));
        }

        private object GetDatabaseTableFieldName(DataRow dataRow, string fieldName) => dataRow[MySQLDBStateless.GetDatabaseTableFieldName(this, fieldName)];
        

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
            this.ID = (int)_MySQLDBStateless.ExecInsertNonQueryReturnID(MySQLDBStateless.GenerateStandardInsertStatement(this));
        }

        private void Update()
        {
            _MySQLDBStateless.ExecNonQuery(MySQLDBStateless.GenerateStandardUpdateStatement(this, nameof(this.ID), this.ID));
        }

        public void Delete()
        {
            _MySQLDBStateless.ExecNonQuery(MySQLDBStateless.GenerateStandardDeleteStatement(this, nameof(this.ID), this.ID));
        }

        #endregion
    }
}
