namespace ProjectManagementSystem.Core.Interfaces
{
    /// <summary>
    /// Interface representing a menu with options and submenus.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Displays the menu options to the user.
        /// </summary>
        void Show();

        /// <summary>
        /// Adds a menu item to the current menu.
        /// </summary>
        /// <param name="item">The menu item to add.</param>
        void AddItem(MenuItem item);

        /// <summary>
        /// Adds a submenu to the current menu.
        /// </summary>
        /// <param name="submenu">The submenu to add.</param>
        void AddSubMenu(Menu submenu);
    }
}
