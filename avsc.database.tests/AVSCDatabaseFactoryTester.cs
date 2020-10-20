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

namespace AVSC.Database.Tests
{
    public class AVSCDatabaseFactoryTester
    {
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
            var db = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType, connectionString: string.Empty);
            db.GeneratorId.Should().Be(expectedGeneratorId);
            var iDbConnection = db.GetDbConnection();
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
    }
}