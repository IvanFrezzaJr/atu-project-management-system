namespace ProjectManagementSystem.Models
{
    public class Principal : Role
    {

        private Database_ database = new Database_();

        public Principal(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Principal(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "principal")
        {
        }

    }
}
