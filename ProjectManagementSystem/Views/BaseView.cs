using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views
{
    public class BaseView
    {
        const int MaxWidth = 40;


        public string GettextCenter(string text, int maxWidth)
        {
            int totalPadding = maxWidth - text.Length;
            int padLeft = totalPadding / 2; // Espaços à esquerda
            int padRight = totalPadding - padLeft; // Espaços à direita
            return text.PadLeft(padLeft + text.Length).PadRight(maxWidth);
        }

        public void DisplayTitle(string title)
        {
            Console.WriteLine("");
            Console.WriteLine(new string('=', MaxWidth));
            Console.WriteLine($"{GettextCenter(title, MaxWidth)}");
            Console.WriteLine(new string('=', MaxWidth));
        }

        //public string? Input(string text, string errorMessage)
        //{
        //    Console.Write($"{text}: ");
        //    string name = Console.ReadLine();
        //    if (name == "0")
        //    {
        //        return "0";
        //    }

        //    if (name == "" || name == null)
        //    {
        //        Console.WriteLine($"\n{errorMessage}\n");
        //        return null;
        //    }

        //    return name;
        //}

        public bool CheckExit(string input)
        {
            if (input == "0")
            {
                return true;
            }

            return false;
        }

        public string GetInput(string prompt)
        {
            Console.WriteLine(prompt);
            Console.Write("> ");
            string text =  Console.ReadLine();

            if (CheckExit(text)){
                return "<EXIT>";
            }

            return text;
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }

        public void DisplaySuccess(string successMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(successMessage);
            Console.ResetColor();
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

        public void DisplayAssessmentResult(List<Assessment> assessments)
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(
                $"|{GettextCenter("Id", 5)}" +
                $"|{GettextCenter("Classroom", 10)}" +
                $"|{GettextCenter("Assessment", 20)}" +
                $"|{GettextCenter("MaxScore", 10)}|");

            foreach (var assessment in assessments)
            {
                Console.WriteLine(
                    $"|{GettextCenter(assessment.Id.ToString(), 5)}" +
                    $"|{GettextCenter(assessment.Classroom, 10)}" +
                    $"|{GettextCenter(assessment.Description, 20)}" +
                    $"|{GettextCenter(assessment.MaxScore.ToString(), 10)}|");
            }
            string total = $"Total: {assessments.Count.ToString()}";
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"|{GettextCenter(total, 50)}|");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("");
        }

    }
}
