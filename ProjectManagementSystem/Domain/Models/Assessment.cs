using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}