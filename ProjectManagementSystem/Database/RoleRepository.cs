using ProjectManagementSystem.Models;
using System.Data.SQLite;

namespace ProjectManagementSystem.Database
{
    public class RoleRepository
    {
        private readonly DatabaseConfig _config;

        public RoleRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public void AddRole(Role role)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = @"
                    INSERT INTO Role (Username, Password, RoleType, Active)
                    VALUES (@Username, @Password, @RoleType, @Active)";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", role.UserName);
                    cmd.Parameters.AddWithValue("@Password", role.Password); // Use hashing in production
                    cmd.Parameters.AddWithValue("@RoleType", role.RoleType);
                    cmd.Parameters.AddWithValue("@Active", role.Active ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Role> GetAllUsers()
        {
            var users = new List<Role>();
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT Id, Username, Password, RoleType, Active FROM Role";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Role(
                                reader.GetInt32(0), //id
                                reader.GetString(1), // userName
                                reader.GetString(2), //password
                                reader.GetInt32(3) == 1, //active
                                reader.GetString(4) //roleType
                            ));
                        }
                    }
                }
            }

            return users;
        }

        public void UpdateUser(Role role)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = @"
                    UPDATE Role 
                    SET Username = @Username, Password = @Password, RoleType = @RoleType, Active = @Active 
                    WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", role.Id);
                    cmd.Parameters.AddWithValue("@Username", role.UserName);
                    cmd.Parameters.AddWithValue("@Password", role.Password); // Use hashing in production
                    cmd.Parameters.AddWithValue("@RoleType", role.RoleType);
                    cmd.Parameters.AddWithValue("@Active", role.Active ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRole(int roleId)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();
                string sql = "DELETE FROM Role WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", roleId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Function that returns data for a specific user based on username
        public Role GetRoleByUserName(string _userName)
        {

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = "SELECT Id, Username, Password, RoleType, Active FROM Role WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", _userName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Se o usuário for encontrado
                        {
                            return new Role(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(4) == 1,
                                reader.GetString(3)
                            );
                        }
                        else
                        {
                            return null; // Retorna null caso o usuário não seja encontrado
                        }
                    }
                }
            }
        }

        public bool RoleExists(string username)
        {
            using (var connection = _config.CreateConnection())
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

        public bool UpdateRolePassword(string username, string password)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                // SQL para atualizar o tipo de role do usuário com base no username
                string sql = "UPDATE Role SET Password = @password WHERE Username = @username";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    // Adicionando parâmetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Executando o comando
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Se nenhuma linha for afetada, significa que o usuário não foi encontrado
                    if (rowsAffected == 0)
                    {
                        return false; // Usuário não encontrado
                    }
                }
            }

            return true; // A atualização foi bem-sucedida
        }

        // Function that returns data for a specific user based on username

        public bool ActivateRole(string username, bool active)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                // SQL para inserir um novo registro
                string sql = "UPDATE Role SET Active = @active WHERE Username = @username;";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    // Adicionando parâmetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@active", active ? 1 : 0);

                    // Executando o comando
                    cmd.ExecuteNonQuery();
                }


            }

            return true;

        }

        public List<dynamic> GetStudentSubmissions(string classroomName)
        {
            string query = @"
                SELECT 
                    c.Name AS ClassroomName,
                    a.Description AS AssessmentDescription,
                    CASE
                        WHEN s.File IS NULL THEN 'Pendent' 
                        ELSE (
                            CASE 
                                WHEN s.Score IS NULL THEN '-'
                                ELSE CAST(s.Score AS TEXT)
                            END
                        )
                    END AS ScoreStatus,
                    r.Username AS StudentName,
                    s.File,
                    s.Score,
                    a.MaxScore
                 FROM 
                      Classroom c
 
                 LEFT JOIN 
                     Assessment a ON a.ClassroomId = c.Id
                 LEFT JOIN 
                     Submission s ON s.AssessmentId = a.Id 
                 LEFT JOIN 
                     Role r ON s.StudentId = r.Id AND r.RoleType = 'student'
                 WHERE 
                     c.Name = @ClassroomName;";

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomName", classroomName);

                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<dynamic>();

                        while (reader.Read())
                        {
                            result.Add(new
                            {
                                ClassroomName = reader["ClassroomName"].ToString(),
                                AssessmentDescription = reader["AssessmentDescription"].ToString(),
                                ScoreStatus = reader["ScoreStatus"].ToString(),
                                StudentName = reader["StudentName"].ToString(),
                                File = reader["File"].ToString(),
                                Score = reader["Score"] == DBNull.Value ? 0 : float.Parse(reader["Score"].ToString()),
                                MaxScore = reader["MaxScore"] == DBNull.Value ? 0 : float.Parse(reader["MaxScore"].ToString())
                            });
                        }

                        return result;
                    }
                }
            }
        }

        public List<dynamic> GetStudentSubmissions(string classroomName, string studentName)
        {
            string query = @"
                SELECT 
                     c.Name AS ClassroomName,
                     a.Description AS AssessmentDescription,
                     CASE
                         WHEN s.File IS NULL THEN 'Pendent'
                         ELSE (
                             CASE 
                                 WHEN s.Score IS NULL THEN '-'
                                 ELSE CAST(s.Score AS TEXT)
                             END
                         )
                     END AS ScoreStatus,
                     a.MaxScore AS MaxScore
                 FROM 
                      Classroom c
 
                 LEFT JOIN 
                     Assessment a ON a.ClassroomId = c.Id
                 LEFT JOIN 
                     Submission s ON s.AssessmentId = a.Id 
                 LEFT JOIN 
                     Role r ON s.StudentId = r.Id AND r.RoleType = 'student'
                 WHERE 
                     c.Name = @ClassroomName
                     AND r.UserName = @StudentName;";

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomName", classroomName);
                    command.Parameters.AddWithValue("@StudentName", studentName);


                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<dynamic>();

                        while (reader.Read())
                        {
                            result.Add(new
                            {
                                ClassroomName = reader["ClassroomName"].ToString(),
                                AssessmentDescription = reader["AssessmentDescription"].ToString(),
                                ScoreStatus = reader["ScoreStatus"].ToString(),
                                MaxScore = reader["MaxScore"] == DBNull.Value ? 0 : float.Parse(reader["MaxScore"].ToString())
                            });
                        }

                        return result;
                    }
                }
            }
        }


    }
}
