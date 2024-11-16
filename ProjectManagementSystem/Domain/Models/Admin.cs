using ProjectManagementSystem.Domain.Interfaces;

using System;
using System.Collections.Generic;

namespace ProjectManagementSystem.Domain.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Admin : Role, ISubscriber
    {
        /// <summary>
        /// The role of the admin.
        /// </summary>
        private string _role = string.Empty;

        /// <summary>
        /// A list of logs containing alerts received by the admin.
        /// </summary>
        private List<Alert> _logs = new List<Alert>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Admin"/> class with the specified role.
        /// </summary>
        /// <param name="role">The role of the admin.</param>
        public Admin(int id, string userName, string password, string roleType) : base(id, userName, password, roleType)
        {
        }


        /// <summary>
        /// Updates the admin with the alert, logging it for future reference.
        /// </summary>
        /// <param name="alert">The alert to log.</param>
        public void Update(Alert alert)
        {
            this._logs.Add(alert);
        }


        /// <summary>
        /// Prints the logs of all alerts received by the admin.
        /// </summary>
        public void PrintLogs()
        {
            foreach (var log in this._logs)
            {
                System.Console.WriteLine($"[{log.CreatedAt}] - {log.Role.UserName}.{log.Action}: {log.Message}");
            }
        }


        public void CreatePrincipal()
        {
            System.Console.WriteLine("Create principal with success!!");
        }
    }
}
