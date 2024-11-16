using ProjectManagementSystem.Domain.Models;
using System.Data.SQLite;

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
            System.Console.WriteLine("Init - CreatePrincipalMenuItem");

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            this.Admin.CreatePrincipal();

            System.Console.WriteLine("End - CreatePrincipalMenuItem");

        }
    }

}