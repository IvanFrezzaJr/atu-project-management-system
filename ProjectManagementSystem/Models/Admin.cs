using ProjectManagementSystem;


namespace ProjectManagementSystem.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Admin : Role
    {

        private string _role = string.Empty;

        private List<Alert> _logs = new List<Alert>();

        private Database database = new Database();

        private Logger logger = new Logger();

        public Admin(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active)
        {
        }


        /// <summary>
        /// Prints the logs of all alerts received by the admin.
        /// </summary>
        public List<Alert> PrintLogs()
        {
            return this.database.GetAllLogs();
           
        }


        public bool CreateRole(string username, string password, string roletype)
        {
            bool exists = this.database.RoleExists(username);

            if (exists)
            {
                System.Console.WriteLine("\nUser already exists\n");
                return false;
            }

            this.database.InsertRole(username, password, roletype);
            return true;
        }


        public bool ResetPassword(string username, string password)
        {
            this.database.UpdateRolePassword(username, password);
            return true;
        }
    }
}
