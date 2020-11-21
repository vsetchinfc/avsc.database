using NUnit.Framework;
using AVSC.Database.Enums;
using FluentAssertions;
using AVSC.Database.Exceptions;
using System.Data.SQLite;
using System;
using Npgsql;
using MySqlConnector;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AVSC.Database.Tests
{
    public class AVSCDatabaseFactoryTester
    {
        public static string AppSettingsFile = "appsettings.json";

        [TestCase(DatabaseType.MySql, "MySql", "", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, "Postgres",
            "Host=localhost;Port=5432;", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "SqlServer",
            "", typeof(SqlConnection))]
        public void GetIAVSCDatabaseByDatabaseType
        (
            DatabaseType databaseType,
            string expectedGeneratorId,
            string connectionString,
            Type expectedDatabaseType
        )
        {
            var avscDatabase = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType, connectionString);
            avscDatabase.GeneratorId.Should().Be(expectedGeneratorId);
            var iDbConnection = avscDatabase.GetDbConnection();
            iDbConnection.GetType().Should().Be(expectedDatabaseType);
        }

        [Test]
        public void GetIAVSCDatabaseByNoneDatabaseType()
        {
            Assert.Throws<NotSupportedDatabaseException>(() =>
                AVSCDatabaseFactory.GetIAVSCDatabase(
                    DatabaseType.None,
                    connectionString: string.Empty
                )
            );
        }

        [Test]
        public void GetIAVSCInMemoryDatabaseSuccess()
        {
            var db = AVSCDatabaseFactory.GetIAVSCInMemoryDatabase();
            db.GeneratorId.Should().Be("Sqlite");
        }

        [Test]
        public void GetInMemoryIDbConnectionSuccess()
        {
            var iDbConnection = AVSCDatabaseFactory.GetInMemoryIDbConnection();
            iDbConnection.Should().BeOfType<SQLiteConnection>();
        }

        [TestCase(DatabaseType.MySql, "MySql", "", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, "Postgres", "localhost", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "SqlServer", "", typeof(SqlConnection))]
        public void GetIAVSCDatabaseByParametersSuccess
        (
            DatabaseType databaseType,
            string expectedGeneratorId,
            string serverName,
            Type expectedDatabaseType
        )
        {
            var avscDatabase = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType,
                serverName,
                username: string.Empty,
                password: string.Empty,
                databaseName: string.Empty
            );

            avscDatabase.GeneratorId.Should().Be(expectedGeneratorId);
            var iDbConnection = avscDatabase.GetDbConnection();
            iDbConnection.GetType().Should().Be(expectedDatabaseType);
        }

        [Test]
        public void GetIAVSCDatabaseByParametersAndNoneDatabaseType()
        {
            Assert.Throws<NotSupportedDatabaseException>(() =>
                AVSCDatabaseFactory.GetIAVSCDatabase(
                    DatabaseType.None,
                    serverName: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    databaseName: string.Empty
                )
            );
        }

        [TestCase(DatabaseType.MySql, "", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, "localhost", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "", typeof(SqlConnection))]
        public void GetIDbConnectionByParametersSuccess
        (
            DatabaseType databaseType,
            string serverName,
            Type expectedDatabaseType
        )
        {
            var iDbConnection = AVSCDatabaseFactory.GetIDbConnection(
                databaseType,
                serverName,
                username: string.Empty,
                password: string.Empty,
                databaseName: string.Empty
            );

            iDbConnection.GetType().Should().Be(expectedDatabaseType);
        }

        [TestCase(DatabaseType.MySql, "", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres,
            "Host=localhost;Port=5432;", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "", typeof(SqlConnection))]
        public void GetIDbConnectionByDatabaseTypeAndConnectionStringSuccess
        (
            DatabaseType databaseType,
            string connectionString,
            Type expectedDatabaseType
        )
        {
            var iDbConnection = AVSCDatabaseFactory.GetIDbConnection(
                databaseType,
                connectionString
            );

            iDbConnection.GetType().Should().Be(expectedDatabaseType);
        }

        private IConfigurationSection GetDatabaseSettings
        (
            string configurationSectionName
        )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    AppSettingsFile,
                    optional: true,
                    reloadOnChange: true
                );

            var configuration = builder.Build();
            return configuration.GetSection(configurationSectionName);
        }

        [Test]
        public void GetIAVSCDatabaseByConfigurationSuccess()
        {
            var databaseSettingsSection
                = GetDatabaseSettings("DatabaseSettings");

            var avscDatabase
                = AVSCDatabaseFactory.GetIAVSCDatabase(databaseSettingsSection);

            avscDatabase.GeneratorId.Should().Be("MySql");
        }

        [Test]
        public void GetIDbConnectionByConfigurationSuccess()
        {
            var databaseSettingsSection
                = GetDatabaseSettings("DatabaseSettings");

            var iDbConnection
                = AVSCDatabaseFactory.GetIDbConnection(databaseSettingsSection);

            iDbConnection.GetType().Should().Be(typeof(MySqlConnection));
        }
    }
}