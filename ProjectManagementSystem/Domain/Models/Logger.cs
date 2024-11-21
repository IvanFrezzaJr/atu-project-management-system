using ProjectManagementSystem.Interfaces;


namespace ProjectManagementSystem.Domain.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Logger : ISubscriber
    {
        private Database database = new Database();


        /// <summary>
        /// Updates the admin with the alert, logging it for future reference.
        /// </summary>
        /// <param name="alert">The alert to log.</param>
        public void Update(Alert alert, bool print=true)
        {
            if (print)
            {
                System.Console.WriteLine($"\n{alert.Message}\n");
            }
            this.database.InsertLog(alert);
        }
    }
}
