using ProjectManagementSystem.Core;
using System.Text.RegularExpressions;

namespace ProjectManagementSystem.Models
{

    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public abstract class User : Publisher
    {
        private int _id;
        private string _userName;
        private string _password;
        private bool _active;

        // Property for Id (optional, no validation required)
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        // Property for UserName with validation
        public string UserName
        {
            get => _userName;
            set
            {
                // Ensure UserName has at least 3 characters and no special characters
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || !Regex.IsMatch(value, "^[a-zA-Z0-9]+$"))
                {
                    throw new ArgumentException("UserName must have at least 3 characters and contain no special characters.");
                }
                _userName = value;
            }
        }

        // Property for Password with validation
        public string Password
        {
            get => _password;
            set
            {
                // Ensure Password has at least 3 characters
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                {
                    throw new ArgumentException("Password must have at least 3 characters.");
                }
                _password = value;
            }
        }

        // Property for Active with default value set to true
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        // Constructor
        public User(int id, string userName, string password, bool active = true)
        {
            Id = id; // Id is optional
            UserName = userName; // Validation is handled in the property setter
            Password = password; // Validation is handled in the property setter
            Active = active; // Default value is true if not specified
        }
    }



    public class Role : User
    {
        private string _roleType;

        // Property for RoleType with validation
        public string RoleType
        {
            get => _roleType;
            set
            {
                // Ensure RoleType is one of the allowed values
                string[] validRoles = { "admin", "principal", "staff", "teacher", "student" };
                if (string.IsNullOrWhiteSpace(value) || Array.IndexOf(validRoles, value.ToLower()) == -1)
                {
                    throw new ArgumentException("RoleType must be one of the following: admin, principal, staff, teacher, student.");
                }
                _roleType = value;
            }
        }

        // Constructor
        public Role(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active)
        {
            RoleType = roleType; // Validation is handled in the property setter
        }

        // Overload: Constructor with Id default value
        public Role(string userName, string password, bool active, string roleType)
            : this(0, userName, password, true, roleType)
        {
        }
    }
}

