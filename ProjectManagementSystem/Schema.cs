namespace ProjectManagementSystem
{
    public class RoleSchema
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }
    }

    public class ClassroomSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }


    public class AttendanceSchema
    {
        DateTime Date { get; set; }
        bool Present { get; set; }
    }




    /// <summary>
    /// Represents an alert that can be sent to observers.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// The role associated with the alert. E.g. Student, Teacher
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The action related to the alert. It must be passed to the class methods.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The message or description of the alert.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The timestamp when the alert was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

}
