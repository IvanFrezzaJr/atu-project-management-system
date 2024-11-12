using ProjectManagementSystem;

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
        //// Create instances of students and an admin
        //Student ivan = new Student("Ivan");
        //Student bruna = new Student("Bruna");
        //Admin admin = new Admin("Super admin");

        //// Subscribe the admin to both students
        //ivan.AddSubscriber(admin);
        //bruna.AddSubscriber(admin);

        //// Students submit events
        //ivan.Submit("Hello");
        //bruna.Submit("World");

        //// Admin prints the logs of the events it received
        //admin.PrintLogs();

        MenuItem option1 = new Option1MenuItem("Option 1");
        MenuItem option2 = new Option2MenuItem("Option 2");
        MenuItem option3 = new Option3MenuItem("Option 3");

        //Criando menus e submenus
        Menu mainMenu = new Menu("Main menu");
        mainMenu.AddItem(option1);
        mainMenu.AddItem(option2);

        // Submenu de exemplo
        Menu submenu = new Menu("Submenu example");
        submenu.AddItem(option1);
        submenu.AddItem(option2);

        // Adiciona o submenu ao menu principal
        mainMenu.AddSubMenu(submenu);
        mainMenu.AddItem(option3);

        // Exibindo o menu principal
        mainMenu.Show();


    }
}
