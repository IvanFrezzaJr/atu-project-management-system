using ProjectManagementSystem;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Core;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Views;
using ProjectManagementSystem.Models;

/* IMPORTANT
 * 
 * RUN `dotnet add package Microsoft.Data.Sqlite` INTO ProjectManagementSystem folder
 *  dotnet add package Moq
 *  
 */

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
        //// Create an instance of the database and ensure it is initialized.
        //Database database = new Database();
        //database.CreateDatabase();

        //// Create an instance of the authentication system.
        //Authentication auth = new Authentication();




        // Initialize the necessary layers
        var userInterface = new UserInterface();
        var config = new DatabaseConfig();
        var authRepository = new AuthRepository(config);
        var authenticationController = new AuthenticationController(userInterface, authRepository);

        // Start the authentication process
        authenticationController.AuthenticateUser();

        var roleRepository = new RoleRepository(config);
        string currentUserName = authenticationController.UserName;
        // Retrieve the role details of the authenticated user from the database.
        Role userRole = roleRepository.GetRoleByUserName(currentUserName);

        // Create a menu factory to generate the appropriate menu for the user's role.
        MenuFactory menuFactory = new MenuFactory(userRole);

        // Build the menu based on the user's role.
        MenuBuilder menu = menuFactory.Build();

        // Display the generated menu to the user.
        menu.Show();

    }
}
