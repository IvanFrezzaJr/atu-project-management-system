using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Controllers;


namespace ProjectManagementSystem.Core
{
    public class TeacherMarkStudentAttendanceMenuItem : MenuItem
    {

        public TeacherController _teacherController;

        public TeacherMarkStudentAttendanceMenuItem(string name, TeacherController teacherController) : base(name)
        {
            _teacherController = teacherController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _teacherController.MarkStudentAttendance();

        }
    }

    public class AddAssignmentMenuItem : MenuItem
    {

        public TeacherController _teacherController;

        public AddAssignmentMenuItem(string name, TeacherController teacherController) : base(name)
        {
            _teacherController = teacherController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();


            _teacherController.AddAssignment();

        }
    }
   
    public class SetStudentGradeMenuItem : MenuItem
    {

        public TeacherController _teacherController;

        public SetStudentGradeMenuItem(string name, TeacherController teacherController) : base(name)
        {
            _teacherController = teacherController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _teacherController.SetStudentGrade();

        }

    }

    public class ShowClassroomGradeMenuItem : MenuItem
    {

        public TeacherController _teacherController;

        public ShowClassroomGradeMenuItem(string name, TeacherController teacherController) : base(name)
        {
            _teacherController = teacherController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _teacherController.DisplayStudentSubmissions();

            

        }

    }
}


