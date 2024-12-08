using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int RoleId { get; set; }
        public string RoleType { get; set; }

        public Enrollment(int id, int classroomId, int roleId, string roleType)
        {
            Id = id;
            ClassroomId = classroomId;
            RoleId = roleId;
            RoleType = roleType;
        }


        // Overload: Construtor with Id default value
        public Enrollment(int classroomId, int roleId, string roleType)
            : this(0, classroomId, roleId, roleType)
        {
        }
    }
}
