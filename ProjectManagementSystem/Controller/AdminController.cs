using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Controller
{
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
            base.Execute();

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

                bool status = this.Admin.CreateRole(username, password, "principal");

                if (status)
                {
                    System.Console.WriteLine($"\nPrincipal '{username}' successful\n");
                    break;
                }
                continue;
            }
        }
    }

    public class CreateStaffMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public CreateStaffMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                Console.Write("Staff username: ");
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

                Console.Write("Staff password: ");
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

                bool status = this.Admin.CreateRole(username, password, "staff");

                if (status)
                {
                    System.Console.WriteLine($"\nStaff '{username}' successful\n");
                    break;
                }


                continue;
            }

        }
    }

    public class CreateTeacherMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public CreateTeacherMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                Console.Write("Teacher username: ");
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

                Console.Write("Teacher password: ");
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

                bool status = this.Admin.CreateRole(username, password, "teacher");

                if (status)
                {
                    System.Console.WriteLine($"\nTeacher '{username}' successful\n");
                    break;
                }


                continue;
            }

        }
    }

    public class CreateStudentMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public CreateStudentMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                Console.Write("Student username: ");
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

                Console.Write("Student password: ");
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

                bool status = this.Admin.CreateRole(username, password, "student");

                if (status)
                {
                    System.Console.WriteLine($"\nStudent '{username}' successful\n");
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
            base.Execute();

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
                    System.Console.WriteLine($"\nUpdated password for '{username}' user\n");
                    break;
                }


                continue;
            }

        }
    }

    public class ShowLogsMenuItem : MenuItem
    {

        public Admin Admin { get; set; }

        public ShowLogsMenuItem(string name, Admin admin) : base(name)
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();


            var logs = this.Admin.PrintLogs();

            Console.WriteLine($"\nSystem logs:\n");

            foreach (var log in logs)
            {
                System.Console.WriteLine($"[{log.CreatedAt}] - '{log.Role}'.{log.Action}: {log.Message}");
            }

        }
    }
}