using ProjectManagementSystem;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Core;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;

/* IMPORTANT
 * 
 * RUN `dotnet add package Microsoft.Data.Sqlite` INTO ProjectManagementSystem folder
 *  dotnet add package Moq
 *  
 */

namespace ProjectManagementSystem
{
    public static class Session
    {
        // Propriedade para armazenar o usuário logado
        public static Role LoggedUser { get; set; }
    }
}

/// <summary>
/// The main program that demonstrates the publish-subscribe pattern and user authentication workflow.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main method that initializes the application, handles user authentication, 
    /// and provides a role-based menu system.
    /// </summary>
    /// <param name="args">Command line arguments (not used in this program).</param>
    static void Main(string[] args)
    {
        // Initialize the necessary layers
        var config = new DatabaseConfig();
        config.CreateDatabase();

        var userView = new LoginView();
        var authRepository = new AuthRepository(config);
        var authenticationController = new AuthenticationController(userView, authRepository);

        // Start the authentication process
        authenticationController.AuthenticateUser();

        // Retrieve the role details of the authenticated user from the database.
        var roleRepository = new RoleRepository(config);
        Role userRole = roleRepository.GetRoleByUserName(authenticationController.UserName);

        // set session
        Session.LoggedUser = userRole;

        // Create a menu factory to generate the appropriate menu for the user's role.
        MenuFactory menuFactory = new MenuFactory(userRole);

        // Build the menu based on the user's role.
        MenuBuilder menu = menuFactory.Build();

        // Display the generated menu to the user.
        menu.Show();

    }
}
