namespace ProjectManagementSystem.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Admin : Role
    {

        public Admin(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }


        // Overload: Construtor with Id default value
        public Admin(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "admin") 
        {
        }
    }
}
