using ProjectManagementSystem.Core;

namespace ProjectManagementSystem.Models
{

    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public abstract class User : Publisher
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public User(int id, string UserName, string Password, bool Active)
        {
            this.Id = id;
            this.UserName = UserName;
            this.Password = Password;
            this.Active = Active;
        }
    }


    public class Role : User
    {
        public string RoleType { get; set; }
        public Role(int id, string UserName, string Password, bool Active, string RoleType) : base(id, UserName, Password, Active)
        {
            this.RoleType = RoleType;
        }

        // Overload: Construtor with Id default value
        public Role(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, roleType)
        {
        }
    }
}

