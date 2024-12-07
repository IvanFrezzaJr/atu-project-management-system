using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem
{
    public class Helpers
    {
        // Helper method to read the password securely (without showing characters)
        public static string ReadPassword()
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
    }
}
