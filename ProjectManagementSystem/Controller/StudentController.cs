using ProjectManagementSystem.Domain.Models;
using System.Collections.Generic;

namespace ProjectManagementSystem.Controller
{

    public class AddSubmissionMenuItem : MenuItem
    {

        public Student Student { get; set; }

        public AddSubmissionMenuItem(string name, Student student) : base(name)
        {
            Student = student;
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


                List<AssignmentSchema> assignmentResult = this.Student.GetAssignmantAvailable(clasroom);
                DisplayAssignmentsMenu(assignmentResult);
                string assignment = this.Input("Which assignment do you what submit (Enter description)?", "Enter a assignment description");
                if (assignment == "0")
                    break;

                if (assignment == null)
                    continue;


                string filePath = this.Input("Task filePath", "Enter a task filePath");
                if (filePath == "0")
                    break;

                if (filePath == null)
                    continue;


                bool status = this.Student.AddSubmission(this.Student.Id, clasroom, assignment, filePath);
                if (status)
                {
                    System.Console.WriteLine($"\nAssignment added with successful\n");
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

}
