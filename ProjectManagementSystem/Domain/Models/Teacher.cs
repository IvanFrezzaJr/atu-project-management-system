namespace ProjectManagementSystem.Domain.Models
{
    public class Teacher : Role
    {

        private Database database = new Database();

        public Teacher(int id, string userName, string password, string roleType, bool active) : base(id, userName, password, roleType, active) { }

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

        public bool AddAssignment(string classroom, string role, string description, float maxScore)
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

            this.database.AddAssessment(classroomResult.Id, roleResult.Id, description, maxScore);
            return true;
        }

    }
}
