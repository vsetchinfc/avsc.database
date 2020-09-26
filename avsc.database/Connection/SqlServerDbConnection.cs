using System.Collections.Generic;
using System.Data;
using AVSC.Database.Interfaces;

namespace AVSC.Database.Connection
{
    internal class SqlServerDbConnection : IAVSCDatabase
    {
        public string GeneratorId { get; } = "SqlServer";

        public string ConnectionString { get; private set; } = string.Empty;

        public SqlServerDbConnection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SqlServerDbConnection
        (
            string serverName,
            string username,
            string password,
            string databaseName
        )
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                UserID = username,
                Password = password,
                InitialCatalog = databaseName
            };

            ConnectionString = connectionStringBuilder.ConnectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new System.Data.SqlClient.SqlConnection(this.ConnectionString);
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
