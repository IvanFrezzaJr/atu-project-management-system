using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{
    public class Student : Role
    {
        public Student(int id, string userName, string password, string roleType) : base(id, userName, password, roleType) { }


        public void SubmitAssessment(string title)
        {
            //var assessment = new Assessment
            //{
            //    Title = title,
            //    SubmissionDate = DateTime.Now,
            //    StudentId = this.Id
            //};
            //Assessments.Add(assessment);

            // Notifica os assinantes sobre a submissão
            string message = $"Student {this.UserName} submitted: {title}";

            System.Console.WriteLine(message);

            // Create an alert and notify observers
            Alert alert = new Alert(this, "Submit", message);
            this.NotifyObservers(alert);
        }

        //public List<Grade> ViewGrades()
        //{
        //    Alert alert = new Alert(this, "Submit", $"Student {Name} viewed grades.");
        //    this.NotifyObservers(alert);
        //    return Grades.ToList();
        //}
    }
}
