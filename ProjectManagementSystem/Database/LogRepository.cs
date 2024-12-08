using System.Data.SQLite;

namespace ProjectManagementSystem.Database
{
    public class LogRepository
    {
        private readonly DatabaseConfig _config;

        public LogRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public void InsertLog(Alert alert)
        {
            try
            {
                using (var connection = _config.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO Logs (Date, Role, Action, Message) VALUES (@Date, @Role, @Action, @Message)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", alert.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Role", alert.Role);
                        command.Parameters.AddWithValue("@Action", alert.Action);
                        command.Parameters.AddWithValue("@Message", alert.Message);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding attendance: {ex.Message}", ex);
            }
        }


        public List<Alert> GetAllLogs()
        {
            var logs = new List<Alert>();

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = "SELECT Date, Role, Action, Message FROM Logs";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var log = new Alert
                            {
                                CreatedAt = DateTime.Parse(reader["Date"].ToString()),
                                Role = reader["Role"].ToString(),
                                Action = reader["Action"].ToString(),
                                Message = reader["Message"].ToString()
                            };

                            logs.Add(log);
                        }
                    }
                }
            }

            return logs;
        }

    }
}