using System.Collections.Generic;
using System.Data;

namespace AVSC.Database.Interfaces
{
    public interface IAVSCDatabase
    {
        /// <summary>
        /// Generator Id for use with Fluent Migrator 
        /// </summary>
        /// <value>string</value>
        string GeneratorId { get; }
        IDbConnection GetDbConnection();
        string ConnectionString { get; }
        bool CreateDatabase();
        bool DeleteDatabase();
        List<string> GetDatabases();
        bool DatabaseExists();
    }
}
