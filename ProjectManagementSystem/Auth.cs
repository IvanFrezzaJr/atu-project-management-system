using System;
using System.Data.SQLite;
using ProjectManagementSystem;

namespace ProjectManagementSystem
{

    public class Authentication
    {
        private string dbFile = "database.db"; // Path to the SQLite database file
        private int? currentUser = null;

        // Constructor
        public Authentication()
        {
            // No parameters needed, as the database is managed by the class itself
        }

        // Method to authenticate the user in the database
        public bool Authenticate(string username, string password)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Role WHERE Username = @Username AND Password = @Password";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    long count = (long)command.ExecuteScalar();
                    return count > 0; // Returns true if the user is found
                }
            }
        }



        // Method to ask the user for the username
        public string GetUsername()
        {
            Console.Write("Enter username: ");
            return Console.ReadLine();
        }

        // Method to ask the user for the password
        public string GetPassword()
        {
            Console.Write("Enter password: ");
            return ReadPassword();
        }

        // Method to read the password in a hidden way (not displaying the typed characters)
        private string ReadPassword()
        {
            string password = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true); // Intercepts the key pressed
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b"); // Removes the last character from the screen
                    }
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Displays an asterisk for each typed character
                }
            }
            Console.WriteLine(); // New line after entering the password
            return password;
        }
    }
}
