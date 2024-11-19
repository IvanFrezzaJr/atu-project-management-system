using ProjectManagementSystem.Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using ProjectManagementSystem;


namespace ProjectManagementSystem.Domain.Models
{
    /// <summary>
    /// Represents an admin subscriber, who can receive and log alerts.
    /// </summary>
    public class Admin : Role, ISubscriber
    {

        private string _role = string.Empty;

        private List<Alert> _logs = new List<Alert>();

        private Database database = new Database();

        public Admin()
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
                System.Console.WriteLine($"[{log.CreatedAt}] - 'log.Role.UserName'.{log.Action}: {log.Message}");
            }
        }


        public bool CreateRole(string username, string password, string roletype)
        {
            bool exists = this.database.RoleExists(username);

            if (exists)
            {
                System.Console.WriteLine("\nUser already exists\n");
                return false;
            }

            this.database.InsertRole(username, password, roletype);
            return true;
        }


        public bool ResetPassword(string username, string password)
        {
            this.database.UpdateRolePassword(username, password);
            return true;
        }
    }
}
