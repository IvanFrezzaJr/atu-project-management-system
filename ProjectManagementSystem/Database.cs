using System.Data.SQLite;

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
                            TeacherId INTEGER NOT NULL,
                            Description TEXT NOT NULL,
                            MaxScore FLOAT NOT NULL,
                            FOREIGN KEY (ClassroomId) REFERENCES Classroom(Id),
                            FOREIGN KEY (RoleId) REFERENCES Role(Id)
                        );

                        CREATE TABLE IF NOT EXISTS Submission (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            AssessmentId INTEGER NOT NULL,
                            StudentId INTEGER NOT NULL,
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
                                UserName = reader.GetString(1),
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

        public AssignmentSchema GetAssignmentByClassroomName(string classroomName, string description)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT a.Id, a.Name, a.Description, a.MaxScore
                FROM Assessment a
                JOIN Classroom e ON e.Id = a.ClassroomId
                WHERE e.Name = @ClassroomName
                AND a.Description = @Description";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomName", classroomName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AssignmentSchema
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ClassroomId = reader.GetInt32(reader.GetOrdinal("ClassroomId")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                MaxScore = reader.GetFloat(reader.GetOrdinal("MaxScore"))
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

    }

}
