namespace ProjectManagementSystem.Domain.Models
{

    /// <summary>
    /// Represents an alert that can be sent to observers.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// The role associated with the alert. E.g. Student, Teacher
        /// </summary>
        public Role Role { get; set; }

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
        public DateTime CreatedAt { get; } = DateTime.Now;

        public Alert(Role role, string action, string message)
        {
            Role = role;
            Action = action;
            Message = message;
        }
    }
}