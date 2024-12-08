using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{
    public class Submission
    {

        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public int StudentId { get; set; }
        public float Score { get; set; }
        public string File { get; set; }
    }
}
