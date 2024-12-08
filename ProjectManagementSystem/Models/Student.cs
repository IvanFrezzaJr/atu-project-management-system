using ProjectManagementSystem._to_be_deleted;

namespace ProjectManagementSystem.Models
{
    public class Student : Role
    {

        private Database_ database = new Database_();

        public Student(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Student(string userName, string password, bool active, string roleType)
            : this(0, userName, password, active, "student")
        {
        }
    }
}
