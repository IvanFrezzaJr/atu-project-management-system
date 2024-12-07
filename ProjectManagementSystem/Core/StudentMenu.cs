using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Core
{

    public class AddSubmissionMenuItem : MenuItem
    {

        public StudentController _studentController;

        public AddSubmissionMenuItem(string name, StudentController studentController) : base(name)
        {
            _studentController = studentController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _studentController.AddSubmission();

        }

        public static void DisplayAssignmentsMenu(List<Assessment> assignments)
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


    public class ShowScoreMenuItem : MenuItem
    {

        public StudentController _studentController;

        public ShowScoreMenuItem(string name, StudentController studentController) : base(name)
        {
            _studentController = studentController;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            base.Execute();

            _studentController.ShowScore();

        }
    }
}
