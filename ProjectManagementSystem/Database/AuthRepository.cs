using System.Data.SQLite;

namespace ProjectManagementSystem.Database
{
    public class AuthRepository
    {
        private readonly DatabaseConfig _config;

        public AuthRepository(DatabaseConfig config)
        {
            _config = config;
        }


        public bool Authenticate(string username, string password)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT Password FROM Role WHERE Username = @Username AND Active = 1";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString(0);
                            return storedPassword == password; // For simplicity. Use hashing in production.
                        }
                    }
                }
            }

            return false;
        }

        public string GetRoleType(string username)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT RoleType FROM Role WHERE Username = @Username AND Active = 1";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }

            return null;
        }

        // Method to verify user credentials in the database
        public bool ValidateCredentials(string username, string password)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Role WHERE Username = @Username AND Password = @Password";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

    }
}



