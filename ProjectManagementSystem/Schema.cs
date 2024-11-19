﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem
{
    public class RoleSchema
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }
    }

    public class ClassroomSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
