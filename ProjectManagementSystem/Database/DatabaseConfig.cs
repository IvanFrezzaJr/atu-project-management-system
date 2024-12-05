using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Database
{
    public class DatabaseConfig
    {
        private const string _dbFile = "database.db";

        private string ConnectionString = $"Data Source={_dbFile};Version=3;"; // Path to the SQLite database file

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
}


