using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{

    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Classroom(int id, string name)
        {
            Id = id;
            Name = name; 
        }


        // Overload: Construtor with Id default value
        public Classroom(string name) 
            : this(0, name)
        {
        }
    }

}
