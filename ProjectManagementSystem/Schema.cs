namespace ProjectManagementSystem
{

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
