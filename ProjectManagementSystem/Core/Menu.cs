using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Core.Interfaces;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;


namespace ProjectManagementSystem.Core
{

    // Abstract class for a base menu with a name
    public abstract class BaseMenuName
    {
        // Property to store the name of the menu
        public string Name { get; set; }

        // Constructor to initialize the menu name
        protected BaseMenuName(string name)
        {
            Name = name;
        }
    }

    // Abstract class for a menu item, which extends from BaseMenuName and implements IMenuItem
    public abstract class MenuItem : BaseMenuName, IMenuItem
    {
        // Constructor to initialize the menu item name
        protected MenuItem(string name) : base(name) { }

        // Abstract method to execute the menu item
        public virtual void Execute()
        {
            //Console.WriteLine($">>{Name}");
            Console.WriteLine($"[Help] Press '0' to quit\n");
        }

        // Helper method to handle user input with error handling
        protected string? Input(string text, string errorMessage)
        {
            Console.Write($"{text}: ");
            string name = Console.ReadLine();
            if (name == "0")
            {
                return "0";
            }

            if (name == "" || name == null)
            {
                Console.WriteLine($"\n{errorMessage}\n");
                return null;
            }

            return name;
        }
    }

    // Class representing a menu, which contains menu items and submenus
    public class Menu : BaseMenuName, IMenu
    {
        // Lists to hold menu items and submenus
        List<MenuItem> _items = new List<MenuItem>();
        List<Menu> _submenus = new List<Menu>();
        List<BaseMenuName> _current = new List<BaseMenuName>();
        Menu? parent = null;

        // Constructor to initialize menu with a name
        public Menu(string name) : base(name) { }

        // Updates the current list of menu items and submenus
        private void UpdateCurrent()
        {
            // Ensures that the current list includes both items and submenus, excluding duplicates
            _current = _current
               .Concat(_items.Except(_current))
               .Concat(_submenus.Except(_current))
               .ToList();
        }

        // Adds a menu item to the current menu
        public void AddItem(MenuItem item)
        {
            _items.Add(item);
            UpdateCurrent();
        }

        // Adds a submenu to the current menu
        public void AddSubMenu(Menu submenu)
        {
            submenu.parent = this;
            _submenus.Add(submenu);
            UpdateCurrent();
        }

        // Displays the available options in the current menu
        public void DisplayOptions()
        {
            Console.WriteLine($"\n--- {Name} menu ---\n");

            int index = 0;

            // Loop through current menu items and display them with an index
            for (int i = 0; i < _current.Count; i++)
            {
                index++;
                Console.WriteLine($"{index}. {_current[i].Name}");
            }

            // Show the back option or exit option based on the presence of a parent menu
            if (parent is not null)
            {
                Console.WriteLine("0. Go Back");
            }
            else
            {
                Console.WriteLine("0. Exit");
            }
        }

        // Handles the user's choice and executes the corresponding action
        public bool HandleChoice(int option)
        {
            // If user selects option 0, exit the loop
            if (option == 0)
            {
                return false;
            }
            else
            {
                // If the selected option is valid, execute the corresponding menu item or show submenu
                if (option >= 1 && option <= _current.Count)
                {
                    var item = _current[option - 1];

                    // Execute the menu item or show submenu if it's a menu
                    if (item is MenuItem menuItem)
                    {
                        menuItem.Execute();
                    }
                    else if (item is Menu menu)
                    {
                        menu.Show();
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }
                }
                return true;
            }
        }

        // Displays the menu and processes user input in a loop
        public void Show()
        {
            while (true)
            {
                DisplayOptions();  // Display current menu options
                try
                {
                    // Prompt the user to input a number
                    Console.Write("\nInput a number: ");
                    string optionString = Console.ReadLine();
                    int option = int.Parse(optionString.Trim());

                    Console.Clear();  // Clear the console for a fresh display
                    bool result = HandleChoice(option);  // Handle the choice
                    if (!result)
                    {
                        break;  // Exit the loop if the user selects "Exit" or "Go Back"
                    }
                }
                catch (FormatException)
                {
                    // Handle invalid input format
                    Console.WriteLine("Invalid input! Please enter an integer.");
                }
            }
        }
    }


    // Abstract builder class to create a menu
    public abstract class MenuBuilder
    {
        protected Menu mainMenu = new Menu("Main");

        public MenuBuilder()
        {
        }

        // Shows the constructed menu
        public void Show()
        {
            mainMenu.Show();
        }

        // Abstract method to build the menu
        public abstract void Build();
    }

    // Builder class for the admin menu
    class MenuAdminBuilder : MenuBuilder
    {
        private readonly AdminController _adminController;

        public MenuAdminBuilder(AdminController adminController)
        {
            _adminController = adminController;
        }

        public override void Build()
        {
            MenuItem ITOption1 = new CreatePrincipalMenuItem("Create Principal User", _adminController);
            MenuItem ITOption2 = new CreateStaffMenuItem("Create Staff User", _adminController);
            MenuItem ITOption3 = new CreateTeacherMenuItem("Create Teacher User", _adminController);
            MenuItem ITOption4 = new CreateStudentMenuItem("Create Student User", _adminController);
            MenuItem ITOption5 = new ResetPasswordMenuItem("Perform Password Reset for Users", _adminController);
            MenuItem ITOption6 = new ShowLogsMenuItem("Show logs", _adminController);

            mainMenu.AddItem(ITOption1);
            mainMenu.AddItem(ITOption2);
            mainMenu.AddItem(ITOption3);
            mainMenu.AddItem(ITOption4);
            mainMenu.AddItem(ITOption5);
            mainMenu.AddItem(ITOption6);
        }
    }

