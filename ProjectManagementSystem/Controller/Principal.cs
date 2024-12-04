using ProjectManagementSystem.Core;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controller
{
    // Class representing the third menu option

    public class AssignTeacherToClassroomMenuItem : MenuItem
    {

        public Principal Principal { get; set; }

        public AssignTeacherToClassroomMenuItem(string name, Principal principal) : base(name)
        {
            Principal = principal;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string clasroom = this.Input("What is the Classroom name?", "Enter a name for a classroom");
                if (clasroom == "0")
                    break;

                if (clasroom == null)
                    continue;


                string role = this.Input("What is Teacher name?", "Enter a name for a classroom");
                if (role == "0")
                    break;

                if (role == null)
                    continue;



                bool status = this.Principal.AssignRoleToClassroom(clasroom, role, "teacher");
                if (status)
                {
                    System.Console.WriteLine($"\n'{role}' assigned succesful to '{clasroom}' class\n");
                    break;
                }
                continue;
            }

        }
    }


    public class ActiveRoleMenuItem : MenuItem
    {

        public Principal Principal { get; set; }

        public ActiveRoleMenuItem(string name, Principal principal) : base(name)
        {
            Principal = principal;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string name = this.Input("What is the User?", "Enter a user name");
                if (name == "0")
                    break;

                if (name == null)
                    continue;

                bool status = this.Principal.ToggleRole(name);
                string statusName = status ? "enabled" : "desabled";
                System.Console.WriteLine($"\n'{name}' has been {statusName} with successful\n");
                break;

            }

        }
    }


    public class ShowGradeByClassroomMenuItem : MenuItem
    {

        public Principal Principal { get; set; }

        public ShowGradeByClassroomMenuItem(string name, Principal principal) : base(name)
        {
            Principal = principal;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            while (true)
            {
                string classroom = this.Input("What is the Classroom name?", "Enter a classroom name");
                if (classroom == "0")
                    break;

                if (classroom == null)
                    continue;

                this.ShowClassroomGrades(classroom);
                break;
            }


        }

        public void ShowClassroomGrades(string classroomName)
        {
            var submissions = this.Principal.DisplayStudentSubmissions(classroomName);

            Console.WriteLine($"\nSubmissions for Classroom: {classroomName}\n");

            foreach (var submission in submissions)
            {
                Console.WriteLine($"Student: {submission.StudentName}");
                Console.WriteLine($"Assessment: {submission.AssessmentDescription}");
                Console.WriteLine($"Score: {submission.ScoreStatus} / {submission.MaxScore}");
                Console.WriteLine(new string('-', 30)); // Separator line
            }
        }
    }

}