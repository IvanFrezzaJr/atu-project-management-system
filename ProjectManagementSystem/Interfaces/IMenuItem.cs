using ProjectManagementSystem.Domain.Models;

namespace ProjectManagementSystem.Interfaces
{
    /// <summary>
    /// Interface representing a menu item with an executable action.
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// Executes the action associated with the menu item.
        /// </summary>
        void Execute();
    }


}
