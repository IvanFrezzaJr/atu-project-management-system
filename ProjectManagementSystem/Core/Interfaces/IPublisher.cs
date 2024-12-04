using ProjectManagementSystem;

namespace ProjectManagementSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a publisher, which can add subscribers and notify them.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Adds a subscriber to the publisher.
        /// </summary>
        /// <param name="subscriber">The subscriber to add.</param>
        void AddSubscriber(ISubscriber subscriber);

        /// <summary>
        /// Notifies all subscribers about an alert.
        /// </summary>
        /// <param name="alert">The alert to notify subscribers with.</param>
        void NotifyObservers(Alert alert, bool print);
    }


}
