using ProjectManagementSystem.Domain.Models;
using System.Data.SQLite;
using System;

namespace ProjectManagementSystem
{
    // Class representing the first menu option
    public class Option1MenuItem : MenuItem
    {
        public Option1MenuItem(string name) : base(name) { }

        // Executes the action for Option 1
        public override void Execute()
        {
            System.Console.WriteLine("Option1MenuItem");
        }
    }

    // Class representing the second menu option
    public class Option2MenuItem : MenuItem
    {
        public Option2MenuItem(string name) : base(name) { }

        // Executes the action for Option 2
        public override void Execute()
        {
            System.Console.WriteLine("Option2MenuItem");
        }
    }

    // Class representing the third menu option
    public class Option3MenuItem : MenuItem
    {
        public Option3MenuItem(string name) : base(name) { }

        // Executes the action for Option 3
        public override void Execute()
        {
            System.Console.WriteLine("Option3MenuItem");
        }
    }


    // Class representing the third menu option
    public class CreatePrincipalMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public CreatePrincipalMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            System.Console.WriteLine($"--- {this.Name} ---");
            System.Console.WriteLine($"quit: 0 + Enter\n");
            while (true)
            {
                Console.Write("Principal username: ");
                string username = Console.ReadLine();
                if (username == "0")
                {
                    break;
                }

                if (username == "" || username == null)
                {
                    System.Console.WriteLine("\nEnter a value for the user\n");
                    continue;
                }

                Console.Write("Principal password: ");
                string password = Console.ReadLine();
                if (password == "0")
                {
                    break;
                }

                if (password == "" || password == null)
                {
                    System.Console.WriteLine("\nEnter a value for the password\n");
                    continue;
                }

                bool status = this.Admin.CreatePrincipal(username, password);

                if (status)
                {
                    System.Console.WriteLine("\nPrincipal '{username}' successful\n");
                    break;
                }


                continue;
            }

        }
    }


    public class ResetPasswordMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public ResetPasswordMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            System.Console.WriteLine($"--- {this.Name} ---");
            System.Console.WriteLine($"quit: 0 + Enter\n");
            while (true)
            {
                Console.Write("Role username: ");
                string username = Console.ReadLine();
                if (username == "0")
                {
                    break;
                }

                if (username == "" || username == null)
                {
                    System.Console.WriteLine("\nEnter a value for the user\n");
                    continue;
                }

                Console.Write("Role new password: ");
                string password = Console.ReadLine();
                if (password == "0")
                {
                    break;
                }

                if (password == "" || password == null)
                {
                    System.Console.WriteLine("\nEnter a value for the password\n");
                    continue;
                }

                bool status = this.Admin.ResetPassword(username, password);

                if (status)
                {
                    System.Console.WriteLine($"\nRole '{username}' successful\n");
                    break;
                }


                continue;
            }

        }
    }

}