using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementSystem.Models;

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

    }
}
