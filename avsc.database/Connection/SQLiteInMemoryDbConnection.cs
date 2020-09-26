using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using AVSC.Database.Interfaces;

namespace AVSC.Database.Connection
{
    internal class SQLiteInMemoryDbConnection : IAVSCDatabase
    {
        public string GeneratorId { get; } = "Sqlite";

        public string ConnectionString { get; private set; } = "FullUri=file::memory:?cache=shared";
        
        public IDbConnection GetDbConnection()
        {
            return new SQLiteConnection(ConnectionString);
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