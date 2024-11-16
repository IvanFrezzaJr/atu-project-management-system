using ProjectManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ProjectManagementSystem.Domain.Services
{

    public class RoleService
    {
        private string _dbFile = "database.db"; // Path to the SQLite database file

        // CREATE: Add a new Role
        public void AddRole(Role role)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "INSERT INTO Role (Username, Password, RoleType) VALUES (@Username, @Password, @RoleType)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", role.UserName);
                    command.Parameters.AddWithValue("@Password", role.Password);
                    command.Parameters.AddWithValue("@RoleType", role.RoleType);
                    command.ExecuteNonQuery();
                }
            }
        }

        // READ: Get a Role by ID
        public Role? GetRoleById(int id)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT Id, Username, Password, RoleType FROM Role WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Role
                            {
                                Id = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleType = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // READ: Get all Roles
        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT Id, Username, Password, RoleType FROM Role";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                Id = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleType = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return roles;
        }

        // UPDATE: Update a Role
        public void UpdateRole(Role role)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "UPDATE Role SET Username = @Username, Password = @Password, RoleType = @RoleType WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", role.UserName);
                    command.Parameters.AddWithValue("@Password", role.Password);
                    command.Parameters.AddWithValue("@RoleType", role.RoleType);
                    command.Parameters.AddWithValue("@Id", role.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // DELETE: Delete a Role by ID
        public void DeleteRole(int id)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "DELETE FROM Role WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
