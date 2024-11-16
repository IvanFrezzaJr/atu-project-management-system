using ProjectManagementSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{
    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public class Role : Publisher
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        public string Name { get; set; }

        public Role(string name)
        {
            Name = name;
        }
    }
}