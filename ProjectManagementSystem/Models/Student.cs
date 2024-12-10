namespace ProjectManagementSystem.Models
{
    public class Student : Role
    {

        public Student(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        // Overload: Construtor with Id default value
        public Student(string userName, string password)
            : this(0, userName, password, true, "student")
        {
        }
    }
}
