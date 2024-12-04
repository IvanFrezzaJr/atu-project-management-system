using System.Reflection;
using System.Xml.Linq;

namespace ProjectManagementSystem.Models
{
    /// <summary>
    /// Represents a Staff member, inheriting from the Role class.
    /// </summary>
    public class Staff : Role
    {
        // Instance of the database to handle data operations.
        private Database database = new Database();

        /// <summary>
        /// Initializes a new instance of the Staff class.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <param name="username">The username of the staff member.</param>
        /// <param name="password">The password of the staff member.</param>
        /// <param name="roleType">The role type of the staff member.</param>
        /// <param name="active">Indicates whether the staff member is active.</param>
        public Staff(int id, string username, string password, string roleType, bool active)
            : base(id, username, password, roleType, active) { }

        /// <summary>
        /// Creates a new classroom if it does not already exist.
        /// </summary>
        /// <param name="name">The name of the classroom to be created.</param>
        /// <returns>True if the classroom is successfully created; otherwise, false.</returns>
        public bool CreateClassroom(string name)
        {
            // Check if the classroom already exists in the database.
            bool exists = this.database.ClassroomExists(name);

            if (exists)
            {
                // Notify observers that the classroom already exists.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom already exists"
                }, true);
                return false;
            }

            // Insert the new classroom into the database.
            this.database.InsertClassroom(name);

            // Notify observers of the successful creation of the classroom.
            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Classroom {name} added successfully"
            }, false);
            return true;
        }

        /// <summary>
        /// Assigns a role to a specific classroom if all conditions are met.
        /// </summary>
        /// <param name="classroom">The name of the classroom.</param>
        /// <param name="role">The username of the role to be assigned.</param>
        /// <param name="typeRole">The type of role to be assigned.</param>
        /// <returns>True if the role is successfully assigned; otherwise, false.</returns>
        public bool AssignRoleToClassroom(string classroom, string role, string typeRole)
        {
            // Fetch the classroom details by name.
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                // Notify observers if the classroom is not found.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom not found"
                }, true);
                return false;
            }

            // Fetch the role details by username.
            RoleSchema roleResult = this.database.GetRoleByUsername(role);
            if (roleResult == null)
            {
                // Notify observers if the role is not found.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Role not found"
                }, true);
                return false;
            }

            // Check if the role type matches the required type.
            if (roleResult.RoleType != typeRole)
            {
                // Notify observers if the role type is not allowed.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
                }, true);
                return false;
            }

            // Assign the role to the classroom in the database.
            this.database.AddRoleToClassroom(classroomResult.Id, roleResult.Id, roleResult.RoleType);

            // Notify observers of the successful assignment of the role.
            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Added role {roleResult.Id} to classroom '{classroomResult.Id}'"
            }, false);
            return true;
        }

        /// <summary>
        /// Marks attendance for a student in a specific classroom.
        /// </summary>
        /// <param name="classroom">The name of the classroom.</param>
        /// <param name="role">The username of the student role.</param>
        /// <param name="typeRole">The type of role for attendance marking.</param>
        /// <returns>True if attendance is successfully marked; otherwise, false.</returns>
        public bool MarkStudentAttendance(string classroom, string role, string typeRole)
        {
            // Fetch the classroom details by name.
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                // Notify observers if the classroom is not found.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom not found"
                }, true);
                return false;
            }

            // Fetch the role details by username.
            RoleSchema roleResult = this.database.GetRoleByUsername(role);
            if (roleResult == null)
            {
                // Notify observers if the role is not found.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Role not found"
                }, true);
                return false;
            }

            // Check if the role type matches the required type.
            if (roleResult.RoleType != typeRole)
            {
                // Notify observers if the role type is not allowed.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
                }, true);
                return false;
            }

            // Fetch the enrollment ID for the student in the classroom.
            int? enrollmentId = this.database.GetEnrollmentId(classroomResult.Id, roleResult.Id);
            if (enrollmentId == null)
            {
                // Notify observers if the enrollment is not found.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Student enrollment not found"
                }, true);
                return false;
            }

            // Mark the attendance in the database.
            this.database.AddAttendance((int)enrollmentId, DateTime.Now, true);

            // Notify observers of the successful attendance marking.
            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Added attendance to {roleResult.UserName} in {classroomResult.Name}"
            }, true);
            return true;
        }
    }
}
