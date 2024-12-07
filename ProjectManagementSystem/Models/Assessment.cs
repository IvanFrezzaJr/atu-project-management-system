using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public string Classroom { get; set; }
        public string Description { get; set; }
        public float? MaxScore { get; set; }
    }
}