    // Builder class for the principal menu
    class MenuPrincipalBuilder : MenuBuilder
    {
        private readonly PrincipalController _principalController;

        public MenuPrincipalBuilder(PrincipalController principalController)
        {
            _principalController = principalController;
        }

        public override void Build()
        {
            MenuItem PrincipalOption1 = new AssignTeacherToClassroomMenuItem("Assign Teacher to Classroom", _principalController);
            MenuItem PrincipalOption2 = new ActiveRoleMenuItem("Enable/Disable User Accounts", _principalController);
            MenuItem PrincipalOption3 = new ShowGradeByClassroomMenuItem("View Grades (Read-Only Access)", _principalController);

            mainMenu.AddItem(PrincipalOption1);
            mainMenu.AddItem(PrincipalOption2);
            mainMenu.AddItem(PrincipalOption3);
        }
    }

    // Builder class for the staff menu
    class MenuStaffBuilder : MenuBuilder
    {
        public StaffController _staffController;

        public MenuStaffBuilder(StaffController staffController)
        {
            _staffController = staffController;
        }

        public override void Build()
        {
            MenuItem StaffOption1 = new CreateClassroomMenuItem("Create New Classroom", _staffController);
            MenuItem StaffOption2 = new AssignStudentToClassroomMenuItem("Assign Student to Classroom", _staffController);
            MenuItem StaffOption3 = new StaffMarkStudentAttendanceMenuItem("Mark Student Attendance", _staffController);

            mainMenu.AddItem(StaffOption1);
            mainMenu.AddItem(StaffOption2);
            mainMenu.AddItem(StaffOption3);
        }
    }

    // Builder class for the teacher menu
    class MenuTeacherBuilder : MenuBuilder
    {
        public TeacherController _teacherController;

        public MenuTeacherBuilder(TeacherController teacherController)
        {
            _teacherController = teacherController;
        }
        public override void Build()
        {
            MenuItem TeacherOption1 = new AddAssignmentMenuItem("Add Assignments for Class", _teacherController);
            MenuItem TeacherOption2 = new SetStudentGradeMenuItem("Set Student Grades", _teacherController);
            MenuItem TeacherOption3 = new TeacherMarkStudentAttendanceMenuItem("Mark Student Attendance", _teacherController);
            MenuItem TeacherOption4 = new ShowClassroomGradeMenuItem("View Grades (Read-Only Access)", _teacherController);

            mainMenu.AddItem(TeacherOption1);
            mainMenu.AddItem(TeacherOption2);
            mainMenu.AddItem(TeacherOption3);
            mainMenu.AddItem(TeacherOption4);
        }
    }

    // Builder class for the student menu
    class MenuStudentBuilder : MenuBuilder
    {
        public StudentController _studentController;

        public MenuStudentBuilder(StudentController studentController)
        {
            _studentController = studentController;
        }

        public override void Build()
        {
            MenuItem StudentOption1 = new AddSubmissionMenuItem("Submit Assignments", _studentController);
            MenuItem StudentOption2 = new ShowScoreMenuItem("View Grades (Read-Only Access)", _studentController);

            mainMenu.AddItem(StudentOption1);
            mainMenu.AddItem(StudentOption2);
        }
    }


    // Factory class to create different menus based on the role
    class MenuFactory
    {
        private Role Role { get; set; }

        public MenuFactory(Role role)
        {
            Role = role;
        }

        // Builds the appropriate menu based on the role
        public MenuBuilder Build()
        {
            var config = new DatabaseConfig();

            if (Role.RoleType == "admin")
            {
                var adminInterface = new AdminView();
                var roleRepository = new RoleRepository(config);
                var logRepository = new LogRepository(config);
                var adminController = new AdminController(adminInterface, roleRepository, logRepository);

                MenuAdminBuilder menuAdmin = new MenuAdminBuilder(adminController);
                menuAdmin.Build();
                return menuAdmin;
            }
            else if (Role.RoleType == "principal")
            {
                var principalView = new PrincipalView();
                var roleRepository = new RoleRepository(config);
                var classroomRepository = new ClassroomRepository(config);
                var principalController = new PrincipalController(principalView, roleRepository, classroomRepository);

                MenuPrincipalBuilder menuPrincipal = new MenuPrincipalBuilder(principalController);
                menuPrincipal.Build();
                return menuPrincipal;

            }
            else if (Role.RoleType == "staff")
            {
                var staffView = new StaffView();
                var roleRepository = new RoleRepository(config);
                var classroomRepository = new ClassroomRepository(config);
                var staffController = new StaffController(staffView, roleRepository, classroomRepository);

                MenuStaffBuilder menuStaff = new MenuStaffBuilder(staffController);
                menuStaff.Build();
                return menuStaff;
            }
            else if (Role.RoleType == "teacher")
            {
                var teacherView = new TeacherView();
                var roleRepository = new RoleRepository(config);
                var classroomRepository = new ClassroomRepository(config);
                var teacherController = new TeacherController(teacherView, roleRepository, classroomRepository);

                MenuTeacherBuilder menuTeacher = new MenuTeacherBuilder(teacherController);
                menuTeacher.Build();
                return menuTeacher;
            }
            else if (Role.RoleType == "student")
            {
                var studentView = new StudentView();
                var roleRepository = new RoleRepository(config);
                var classroomRepository = new ClassroomRepository(config);
                var studentController = new StudentController(studentView, roleRepository, classroomRepository);

                MenuStudentBuilder menuStudent = new MenuStudentBuilder(studentController);
                menuStudent.Build();
                return menuStudent;
            }
            else
            {
                throw new ArgumentException("Role type not found");
            }
        }

        public void AttachLogger(IPublisher publisher)
        {
            Logger logger = new Logger();
            publisher.AddSubscriber(logger);
        }

    }
}
