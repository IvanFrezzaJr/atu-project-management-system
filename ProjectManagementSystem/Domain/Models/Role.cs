using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{


    public class RoleSchema
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RoleType { get; set; }
    }


    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public class Role : Publisher
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RoleType { get; set; }

        public Role(int id, string userName, string password, string roleType)
        {
            Id = id;
            UserName = userName;
            Password = password;
            RoleType = roleType;
    }
    }
}