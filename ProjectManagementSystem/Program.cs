using ProjectManagementSystem;
using ProjectManagementSystem.Core;

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
        // Create an instance of the database and ensure it is initialized.
        Database database = new Database();
        database.CreateDatabase();

        // Create an instance of the authentication system.
        Authentication auth = new Authentication();

        // Variables to hold user input for username and password.
        string? inputUsername = null;
        string? inputPassword = null;

        // Counter for the number of authentication attempts.
        int count = 0;

        // Infinite loop for user login until successful authentication or max attempts are reached.
        while (true)
        {
            // Request the username from the user.
            inputUsername = auth.GetUsername();

            // Request the password from the user.
            inputPassword = auth.GetPassword();

            // Attempt to authenticate the user with the provided credentials.
            if (auth.Authenticate(inputUsername, inputPassword))
            {
                // Notify the user of successful login.
                Console.WriteLine("Login successful!");

                // Retrieve the role details of the authenticated user from the database.
                RoleSchema userRole = database.GetUserByName(inputUsername);

                // Create a menu factory to generate the appropriate menu for the user's role.
                MenuFactory menuFactory = new MenuFactory(userRole);

                // Build the menu based on the user's role.
                MenuBuilder menu = menuFactory.Build();

                // Display the generated menu to the user.
                menu.Show();
            }
            else
            {
                // Increment the authentication attempt counter.
                count++;

                // Notify the user of invalid credentials.
                Console.WriteLine("Invalid username or password.\n");

                // Check if the maximum number of attempts has been reached.
                if (count == 3)
                {
                    // Notify the user and exit the loop after 3 failed attempts.
                    Console.WriteLine("Exceeded number of attempts.\n");
                    break;
                }
            }
        }
    }
}
