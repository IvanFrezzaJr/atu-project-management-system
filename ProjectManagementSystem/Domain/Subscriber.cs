using ProjectManagementSystem.Domain.Interfaces;
using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Domain
{
    /// <summary>
    /// Abstract class representing a subscriber which receives alerts.
    /// </summary>
    public abstract class Subscriber : ISubscriber
    {
        /// <summary>
        /// Abstract method that must be implemented by derived classes to update the subscriber with the alert.
        /// </summary>
        /// <param name="alert">The alert to update the subscriber with.</param>
        public abstract void Update(Alert alert, bool print);
    }
}