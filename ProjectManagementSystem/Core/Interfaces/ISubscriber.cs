namespace ProjectManagementSystem.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a subscriber, which can update on an alert.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Updates the subscriber with the given alert.
        /// </summary>
        /// <param name="alert">The alert to update the subscriber with.</param>
        void Update(Alert alert, bool print);
    }

}