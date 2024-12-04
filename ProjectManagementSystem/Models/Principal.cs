using System.Reflection;

namespace ProjectManagementSystem.Models
{
    public class Principal : Role
    {

        private Database database = new Database();

        public Principal(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active) { }


        public bool AssignRoleToClassroom(string classroom, string role, string typeRole)
        {
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom not found"
                }, true);
                return false;
            }

            RoleSchema roleResult = this.database.GetRoleByUsername(role);
            if (roleResult == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Role not found"
                }, true);
                return false;
            }

            if (roleResult.RoleType != typeRole)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
                }, true);
                return false;
            }

            this.database.AddRoleToClassroom(classroomResult.Id, roleResult.Id, roleResult.RoleType);
            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Added role '{typeRole}' to classroom  {classroomResult.Name}"
            }, false);
            return true;
        }


        public bool ToggleRole(string username)
        {
            RoleSchema role = this.database.GetRoleByUsername(username);

            if (role == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Role not found"
                }, true);
                return false;
            }

            bool active = (role.Active) ? false : true;

            this.database.ActivateRole(username, active);
            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Role {role} activate: {active}"
            }, false);
            return active;
        }

        public List<dynamic> DisplayStudentSubmissions(string classroomName)
        {

            return this.database.GetStudentSubmissions(classroomName);

        }

    }
}
