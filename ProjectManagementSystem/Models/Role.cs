using ProjectManagementSystem.Core;

namespace ProjectManagementSystem.Models
{

    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public abstract class Role : Publisher
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }

        public Role(int id, string UserName, string Password, string RoleType, bool Active)
        {
            this.Id = id;
            this.UserName = UserName;
            this.Password = Password;
            this.RoleType = RoleType;
            this.Active = Active;
        }
    }
}

