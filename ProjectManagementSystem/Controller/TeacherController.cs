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

    public class SetStudentGradeMenuItem : MenuItem
    {

        public Teacher Teacher { get; set; }

        public SetStudentGradeMenuItem(string name, Teacher teacher) : base(name)
        {
            Teacher = teacher;
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


                List<AssignmentSchema> assignmentResult = this.Teacher.GetAssignmantAvailable(classroom);
                DisplayAssignmentsMenu(assignmentResult);
                string assignment = this.Input("What is Assignment description?", "Enter a Assignment name");
                if (assignment == "0")
                    break;

                if (assignment == null)
                    continue;


                string role = this.Input("What is Student name?", "Enter a Student name");
                if (role == "0")
                    break;

                if (role == null)
                    continue;


                float score = 0;
                try
                {
                    score = float.Parse(this.Input("Max Score", "Enter a max score"));
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
                if (score == 0)
                    break;

                bool status = this.Teacher.SetStudentGrade(classroom, assignment, role, "student", score);
                if (status)
                {
                    System.Console.WriteLine($"\nSet score to '{role}' in '{classroom}' classroom\n");
                    break;
                }
                continue;
            }

        }

        public static void DisplayAssignmentsMenu(List<AssignmentSchema> assignments)
        {
            Console.WriteLine("Assignments Menu:");
            Console.WriteLine("=================");

            if (assignments.Count == 0)
            {
                Console.WriteLine("- No assignments found.");
            }
            else
            {
                foreach (var assignment in assignments)
                {
                    Console.WriteLine($"- ID: {assignment.Id}");
                    Console.WriteLine($"  Classroom: {assignment.Classroom}");
                    Console.WriteLine($"  Description: {assignment.Description}");
                    Console.WriteLine($"  Max Score: {assignment.MaxScore}");
                    Console.WriteLine("-------------------------------\n");
                }
            }
        }
    }


    public class ShowClassroomGradeMenuItem : MenuItem
    {

        public Teacher Teacher { get; set; }

        public ShowClassroomGradeMenuItem(string name, Teacher teacher) : base(name)
        {
            Teacher = teacher;
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
            var submissions = this.Teacher.DisplayStudentSubmissions(classroomName);

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
