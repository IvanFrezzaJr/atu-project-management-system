using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Controller
{
    public class TeacherMarkStudentAttendanceMenuItem : MenuItem
    {

        public Teacher Teacher { get; set; }

        public TeacherMarkStudentAttendanceMenuItem(string name, Teacher teacher) : base(name)
        {
            Teacher = teacher;
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



                bool status = this.Teacher.MarkStudentAttendance(clasroom, role, "student");
                if (status)
                {
                    System.Console.WriteLine($"\nMarked attendance to '{role}' in '{clasroom}' classroom\n");
                    break;
                }
                continue;
            }

        }
    }

    public class AddAssignmentMenuItem : MenuItem
    {

        public Teacher Teacher { get; set; }

        public AddAssignmentMenuItem(string name, Teacher teacher) : base(name)
        {
            Teacher = teacher;
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


                string description = this.Input("Task description", "Enter a task description");
                if (description == "0")
                    break;

                if (description == null)
                    continue;

                float maxScore = 0;
                try
                {
                    maxScore = float.Parse(this.Input("Max Score", "Enter a max score"));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: The value is not a valid number");
                    continue;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Error: The value entered is too large or too small for a float.");
                    continue;
                }
                if (maxScore == 0)
                    break;


                bool status = this.Teacher.AddAssignment(clasroom, Teacher.UserName, description, maxScore);
                if (status)
                {
                    System.Console.WriteLine($"\nAssignment {description} added with successful\n");
                    break;
                }
                continue;
            }

        }
    }

}
