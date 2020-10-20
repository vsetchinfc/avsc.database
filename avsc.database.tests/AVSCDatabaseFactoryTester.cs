using NUnit.Framework;
using AVSC.Database.Enums;
using FluentAssertions;
using AVSC.Database.Exceptions;

namespace AVSC.Database.Tests
{
    public class AVSCDatabaseFactoryTester
    {
        [TestCase(DatabaseType.MySql, "MySql")]
        [TestCase(DatabaseType.Postgres, "Postgres")]
        [TestCase(DatabaseType.SqlServer, "SqlServer")]
        public void GetIAVSCDatabaseByDatabaseType
        (
            DatabaseType databaseType,
            string expectedGeneratorId
        )
        {
            string connectionString = string.Empty;

            var db
            = AVSCDatabaseFactory.GetIAVSCDatabase(
                databaseType, connectionString);
            db.GeneratorId.Should().Be(expectedGeneratorId);
        }

        [Test]
        public void GetIAVSCDatabaseByNoneDatabaseType()
        {
            Assert.Throws<NotSupportedDatabaseException>(() =>
                AVSCDatabaseFactory.GetIAVSCDatabase(
                    DatabaseType.None,
                    string.Empty
                )
            );
        }
    }
}