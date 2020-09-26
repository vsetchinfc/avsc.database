using System.Collections.Generic;
using System.Data;
using AVSC.Database.Interfaces;
using MySqlConnector;

namespace AVSC.Database.Connection
{
    internal class MySqlDbConnection : IAVSCDatabase
    {
        public string GeneratorId { get; } = "MySql";
        
        public string ConnectionString { get; private set; } = string.Empty;

        public MySqlDbConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public MySqlDbConnection
        (
            string serverName,
            string username,
            string password,
            string databaseName
        )
        {
            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder() 
            {
                Server = serverName,
                UserID = username,
                Password = password,
                Database = databaseName
            };

            this.ConnectionString = mySqlConnectionStringBuilder.ToString();
        }

        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(this.ConnectionString);
        }

        public bool CreateDatabase()
        {
            return true;
        }

        public bool DeleteDatabase()
        {
            return true;
        }

        public List<string> GetDatabases()
        {
            return new List<string>();
        }

        public bool DatabaseExists()
        {
            return true;
        }
    }
}