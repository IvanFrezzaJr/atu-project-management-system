using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManagementSystem.Database;
using System.Data.SQLite;
using System.IO;

namespace ProjectManagementSystem.Tests
{
    [TestClass]
    public class AuthRepositoryTests
    {
        private const string TestDbFile = "authrepository.tests.db";
        private DatabaseConfig _config;

        [TestInitialize]
        public void Setup()
        {
            // Remove the test database file before each test
            if (File.Exists(TestDbFile))
            {
                File.Delete(TestDbFile);
            }

            // Initialize the DatabaseConfig with the test database file
            _config = new DatabaseConfig(TestDbFile);

            // Create the database and tables
            _config.CreateDatabase();

            // Seed the database with test data
            SeedTestData();
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
        public void Test_Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.Authenticate("admin", "admin");

            // Assert
            Assert.IsTrue(result, "Authentication failed with valid credentials.");
        }

        [TestMethod]
        public void Test_Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.Authenticate("admin", "wrongpassword");

            // Assert
            Assert.IsFalse(result, "Authentication succeeded with invalid credentials.");
        }

        [TestMethod]
        public void Test_Authenticate_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.Authenticate("nonexistent", "password");

            // Assert
            Assert.IsFalse(result, "Authentication succeeded with a non-existent user.");
        }

        [TestMethod]
        public void Test_GetRoleType_ValidUsername_ReturnsRoleType()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            string roleType = authRepository.GetRoleType("admin");

            // Assert
            Assert.AreEqual("admin", roleType, "Role type was not retrieved correctly.");
        }

        [TestMethod]
        public void Test_GetRoleType_InvalidUsername_ReturnsNull()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            string roleType = authRepository.GetRoleType("nonexistent");

            // Assert
            Assert.IsNull(roleType, "Role type should be null for a non-existent user.");
        }

        [TestMethod]
        public void Test_ValidateCredentials_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.ValidateCredentials("admin", "admin");

            // Assert
            Assert.IsTrue(result, "Validation failed with valid credentials.");
        }

        [TestMethod]
        public void Test_ValidateCredentials_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.ValidateCredentials("admin", "wrongpassword");

            // Assert
            Assert.IsFalse(result, "Validation succeeded with invalid credentials.");
        }

        [TestMethod]
        public void Test_ValidateCredentials_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            var authRepository = new AuthRepository(_config);

            // Act
            bool result = authRepository.ValidateCredentials("nonexistent", "password");

            // Assert
            Assert.IsFalse(result, "Validation succeeded with a non-existent user.");
        }

        private void SeedTestData()
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                // Insert test data into the Role table
                string insertQuery = @"
                    INSERT INTO Role (Username, Password, RoleType, Active) VALUES
                    ('admin', 'admin', 'admin', 1),
                    ('teacher', 'teacher', 'teacher', 1),
                    ('student', 'student', 'student', 1),
                    ('inactive_user', 'password', 'user', 0)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
