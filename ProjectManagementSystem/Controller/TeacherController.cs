using ProjectManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}
