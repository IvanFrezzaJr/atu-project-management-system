using ProjectManagementSystem.Models;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class StaffView : BaseView
    {
        public void ShowSubmissionsResult(List<dynamic> submissions)
        {
            foreach (var submission in submissions)
            {
                Console.WriteLine($"Student: {submission.StudentName}");
                Console.WriteLine($"Assessment: {submission.AssessmentDescription}");
                Console.WriteLine($"Score: {submission.ScoreStatus} / {submission.MaxScore}");
                Console.WriteLine(new string('-', 30)); // Separator line
            }
        }

        public void DisplayClassroomInfo(Classroom classroom)
        {
            Console.WriteLine($"ClassName: {classroom.Name}");
        }

        public void DisplayEnrollmentInfo(Role role, Classroom classroom)
        {
            Console.WriteLine($"Classroom: {classroom.Name}");
            Console.WriteLine($"{role.RoleType.ToUpper()}: {role.UserName}");
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


        public void DisplayAttendanceInfo(List<Attendance> attendances, Classroom classroom, Role role)
        {
            Console.WriteLine(new string('=', 22));
            Console.WriteLine($"Student: {role.UserName}");
            Console.WriteLine($"ClassName: {classroom.Name}");
            Console.WriteLine(new string('=', 22));
            Console.WriteLine($"|{GettextCenter("Date", 15)}|{GettextCenter("Present", 5)}|");
            foreach (var attendance in attendances)
            {
                Console.WriteLine($"|{GettextCenter(attendance.Date.ToString(), 15)}|{GettextCenter(attendance.Present.ToString(), 5)}|");
            }
            string total = $"Total: {attendances.Count.ToString()}";
            Console.WriteLine(new string('=', 22));
            Console.WriteLine($"|{GettextCenter(total, 21)}|");
            Console.WriteLine(new string('=', 22));
            Console.WriteLine("");
        }



        

    }
}

