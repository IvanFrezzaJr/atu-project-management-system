using System.Data.SQLite;
using System.Xml.Linq;

namespace ProjectManagementSystem
{

    public class Database_
    {
        private string _dbFile = "database.db"; // Path to the SQLite database file

        public bool RoleExists(string username)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Role WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    long count = (long)command.ExecuteScalar();
                    return count > 0; // Returns true if the user exists
                }
            }
        }

        // Method to check if classroom exists
       
        // Method to retrieve a user by username

        public RoleSchema GetUserByName(string _userName)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = @"SELECT
                        Id,
                        Username,
                        Password,
                        RoleType,
                        Active
                    FROM Role 
                    WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", _userName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // If the user is found
                        {
                            return new RoleSchema
                            {
                                Id = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleType = reader.GetString(3),
                                Active = reader.IsDBNull(4) ? false : reader.GetInt32(4) == 1
                            };
                        }
                        else
                        {
                            return null; // Returns null if the user is not found
                        }
                    }
                }
            }
        }


     

        public bool InsertRole(string username, string password, string roletype)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // SQL para inserir um novo registro
                string sql = "INSERT INTO Role (Username, Password, RoleType) VALUES (@username, @password, @roletype)";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    // Adicionando parâmetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@roletype", roletype);

                    // Executando o comando
                    cmd.ExecuteNonQuery();
                }


            }

            return true;

        }
      
     
        
  

        public void InsertLog(Alert alert)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
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

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
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
