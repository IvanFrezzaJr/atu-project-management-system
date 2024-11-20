using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Controller
{
    // Class representing the third menu option
    public class CreateClassroomMenuItem : MenuItem
    {

        public Staff Staff { get; set; }

        public CreateClassroomMenuItem(string name, Staff staff) : base(name)
        {
            Staff = staff;
        }

        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string name = this.Input("Classroom name", "Enter a name for a classroom");

                if (name == "0")
                    break;

                if (name == null)
                    continue;


                bool status = this.Staff.CreateClassroom(name);
                if (status)
                {
                    System.Console.WriteLine($"\nClassroom '{name}' created successful\n");
                    break;
                }
                continue;
            }

        }
    }

    public class AssignStudentToClassroomMenuItem : MenuItem
    {

        public Staff Staff { get; set; }

        public AssignStudentToClassroomMenuItem(string name, Staff staff) : base(name)
        {
            Staff = staff;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string clasroom = this.Input("What is the Classroom name?", "Enter a classroom name");
                if (clasroom == "0")
                    break;

                if (clasroom == null)
                    continue;


                string role = this.Input("What is Student name?", "Enter a role name");
                if (role == "0")
                    break;

                if (role == null)
                    continue;



                bool status = this.Staff.AssignRoleToClassroom(clasroom, role, "student");
                if (status)
                {
                    System.Console.WriteLine($"\n'{role}' assigned succesful to '{clasroom}' classroom\n");
                    break;
                }
                continue;
            }

        }
    }

    public class StaffMarkStudentAttendanceMenuItem : MenuItem
    {

        public Staff Staff { get; set; }

        public StaffMarkStudentAttendanceMenuItem(string name, Staff staff) : base(name)
        {
            Staff = staff;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string clasroom = this.Input("What is the Classroom name?", "Enter a classroom name");
                if (clasroom == "0")
                    break;

                if (clasroom == null)
                    continue;


                string role = this.Input("What is Student name?", "Enter a role name");
                if (role == "0")
                    break;

                if (role == null)
                    continue;



                bool status = this.Staff.MarkStudentAttendance(clasroom, role, "student");
                if (status)
                {
                    System.Console.WriteLine($"\nMarked attendance to '{role}' in '{clasroom}' classroom\n");
                    break;
                }
                continue;
            }

        }
    }

}