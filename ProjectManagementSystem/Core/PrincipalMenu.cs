using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Core
{
    // Class representing the third menu option

    public class AssignTeacherToClassroomMenuItem : MenuItem
    {

        public PrincipalController _principalController;

        public AssignTeacherToClassroomMenuItem(string name, PrincipalController principalController) : base(name)
        {
            _principalController = principalController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _principalController.AssignRoleToClassroom();

            
        }
    }


    public class ActiveRoleMenuItem : MenuItem
    {

        public PrincipalController _principalController;

        public ActiveRoleMenuItem(string name, PrincipalController principalController) : base(name)
        {
            _principalController = principalController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _principalController.ToggleRole();

        }
    }


    public class ShowGradeByClassroomMenuItem : MenuItem
    {

        public PrincipalController _principalController;

        public ShowGradeByClassroomMenuItem(string name, PrincipalController principalController) : base(name)
        {
            _principalController = principalController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _principalController.DisplayStudentSubmissions();

        }
    }

}