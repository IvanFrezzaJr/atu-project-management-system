using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



    }
}
