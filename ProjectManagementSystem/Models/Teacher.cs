namespace ProjectManagementSystem.Models
{
    public class Teacher : Role
    {

        private Database_ database = new Database_();

        public Teacher(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Teacher(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "teacher")
        {
        }

    }
}
