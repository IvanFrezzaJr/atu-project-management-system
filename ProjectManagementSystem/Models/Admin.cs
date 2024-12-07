namespace ProjectManagementSystem.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Admin : Role
    {

        private string _role = string.Empty;

        private List<Alert> _logs = new List<Alert>();

        private Database_ database = new Database_();

        private Logger logger = new Logger();

        public Admin(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }


        // Overload: Construtor with Id default value
        public Admin(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "admin") 
        {
        }
    }
}
