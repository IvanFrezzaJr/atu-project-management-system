using System.Data.SQLite;

namespace ProjectManagementSystem.Database
{
    public class ClassroomRepository
    {
        private readonly DatabaseConfig _config;

        public ClassroomRepository(DatabaseConfig config)
        {
            _config = config;
        }


        public ClassroomSchema GetClassroomByName(string classroomName)
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

        public bool InsertClassroom(string name)
        {
            using (var connection = _config.CreateConnection())
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

        public bool AddRoleToClassroom(int classroomId, int roleId, string roleType)
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

        public int? GetEnrollmentId(int classroomId, int roleId)
        {
            using (var connection = _config.CreateConnection())
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

        public AssignmentSchema GetAssignmentByName(string assignment)
        {
            using (var connection = _config.CreateConnection())
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
                            return new AssignmentSchema
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                MaxScore = (float)reader.GetDouble(2)
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

        public List<AssignmentSchema> GetAssignmentsByClassroom(string classroomName)
        {
            List<AssignmentSchema> assessments = new List<AssignmentSchema>();

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

                            assessments.Add(new AssignmentSchema
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


    }
}
