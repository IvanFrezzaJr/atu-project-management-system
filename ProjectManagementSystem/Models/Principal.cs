namespace ProjectManagementSystem.Models
{
    public class Principal : Role
    {

        public Principal(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Principal(string userName, string password)
            : this(0, userName, password, true, "principal")
        {
        }

    }
}
