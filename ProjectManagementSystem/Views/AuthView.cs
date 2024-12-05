using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class UserInterface
    {
        // Method to prompt the user for a username
        public string GetUsername()
        {
            Console.Write("Enter username: ");
            return Console.ReadLine();
        }

        // Method to prompt the user for a password (securely)
        public string GetPassword()
        {
            Console.Write("Enter password: ");
            return ReadPassword();
        }

        // Helper method to read the password securely (without showing characters)
        private string ReadPassword()
        {
            string password = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password;
        }

        // Method to display authentication result
        public void ShowAuthenticationResult(bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                Console.WriteLine("Authentication successful!");
            }
            else
            {
                Console.WriteLine("Authentication failed! Invalid username or password.");
            }
        }

        public void ShowAuthenticationAttempts(int attempts, int maxAttempts = 3)
        {
            if (attempts > maxAttempts)
            {
                throw new Exception("'maxAttempts' shouldn't be greater than 'attempts'");
            }

            if (attempts == maxAttempts)
            {
                Console.WriteLine("You have reached the maximum number of attempts. The program will be terminated.");
            }

            Console.WriteLine($"You have {maxAttempts - attempts} more attempts.");


        }
    }
}

