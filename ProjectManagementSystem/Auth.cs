using System.Data.SQLite;

namespace ProjectManagementSystem
{
    // Class responsible for handling user authentication
    public class Authentication
    {
        // Path to the SQLite database file
        private string dbFile = "database.db";
        private int? currentUser = null; // Stores the current user ID if authenticated

        // Constructor to initialize the Authentication class
        public Authentication()
        {
            // No parameters needed, as the database is managed by the class itself
        }

        // Method to authenticate the user in the database
        public bool Authenticate(string username, string password)
        {
            // Creates a new connection to the SQLite database
            using (var connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open(); // Opens the connection to the database
                string query = "SELECT COUNT(1) FROM Role WHERE Username = @Username AND Password = @Password"; // SQL query to check user credentials
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Adds the parameters to the SQL command to avoid SQL injection
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    long count = (long)command.ExecuteScalar(); // Executes the query and checks if the user exists
                    return count > 0; // Returns true if the user is found in the database
                }
            }
        }

        // Method to prompt the user to enter a username
        public string GetUsername()
        {
            Console.Write("Enter username: ");
            return Console.ReadLine(); // Reads the entered username from the console
        }

        // Method to prompt the user to enter a password
        public string GetPassword()
        {
            Console.Write("Enter password: ");
            return ReadPassword(); // Calls ReadPassword to securely get the password
        }

        // Method to read the password without displaying the typed characters on the screen
        private string ReadPassword()
        {
            string password = ""; // Initializes an empty string to store the password
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true); // Intercepts the key pressed without displaying it
                if (key.Key == ConsoleKey.Enter) // If Enter is pressed, break the loop
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace) // If Backspace is pressed, delete the last character
                {
                    if (password.Length > 0) // Prevents removing characters when password is empty
                    {
                        password = password.Substring(0, password.Length - 1); // Removes the last character
                        Console.Write("\b \b"); // Moves the cursor back and clears the character on screen
                    }
                }
                else // Adds the typed character to the password string
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Displays an asterisk for each character typed
                }
            }
            Console.WriteLine(); // Adds a newline after the password is entered
            return password; // Returns the entered password
        }
    }
}
