using System.Data;
using AVSC.Database.Connection;
using AVSC.Database.Enums;
using AVSC.Database.Exceptions;
using AVSC.Database.Interfaces;
using AVSC.Database.Settings;
using Microsoft.Extensions.Configuration;

namespace AVSC.Database
{
    public static class AVSCDatabaseFactory
    {
        #region Get IAVSCDatabase
        public static IAVSCDatabase GetIAVSCDatabase
        (
            DatabaseType databaseType,
            string serverName,
            string username,
            string password,
            string databaseName
        )
        {
            IAVSCDatabase db;

            switch (databaseType)
            {
                case DatabaseType.Postgres:
                {
                    db = new PostgresDbConnection(serverName, username, password, databaseName);
                }
                break;
                case DatabaseType.MySql:
                {
                    db = new MySqlDbConnection(serverName, username, password, databaseName);
                }
                break;
                case DatabaseType.SqlServer:
                {
                    db = new SqlServerDbConnection(serverName, username, password, databaseName);
                }
                break;
                default:
                {
                    throw new NotSupportedDatabaseException();
                }
            }

            return db;
        }

        public static IAVSCDatabase GetIAVSCDatabase
        (
            DatabaseType databaseType,
            string connectionString
        )
        {
            IAVSCDatabase db = null;

            switch (databaseType)
            {
                case DatabaseType.Postgres:
                {
                    db = new PostgresDbConnection(connectionString);
                }
                break;
                case DatabaseType.MySql:
                {
                    db = new MySqlDbConnection(connectionString);
                }
                break;
                case DatabaseType.SqlServer:
                {
                    db = new SqlServerDbConnection(connectionString);
                }
                break;
                default:
                {
                    throw new NotSupportedDatabaseException();
                }
            }

            return db;
        }

        public static IAVSCDatabase GetIAVSCInMemoryDatabase()
        {
            return new SQLiteInMemoryDbConnection();
        }

        public static IAVSCDatabase GetIAVSCDatabase(IConfiguration configuration)
        {
			var databaseSettings = new DatabaseSettings();
			configuration.Bind(databaseSettings);

			return AVSCDatabaseFactory.GetIAVSCDatabase
            (
                databaseSettings.DatabaseType,
                databaseSettings.ServerName,
                databaseSettings.Username, 
                databaseSettings.Password, 
                databaseSettings.DatabaseName
            );
        }
        #endregion // Get AVSCDatabase

        #region Get IDbConnection
        public static IDbConnection GetIDbConnection
        (
            DatabaseType databaseType,
            string serverName,
            string username,
            string password,
            string databaseName
        )
        {
            IAVSCDatabase db = AVSCDatabaseFactory.GetIAVSCDatabase(databaseType, serverName, username, password, databaseName);

            return db.GetDbConnection();
        }

        public static IDbConnection GetIDbConnection
        (
            DatabaseType databaseType,
            string connectionString
        )
        {
            IAVSCDatabase db = AVSCDatabaseFactory.GetIAVSCDatabase(databaseType, connectionString);

            return db.GetDbConnection();
        }

        public static IDbConnection GetInMemoryIDbConnection()
        {
            IAVSCDatabase db = AVSCDatabaseFactory.GetIAVSCInMemoryDatabase();

            return db.GetDbConnection();
        }

        public static IDbConnection GetIDbConnection(IConfiguration configuration)
        {
			var databaseSettings = new DatabaseSettings();
			configuration.Bind(databaseSettings);

			return AVSCDatabaseFactory.GetIDbConnection
            (
                databaseSettings.DatabaseType,
                databaseSettings.ServerName,
                databaseSettings.Username, 
                databaseSettings.Password, 
                databaseSettings.DatabaseName
            );
        }
        #endregion // Get IDbConnection
    }
}