using ProjectManagementSystem.Models;
using System.Data;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class TeacherView : BaseView
    {
       

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

