using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem
{

    public class Database
    {
        private string _dbFile = "database.db"; // Path to the SQLite database file

        // Method to create the database and the user table
        public void CreateDatabase()
        {
            // database location
            //Console.WriteLine("Database file location: " + Path.GetFullPath(_dbFile));

            if (!File.Exists(_dbFile))
            {
                // Creates the SQLite database file
                SQLiteConnection.CreateFile(_dbFile);

                using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
                {
                    connection.Open();

                    // Creates the user table
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Role (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            Password TEXT NOT NULL,
                            RoleType TEXT NOT NULL,
                            Active INTEGER DEFAULT 1
                        );

                        CREATE TABLE IF NOT EXISTS Classroom (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Name TEXT NOT NULL
                            );

                        CREATE TABLE IF NOT EXISTS Enrollment (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ClassroomId INTEGER NOT NULL,
                            RoleId INTEGER NOT NULL,
                            RoleType TEXT NOT NULL, -- 'Teacher' or 'Student'
                            FOREIGN KEY (ClassroomId) REFERENCES Classroom(Id),
                            FOREIGN KEY (RoleId) REFERENCES Role(Id)
                        );


                        CREATE TABLE IF NOT EXISTS Assessment (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ClassroomId INTEGER NOT NULL,
                            RoleId INTEGER NOT NULL, -- teacher
                            Description TEXT NOT NULL,
                            MaxScore FLOAT NOT NULL,
                            FOREIGN KEY (ClassroomId) REFERENCES Classroom(Id),
                            FOREIGN KEY (RoleId) REFERENCES Role(Id)
                        );

                        CREATE TABLE IF NOT EXISTS Submission (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            AssessmentId INTEGER NOT NULL,
                            RoleId INTEGER NOT NULL, -- student
                            Score FLOAT,
                            File TEXT NOT NULL,
                            FOREIGN KEY (AssessmentId) REFERENCES Assessment(Id),
                            FOREIGN KEY (RoleId) REFERENCES Role(Id)
                        );

                        CREATE TABLE Attendance (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            EnrollmentId INTEGER NOT NULL,
                            Date TEXT NOT NULL,
                            Present INTEGER NOT NULL,
                            FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(Id) ON DELETE CASCADE
                        );

                    ";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Inserts a default user (can be removed or replaced)
                    InsertDefaultUser(connection);
                }
            }
        }

        // Method to insert a default user (for initial testing)
        private void InsertDefaultUser(SQLiteConnection connection)
        {
            // admin role
            string insertQuery = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", "admin");
                command.Parameters.AddWithValue("@Password", "admin");
                command.Parameters.AddWithValue("@RoleType", "admin");
                command.ExecuteNonQuery();
            }

            // principal role
            insertQuery = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", "principal");
                command.Parameters.AddWithValue("@Password", "principal");
                command.Parameters.AddWithValue("@RoleType", "principal");
                command.ExecuteNonQuery();
            }

            // staff role
            insertQuery = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", "staff");
                command.Parameters.AddWithValue("@Password", "staff");
                command.Parameters.AddWithValue("@RoleType", "staff");
                command.ExecuteNonQuery();
            }

            // teacher role
            insertQuery = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", "teacher");
                command.Parameters.AddWithValue("@Password", "teacher");
                command.Parameters.AddWithValue("@RoleType", "teacher");
                command.ExecuteNonQuery();
            }

            // student role
            insertQuery = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", "student");
                command.Parameters.AddWithValue("@Password", "student");
                command.Parameters.AddWithValue("@RoleType", "student");
                command.ExecuteNonQuery();
            }
        }
        // Method to check if a user exists in the database
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
        public string GetUserRole(string _userName)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT RoleType FROM Role WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", _userName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // If the user is found
                        {
                            return reader.GetString(0);
                        }
                        else
                        {
                            return null; // Returns null if the user is not found
                        }
                    }
                }
            }
        }

        // Função que retorna os dados de um usuário específico com base no nome de usuário
        public RoleSchema GetRoleByUsername(string _userName)
        {

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
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
                            return new RoleSchema
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleType = reader.GetString(3),
                                Active = reader.GetInt32(4) == 1
                            };
                        }
                        else
                        {
                            return null; // Retorna null caso o usuário não seja encontrado
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
        public bool UpdateRolePassword(string username, string password)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // SQL para atualizar o tipo de role do usuário com base no username
                string sql = "UPDATE Role SET Password = @password WHERE Username = @username";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
        public bool AddRoleToClassroom(int classroomId, int roleId, string roleType)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Enrollment (ClassroomId, RoleId, RoleType) 
            VALUES (@ClassroomId, @RoleId, @RoleType)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@RoleType", roleType);
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        public bool InsertClassroom(string name)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Classroom (Name) VALUES (@Name)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }

        public ClassroomSchema GetClassroomByName(string classroomName)
        {
            var classroom = new List<(int Id, string Name)>();

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string query = @"
                    SELECT Id, Name
                    FROM Classroom 
                    WHERE Name = @Name";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", classroomName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ClassroomSchema
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public bool AddAssessment(int classroomId, int teacherId, string description, float maxScore)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Assessment (ClassroomId, RoleId, Description, MaxScore) 
            VALUES (@ClassroomId, @RoleId, @Description, @MaxScore)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@RoleId", teacherId);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@MaxScore", maxScore);

                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        public List<(int Id, string Description, float MaxScore)> GetAssessmentsByClassroom(int classroomId)
        {
            var assessments = new List<(int Id, string Description, float MaxScore)>();

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string query = @"
            SELECT Id, Description, MaxScore 
            FROM Assessment 
            WHERE ClassroomId = @ClassroomId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            float maxScore = reader.GetFloat(2);
                            assessments.Add((id, description, maxScore));
                        }
                    }
                }
            }

            return assessments;
        }

        public bool AddSubmission(int assessmentId, int studentId, float? score, string filePath)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Submission (AssessmentId, RoleId, Score, File) 
            VALUES (@AssessmentId, @RoleId, @Score, @File)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@AssessmentId", assessmentId);
                    command.Parameters.AddWithValue("@RoleId", studentId);
                    command.Parameters.AddWithValue("@Score", score.HasValue ? score.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@File", filePath);

                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        public List<(int Id, int StudentId, float? Score, string FilePath)> GetSubmissionsByAssessment(int assessmentId)
        {
            var submissions = new List<(int Id, int StudentId, float? Score, string FilePath)>();

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();

                string query = @"
            SELECT Id, RoleId, Score, File 
            FROM Submission 
            WHERE AssessmentId = @AssessmentId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssessmentId", assessmentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int studentId = reader.GetInt32(1);
                            float? score = reader.IsDBNull(2) ? null : reader.GetFloat(2);
                            string filePath = reader.GetString(3);

                            submissions.Add((id, studentId, score, filePath));
                        }
                    }
                }
            }

            return submissions;
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
        public List<(DateTime Date, bool Present)> GetAttendanceByEnrollment(int enrollmentId)
        {
            var attendanceRecords = new List<(DateTime, bool)>();

            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
                {
                    connection.Open();
                    string query = "SELECT Date, Present FROM Attendance WHERE EnrollmentId = @EnrollmentId";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var date = DateTime.Parse(reader["Date"].ToString());
                                var present = Convert.ToBoolean(reader["Present"]);
                                attendanceRecords.Add((date, present));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving attendance: {ex.Message}", ex);
            }

            return attendanceRecords;
        }

        public bool ActivateRole(string username, bool active)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // SQL para inserir um novo registro
                string sql = "UPDATE Role SET Active = @active WHERE Username = @username;";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
    }

}
