namespace ProjectManagementSystem.Models
{
    /// <summary>
    /// Represents a Staff member, inheriting from the Role class.
    /// </summary>
    public class Staff : Role
    {
        // Instance of the database to handle data operations.
        private Database_ database = new Database_();

        public Staff(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Staff(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "staff")
        {
        }
    }
}
