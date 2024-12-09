using System.Data;
using System.Data.SQLite;
using System.Xml.Linq;
using ProjectManagementSystem.Models;
    
namespace ProjectManagementSystem.Database
{
    public class ClassroomRepository
    {
        private readonly DatabaseConfig _config;

        public ClassroomRepository(DatabaseConfig config)
        {
            _config = config;
        }


        public Classroom GetClassroomByName(string classroomName)
        {
            var classroom = new List<(int Id, string Name)>();

            using (var connection = _config.CreateConnection())
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
                            return new Classroom
                            (
                                reader.GetInt32(0), // Id
                                reader.GetString(1) // Name
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void InsertClassroom(Classroom classroom)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string insertQuery = "INSERT INTO Classroom (Name) VALUES (@Name)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", classroom.Name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool AddEnrollment(int classroomId, int roleId, string roleType)
        {
            using (var connection = _config.CreateConnection())
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

        public void AddAttendance(int enrollmentId, DateTime date, bool present)
        {
            try
            {
                using (var connection = _config.CreateConnection())
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


        public List<Attendance> GetAttendances(int enrollmentId)
        {
            try
            {
                var attendances = new List<Attendance>();

                using (var connection = _config.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT Id, EnrollmentId, Date, Present FROM Attendance WHERE EnrollmentId = @EnrollmentId";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                attendances.Add(new Attendance
                                {
                                    Id = reader.GetInt32(0), // Assuming 'Id' is the first column
                                    EnrollmentId = reader.GetInt32(1), // 'EnrollmentId' is the second column
                                    Date = reader.GetDateTime(2), // 'Date' is the third column
                                    Present = reader.GetInt32(3) == 1 // Assuming 'Present' stores 1 for true and 0 for false
                                });
                            }
                        }
                    }
                }

                return attendances;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving attendances: {ex.Message}", ex);
            }
        }


        public bool ClassroomExists(string name)
        {
            using (var connection = _config.CreateConnection())
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

        public Enrollment GetEnrollment(int classroomId, int roleId, string roleType)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                SELECT Id, ClassroomId, RoleId, RoleType
                FROM Enrollment 
                WHERE ClassroomId = @ClassroomId AND RoleId = @RoleId AND RoleType = @RoleType
                LIMIT 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@RoleType", roleType);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Enrollment
                            (
                                reader.GetInt32(0), // id
                                reader.GetInt32(1), // classroomId
                                reader.GetInt32(2), // roleId
                                reader.GetString(3) // roleType
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public bool EnrollmentExists(int classroomId, int roleId, string roleType)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                SELECT COUNT(*) 
                FROM Enrollment 
                WHERE ClassroomId = @ClassroomId 
                AND RoleId = @RoleId 
                AND RoleType = @RoleType";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassroomId", classroomId);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@RoleType", roleType);
                    long count = (long)command.ExecuteScalar();
                    return count > 0; // Returns true if the user exists
                }
            }
        }

        public bool AddAssessment(int classroomId, int teacherId, string description, float maxScore)
        {
            using (var connection = _config.CreateConnection())
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

        public Assessment GetAssignmentByName(string assignment)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                SELECT a.Id, a.Description, a.MaxScore
                FROM Assessment a
                WHERE a.Description = @Assessment";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Assessment", assignment);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Assessment
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                MaxScore = Convert.ToSingle(reader.GetDouble(2))
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


        public Submission GetSubmissionById(int submissionId)
        {
            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                SELECT
                  Id,
                  AssessmentId,
                  StudentId,
                  Score,
                  File
                FROM Submission
                WHERE Id = @SubmissionId";



                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubmissionId", submissionId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Submission
                            {
                                Id = reader.GetInt32(0),
                                AssessmentId = reader.GetInt32(1),
                                StudentId = reader.GetInt32(2),
                                Score = (float)(reader.GetValue(3) as double? ?? 0.0),
                                File = reader.GetString(4),  
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




        public bool UpdateScore(int assessmentId, int studentId, float score)
        {
            using (var connection = _config.CreateConnection())
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

        public List<Assessment> GetAssignmentsByClassroom(string classroomName)
        {
            List<Assessment> assessments = new List<Assessment>();

            using (var connection = _config.CreateConnection())
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

                            assessments.Add(new Assessment
                            {
                                Id = id,
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


        public List<Role> GetRolesByClassroom(string classroomName)
        {
            List<Role> roles = new List<Role>();

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                SELECT a.Id, a.Username, a.Password, a.Active, a.RoleType
                FROM Role a
                JOIN Enrollment e ON e.RoleId = a.Id
                JOIN Classroom c ON c.Id = e.ClassroomId
                WHERE c.Name = @Classroom";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Classroom", classroomName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string password = reader.GetString(2);
                            bool active = reader.GetInt32(3) != 0;
                            string _roleType = reader.GetString(4);

                            roles.Add(new Role(id, name, password, active, _roleType));

                        }
                    }
                }
            }
            return roles;
        }


        public List<Classroom> GetAllClassroom()
        {
            List<Classroom> classrooms = new List<Classroom>();

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT Id, Name
                    FROM Classroom;";

                using (var command = new SQLiteCommand(query, connection))
                {
 
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string classroom = reader.GetString(1);

                            classrooms.Add(new Classroom(
                                id,  // Id
                                classroom  // Classroom
                            ));
                        }
                
                    }
                }
            }

            return classrooms;
        }


        public List<Role> GetAllRoles(string roleType=null)
        {
            List<Role> roles = new List<Role>();

            using (var connection = _config.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT Id, Username, Password, Active, RoleType
                    FROM Role";

         
                string where = (roleType != null) ? $" WHERE RoleType = @RoleType;" : ";";
                query += where;

                using (var command = new SQLiteCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@RoleType", roleType);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string password = reader.GetString(2);
                            bool active = reader.GetInt32(3) != 0;
                            string _roleType = reader.GetString(4);

                            roles.Add(new Role(id,  name, password, active, _roleType));
                        }

                    }
                }
            }

            return roles;
        }


        public void AddSubmission(int assessmentId, int studentId, string filePath)
        {
            using (var connection = _config.CreateConnection())
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
        }


    }
}
