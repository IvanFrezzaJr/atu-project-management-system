using ProjectManagementSystem.Domain.Interfaces;
using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Domain
{

    /// <summary>
    /// Abstract class representing a publisher that can manage subscribers and notify them.
    /// </summary>
    public abstract class Publisher : IPublisher
    {
        /// <summary>
        /// List of subscribers to be notified.
        /// </summary>
        private List<ISubscriber> _subscribers = new List<ISubscriber>();

        /// <summary>
        /// Adds a subscriber to the publisher's subscriber list.
        /// </summary>
        /// <param name="subscriber">The subscriber to add.</param>
        public void AddSubscriber(ISubscriber subscriber)
        {
            // Add subscriber if not already present
            if (!_subscribers.Contains(subscriber))
            {
                _subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// Notifies all subscribers about the alert.
        /// </summary>
        /// <param name="alert">The alert to notify the subscribers about.</param>
        public void NotifyObservers(Alert alert, bool print=false)
        {
            // Notify each subscriber with the alert
            foreach (var subscriber in _subscribers)
            {
                subscriber.Update(alert, print);
            }
        }
    }
}