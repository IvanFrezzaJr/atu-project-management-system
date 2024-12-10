using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManagementSystem.Database;
using System.Data.SQLite;
using System.IO;

namespace ProjectManagementSystem.Tests
{
    [TestClass]
    public class DatabaseConfigTests
    {
        private const string TestDbFile = "database.tests.db";

        [TestInitialize]
        public void Setup()
        {
            // Remove the test database file before each test
            if (File.Exists(TestDbFile))
            {
                File.Delete(TestDbFile);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Ensure the test database file is removed after each test
            if (File.Exists(TestDbFile))
            {
                File.Delete(TestDbFile);
            }
        }

        [TestMethod]
        public void Test_CreateDatabase_CreatesFileAndTables()
        {
            // Arrange
            var config = new DatabaseConfig(TestDbFile);

            // Act
            config.CreateDatabase();

            // Assert
            // Check if the database file was created
            Assert.IsTrue(File.Exists(TestDbFile), "The database file was not created.");

            // Verify that all tables were created
            using (var connection = config.CreateConnection())
            {
                connection.Open();

                string[] tables = { "Role", "Classroom", "Enrollment", "Assessment", "Submission", "Attendance", "Logs" };
                foreach (var table in tables)
                {
                    using (var command = new SQLiteCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{table}';", connection))
                    {
                        var result = command.ExecuteScalar();
                        Assert.IsNotNull(result, $"The table '{table}' was not created.");
                    }
                }
            }
        }

        [TestMethod]
        public void Test_CreateDatabase_InsertsDefaultUsers()
        {
            // Arrange
            var config = new DatabaseConfig(TestDbFile);

            // Act
            config.CreateDatabase();

            // Assert
            // Verify that the default users are inserted into the Role table
            using (var connection = config.CreateConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Role WHERE Username IN ('admin', 'principal', 'staff', 'teacher', 'student');";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = (long)command.ExecuteScalar();
                    Assert.AreEqual(5, result, "Default users were not inserted correctly.");
                }
            }
        }

        [TestMethod]
        public void Test_CreateConnection_ReturnsValidConnection()
        {
            // Arrange
            var config = new DatabaseConfig(TestDbFile);

            // Act
            var connection = config.CreateConnection();

            // Assert
            // Check that the connection object is not null and has the correct connection string
            Assert.IsNotNull(connection, "The returned connection is null.");
            Assert.AreEqual($"Data Source={TestDbFile};Version=3;", connection.ConnectionString, "The connection string is incorrect.");
        }
    }
}
