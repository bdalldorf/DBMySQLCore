using System;
using System.Collections.Generic;
using System.Text;

namespace DBMySql
{
    public class DatabaseModel
    {
        private string _ConnectionString {get; set;}
        protected MySQLDBStateless _MySQLDBStateless { get; set; }

        public DatabaseModel(string connectionString)
        {
            if (_MySQLDBStateless == null || _ConnectionString != connectionString)
            {
                _ConnectionString = connectionString;
                _MySQLDBStateless = new MySQLDBStateless(connectionString);
            }
        }
    }
}
