using System;
using System.Data.SQLite;
using System.IO;
using ProjectManagementSystem;

using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Services
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
                    RoleType TEXT NOT NULL
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

        // Method to retrieve a user by username
        public string GetUserRole(string username)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT RoleType FROM Role WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

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
   
        public bool InsertRole(string username, string password, string roletype)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // SQL para inserir um novo registro
                string sql = "INSERT INTO Role (Username, Password, RoleType) VALUES (@username, @password, 'principal')";

                // Criando o comando
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    // Adicionando parâmetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

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


    }

}
