using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; }
    }


}
