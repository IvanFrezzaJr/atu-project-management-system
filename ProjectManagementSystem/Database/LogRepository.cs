using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Database
{
    public class LogRepository
    {
        private readonly DatabaseConfig _config;

        public LogRepository(DatabaseConfig config)
        {
            _config = config;
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