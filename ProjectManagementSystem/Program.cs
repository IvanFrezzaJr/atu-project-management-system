using ProjectManagementSystem;

/* IMPORTANT
 * 
 * RUN `dotnet add package Microsoft.Data.Sqlite` INTO ProjectManagementSystem folder
 * 
 */

/// <summary>
/// The main program that demonstrates the publish-subscribe pattern.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main method that runs the program, simulating event submission and logging by a student and an admin.
    /// </summary>
    /// <param name="args">Command line arguments (not used in this program).</param>
    static void Main(string[] args)
    {


        Database database = new Database();
        database.CreateDatabase();

        // Create an authentication instance
        Authentication auth = new Authentication();

        string? inputUsername = null;
        string? inputPassword = null;
        int count = 0;
        while (true)
        {
            // request user and password
            inputUsername = auth.GetUsername();
            inputPassword = auth.GetPassword();

            // Try to authenticate
            if (auth.Authenticate(inputUsername, inputPassword))
            {
                Console.WriteLine("Login successful!");
                UserModel? usernModel = database.GetUserByUsername(inputUsername);

                /* TODO: load the domain Role and use it as a parameter in the menuFactory.
                 * The Role class will contain the operations to be call in the menu.
                 */
                MenuFactory menuFactory = new MenuFactory(usernModel.RoleType);
                MenuBuilder menu = menuFactory.Build();
                menu.Show();
            }
            else
            {
                count++;
                Console.WriteLine("Invalid username or password.\n");

                // after 3 attempts, exit
                if (count == 3)
                {
                    Console.WriteLine("Exceded numbers of attempts.\n");
                    break;
                }
            }
        }

    }
}
