using NUnit.Framework;
using AVSC.Database.Enums;
using FluentAssertions;
using AVSC.Database.Exceptions;
using System.Data;
using System.Data.SQLite;
using System;
using Npgsql;
using MySqlConnector;
using System.Data.SqlClient;
using FizzWare.NBuilder;
using Moq;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AVSC.Database.Tests
{
    public class AVSCDatabaseFactoryTester
    {
        public static string AppSettingsFile = "appsettings.json";

        [TestCase(DatabaseType.MySql, "MySql", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, "Postgres", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "SqlServer", typeof(SqlConnection))]
        public void GetIAVSCDatabaseByDatabaseType
        (
            DatabaseType databaseType,
            string expectedGeneratorId,
            Type expectedDatabaseType
        )
        {
            var avscDatabase = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType, connectionString: string.Empty);
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

        [TestCase(DatabaseType.MySql, "MySql", typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, "Postgres", typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, "SqlServer", typeof(SqlConnection))]
        public void GetIAVSCDatabaseByParametersSuccess
        (
            DatabaseType databaseType,
            string expectedGeneratorId,
            Type expectedDatabaseType
        )
        {
            var avscDatabase = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType,
                serverName: string.Empty,
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

        [TestCase(DatabaseType.MySql, typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, typeof(SqlConnection))]
        public void GetIDbConnectionByParametersSuccess
        (
            DatabaseType databaseType,
            Type expectedDatabaseType
        )
        {
            var iDbConnection = AVSCDatabaseFactory.GetIDbConnection(
                databaseType,
                serverName: string.Empty,
                username: string.Empty,
                password: string.Empty,
                databaseName: string.Empty
            );

            iDbConnection.GetType().Should().Be(expectedDatabaseType);
        }

        [TestCase(DatabaseType.MySql, typeof(MySqlConnection))]
        [TestCase(DatabaseType.Postgres, typeof(NpgsqlConnection))]
        [TestCase(DatabaseType.SqlServer, typeof(SqlConnection))]
        public void GetIDbConnectionByDatabaseTypeAndConnectionStringSuccess
        (
            DatabaseType databaseType,
            Type expectedDatabaseType
        )
        {
            var iDbConnection = AVSCDatabaseFactory.GetIDbConnection(
                databaseType,
                connectionString: string.Empty
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