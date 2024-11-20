using ProjectManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


                string role = this.Input("What is Student name?", "Enter a role name");
                if (role == "0")
                    break;

                if (role == null)
                    continue;


                string description = this.Input("Task description", "Enter a task description");
                if (description == "0")
                    break;

                if (description == null)
                    continue;

                string filePath = this.Input("Task filePath", "Enter a task filePath");
                if (filePath == "0")
                    break;

                if (filePath == null)
                    continue;


                bool status = this.Student.AddSubmission(this.Student.Id,  clasroom, description, filePath);
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
