namespace ProjectManagementSystem.Domain.Models
{
    public class Staff : Role
    {

        private Database database = new Database();

        public Staff(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active) { }


        //public List<Grade> ViewGrades()
        //{
        //    Alert alert = new Alert(this, "Submit", $"Staff {Name} viewed grades.");
        //    this.NotifyObservers(alert);
        //    return Grades.ToList();
        //}


        public bool CreateClassroom(string name)
        {
            bool exists = this.database.ClassroomExists(name);

            if (exists)
            {
                System.Console.WriteLine("\nClassroom already exists\n");
                return false;
            }

            this.database.InsertClassroom(name);
            return true;
        }


        public bool AssignRoleToClassroom(string classroom, string role, string typeRole)
        {
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                System.Console.WriteLine("\nClassroom not found\n");
                return false;
            }

            RoleSchema roleResult = this.database.GetRoleByUsername(role);
            if (roleResult == null)
            {
                System.Console.WriteLine("\nRole not found\n");
                return false;
            }

            if (roleResult.RoleType != typeRole)
            {
                System.Console.WriteLine($"\nOperation denied. Only allowed to assign '{typeRole}' role.\n");
                return false;
            }

            this.database.AddRoleToClassroom(classroomResult.Id, roleResult.Id, roleResult.RoleType);
            return true;
        }

        public bool MarkStudentAttendance(string classroom, string role, string typeRole)
        {
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                System.Console.WriteLine("\nClassroom not found\n");
                return false;
            }

            RoleSchema roleResult = this.database.GetRoleByUsername(role);
            if (roleResult == null)
            {
                System.Console.WriteLine("\nRole not found\n");
                return false;
            }

            if (roleResult.RoleType != typeRole)
            {
                System.Console.WriteLine($"\nOperation denied. Only allowed to assign '{typeRole}' role.\n");
                return false;
            }

            int? enrollmentId = this.database.GetEnrollmentId(classroomResult.Id, roleResult.Id);
            if (enrollmentId == null)
            {
                System.Console.WriteLine($"\nStudent enrollment not found.\n");
                return false;
            }

            this.database.AddAttendance((int)enrollmentId, DateTime.Now, true);
            return true;
        }
    }
}
