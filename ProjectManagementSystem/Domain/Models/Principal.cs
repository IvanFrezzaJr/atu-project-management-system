using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{
    public class Principal : Role
    {
        public Principal(int id, string userName, string password, string roleType) : base(id, userName, password, roleType) { }


        //public List<Grade> ViewGrades()
        //{
        //    Alert alert = new Alert(this, "Submit", $"Principal {Name} viewed grades.");
        //    this.NotifyObservers(alert);
        //    return Grades.ToList();
        //}
    }
}