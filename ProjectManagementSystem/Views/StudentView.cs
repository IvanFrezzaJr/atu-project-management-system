using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class StudentView : BaseView
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


        public void ShowAssignmentsMenu(List<Assessment> assignments)
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
}

