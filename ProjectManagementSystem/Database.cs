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
        public bool ClassroomExists(string name)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Classroom WHERE Name = @Name";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    long count = (long)command.ExecuteScalar();
                    return count > 0; // Returns true if the user exists
                }
            }
        }
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
      
     
        
        public bool AddAssessment(int classroomId, int teacherId, string description, float maxScore)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Assessment (ClassroomId, TeacherId, Description, MaxScore) 
            VALUES (@ClassroomId, @TeacherId, @Description, @MaxScore)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@MaxScore", maxScore);

                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
  
        public bool AddSubmission(int assessmentId, int studentId, string filePath)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Submission (AssessmentId, StudentId, Score, File) 
            VALUES (@AssessmentId, @StudentId, @Score, @File)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@AssessmentId", assessmentId);
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@Score", null);
                    command.Parameters.AddWithValue("@File", filePath);

                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
   
        public void AddAttendance(int enrollmentId, DateTime date, bool present)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
                {
                    connection.Open();
                    string query = "INSERT INTO Attendance (EnrollmentId, Date, Present) VALUES (@EnrollmentId, @Date, @Present)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EnrollmentId", enrollmentId);
                        command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Present", present ? 1 : 0);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding attendance: {ex.Message}", ex);
            }
        }

     
        public int? GetEnrollmentId(int classroomId, int roleId)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT Id 
                FROM Enrollment 
                WHERE ClassroomId = @ClassroomId AND RoleId = @RoleId
                LIMIT 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@RoleId", roleId);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        return id;
                    }

                    return null;
                }
            }
        }

        public AssignmentSchema GetAssignmentByName(string assignment)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT a.Id, a.Description, a.MaxScore
                FROM Assessment a
                WHERE a.Description = @Assignment";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Assignment", assignment);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AssignmentSchema{
                                Id= reader.GetInt32(0), 
                                Description= reader.GetString(1),
                                MaxScore= (float)reader.GetDouble(2)
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

        public List<AssignmentSchema> GetAssignmentsByClassroom(string classroomName)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            List<AssignmentSchema> assessments = new List<AssignmentSchema>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT a.Id, c.Name, a.Description, a.MaxScore
                FROM Assessment a
                JOIN Classroom c ON c.Id = a.ClassroomId
                WHERE c.Name = @Classroom";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Classroom", classroomName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string classroom = reader.GetString(1);
                            string description = reader.GetString(2);
                            float maxScore = (float)reader.GetDouble(3);

                            assessments.Add(new AssignmentSchema
                            {
                                Id =id,
                                Classroom = classroom,
                                Description = description,
                                MaxScore = maxScore
                            });

                        }
                    }
                }
            }
            return assessments;
        }

        public bool UpdateScore(int assessmentId, int studentId, float score)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string updateQuery = @"
        UPDATE Submission
        SET Score = @Score
        WHERE AssessmentId = @AssessmentId AND StudentId = @StudentId";

                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Score", score);
                    command.Parameters.AddWithValue("@AssessmentId", assessmentId);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Retorna true se ao menos uma linha foi afetada
                    return rowsAffected > 0;
                }
            }
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
