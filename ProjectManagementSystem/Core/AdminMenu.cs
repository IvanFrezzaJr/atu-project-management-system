using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Core
{
    public class CreatePrincipalMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public CreatePrincipalMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            AdminController.CreatePrincipal();
        }
    }

    public class CreateStaffMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public CreateStaffMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            AdminController.CreateStaff();
        }
    }

    public class CreateTeacherMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public CreateTeacherMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            AdminController.CreateTeacher();
        }
    }

    public class CreateStudentMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public CreateStudentMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            AdminController.CreateStudent();

        }
    }

    public class ResetPasswordMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public ResetPasswordMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            AdminController.ResetPassword();

        }
    }

    public class ShowLogsMenuItem : MenuItem
    {

        public AdminController AdminController { get; set; }

        public ShowLogsMenuItem(string name, AdminController adminController) : base(name)
        {
            AdminController = adminController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();


            AdminController.PrintLogs();


        }
    }
}
