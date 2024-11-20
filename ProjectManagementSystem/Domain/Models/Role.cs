using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{

    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public class Role : Publisher
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }

        public Role(int id, string Username, string Password, string RoleType, bool Active) {
            Id = id;
            Username = Username;
            Password = Password;
            RoleType = RoleType;
            Active = Active;
        }
    }               
}                    
                     
                     