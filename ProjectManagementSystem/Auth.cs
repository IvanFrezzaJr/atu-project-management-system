
using ProjectManagementSystem;

namespace ProjectManagementSystem
{



    public class Authentication
    {
        // Predefined username and password (these can be replaced with database logic in a real-world scenario)
        private string correctUsername = "user123";
        private string correctPassword = "pass123";

        // Method to get the username from the user
        public string GetUsername()
        {
            Console.Write("Enter username: ");
            return Console.ReadLine();
        }

        // Method to get the password from the user (hidden input)
        public string GetPassword()
        {
            Console.Write("Enter password: ");
            return ReadPassword();
        }

        // Method to authenticate the user based on the username and password
        public bool Authenticate(string username, string password)
        {
            return username == correctUsername && password == correctPassword;
        }

        // Method to read the password securely (without showing it on the screen)
        private string ReadPassword()
        {
            string password = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true); // intercept key press
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b"); // Remove the last character on screen
                    }
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Display asterisk for each typed character
                }
            }
            Console.WriteLine(); // Move to the next line after password input
            return password;
        }
    }

}