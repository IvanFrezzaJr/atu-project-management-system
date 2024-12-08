using ProjectManagementSystem.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class PrincipalView : BaseView
    {
        public void ShowSubmissionsResult(List<dynamic> submissions)
        {
            Console.WriteLine(new string('=', 103));
            Console.WriteLine(
                $"|{GettextCenter("Classroom", 10)}" +
                $"|{GettextCenter("Assessment Description", 30)}" +
                $"|{GettextCenter("Student", 10)}" +
                $"|{GettextCenter("File", 20)}" +
                $"|{GettextCenter("Status", 10)}" +
                $"|{GettextCenter("Score", 5)}" +
                $"|{GettextCenter("Max Score", 10)}|");


            foreach (var submission in submissions)
            {
                float grade = submission.Score / submission.MaxScore;
                Console.WriteLine(
                    $"|{GettextCenter(submission.ClassroomName, 10)}" +
                    $"|{GettextCenter(submission.AssessmentDescription, 30)}" +
                    $"|{GettextCenter(submission.StudentName, 10)}" +
                    $"|{GettextCenter(submission.File, 20)}" +
                    $"|{GettextCenter(submission.ScoreStatus, 10)}" +
                    $"|{GettextCenter(grade.ToString(), 5)}" +
                    $"|{GettextCenter(submission.MaxScore.ToString(), 10)}|");
            }
            string total = $"Total: {submissions.Count.ToString()}";
            Console.WriteLine(new string('=', 103));
            Console.WriteLine($"|{GettextCenter(total, 101)}|");
            Console.WriteLine(new string('=', 103));
            Console.WriteLine("");
        }

        public void DisplayClassrooms(List<Classroom> classrooms)
        {
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{GettextCenter("Id", 5)}|{GettextCenter("Classroom", 20)}|");
            foreach (var classroom in classrooms)
            {
                Console.WriteLine($"|{GettextCenter(classroom.Id.ToString(), 5)}|{GettextCenter(classroom.Name, 20)}|");
            }
            string total = $"Total: {classrooms.Count.ToString()}";
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{GettextCenter(total, 26)}|");
            Console.WriteLine(new string('=', 28));
            Console.WriteLine("");
        }


        public void DisplayRoles(List<Role> roles)
        {
            Console.WriteLine(new string('=', 39));
            Console.WriteLine($"|{GettextCenter("Id", 5)}|{GettextCenter("Name", 20)}|{GettextCenter("Role", 10)}|");
            foreach (var role in roles)
            {
                Console.WriteLine($"|{GettextCenter(role.Id.ToString(), 5)}|{GettextCenter(role.UserName, 20)}|{GettextCenter(role.RoleType, 10)}|");
            }
            string total = $"Total: {roles.Count.ToString()}";
            Console.WriteLine(new string('=', 39));
            Console.WriteLine($"|{GettextCenter(total, 37)}|");
            Console.WriteLine(new string('=', 39));
            Console.WriteLine("");
        }

        public void DisplayEnrollmentInfo(Role role, Classroom classroom)
        {
            Console.WriteLine($"Classroom: {classroom.Name}");
            Console.WriteLine($"{role.RoleType.ToUpper()}: {role.UserName}");
        }


    }
}

