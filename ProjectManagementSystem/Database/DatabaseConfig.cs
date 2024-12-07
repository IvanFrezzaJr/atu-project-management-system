using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Database
{
    public class DatabaseConfig
    {
        private const string _dbFile = "database.db";

        private string ConnectionString = $"Data Source={_dbFile};Version=3;"; // Path to the SQLite database file

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        // Method to create the database and the user table
        public void CreateDatabase()
        {
            // database location
            //Console.WriteLine("Database file location: " + Path.GetFullPath(_dbFile));

            if (!File.Exists(_dbFile))
            {
                // Creates the SQLite database file
                SQLiteConnection.CreateFile(_dbFile);

                using (var connection = this.CreateConnection())
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
                            FOREIGN KEY (TeacherId) REFERENCES Role(Id)
                        );

                        CREATE TABLE IF NOT EXISTS Submission (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            AssessmentId INTEGER NOT NULL,
                            StudentId INTEGER NOT NULL,
                            Score FLOAT,
                            File TEXT NOT NULL,
                            FOREIGN KEY (AssessmentId) REFERENCES Assessment(Id),
                            FOREIGN KEY (StudentId) REFERENCES Role(Id)
                        );

                        CREATE TABLE Attendance (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            EnrollmentId INTEGER NOT NULL,
                            Date TEXT NOT NULL,
                            Present INTEGER NOT NULL,
                            FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(Id) ON DELETE CASCADE
                        );

                        CREATE TABLE Logs (
                            Date TEXT NOT NULL,
                            Role TEXT NOT NULL,
                            Action TEXT NOT NULL,
                            Message TEXT NOT NULL
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
    }
}


