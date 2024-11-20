namespace ProjectManagementSystem.Domain.Models
{
    public class Principal : Role
    {

        private Database database = new Database();

        public Principal(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active) { }


        //public List<Grade> ViewGrades()
        //{
        //    Alert alert = new Alert(this, "Submit", $"Principal {Name} viewed grades.");
        //    this.NotifyObservers(alert);
        //    return Grades.ToList();
        //}



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


        public bool ToggleRole(string username)
        {
            RoleSchema role = this.database.GetRoleByUsername(username);

            if (role == null)
            {
                System.Console.WriteLine("\nUser not found\n");
                return false;
            }

            bool active = (role.Active) ? false : true;

            this.database.ActivateRole(username, active);
            return active;
        }

        public List<dynamic> DisplayStudentSubmissions(string classroomName)
        {
            return this.database.GetStudentSubmissions(classroomName);

        }

    }
}
