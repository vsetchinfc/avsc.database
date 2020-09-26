using System.Collections.Generic;
using System.Data;
using AVSC.Database.Interfaces;
using Npgsql;

namespace AVSC.Database.Connection
{
    internal class PostgresDbConnection : IAVSCDatabase
    {
        public string GeneratorId { get; } = "Postgres";

        public string ConnectionString { get; private set; } = string.Empty;

        public PostgresDbConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public PostgresDbConnection
        (
            string serverName,
            string username,
            string password,
            string databaseName
        )
        {
            NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new NpgsqlConnectionStringBuilder() 
            {
                Host = serverName,
                Username = username,
                Password = password,
                Database = databaseName
            };

            ConnectionString = postgresConnectionStringBuilder.ConnectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(this.ConnectionString);
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