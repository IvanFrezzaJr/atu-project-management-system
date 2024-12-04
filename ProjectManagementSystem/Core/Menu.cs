using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Core.Interfaces;
using ProjectManagementSystem.Models;
using System.Drawing;


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
            Console.WriteLine($"--- {Name} ---");
            Console.WriteLine($"quit: 0 + Enter\n");
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
    class MenuAdmin : MenuBuilder
    {
        public Admin Admin { get; set; }

        public MenuAdmin(Admin admin)
        {
            Admin = admin;
        }

        public override void Build()
        {
            MenuItem ITOption1 = new CreatePrincipalMenuItem("Create Principal User", Admin);
            MenuItem ITOption2 = new CreateStaffMenuItem("Create Staff User", Admin);
            MenuItem ITOption3 = new CreateTeacherMenuItem("Create Teacher User", Admin);
            MenuItem ITOption4 = new CreateStudentMenuItem("Create Student User", Admin);
            MenuItem ITOption5 = new ResetPasswordMenuItem("Perform Password Reset for Users", Admin);
            MenuItem ITOption6 = new ShowLogsMenuItem("Show logs", Admin);

            mainMenu.AddItem(ITOption1);
            mainMenu.AddItem(ITOption2);
            mainMenu.AddItem(ITOption3);
            mainMenu.AddItem(ITOption4);
            mainMenu.AddItem(ITOption5);
            mainMenu.AddItem(ITOption6);
        }
    }

    // Builder class for the principal menu
    class MenuPrincipal : MenuBuilder
    {
        public Principal Principal { get; set; }

        public MenuPrincipal(Principal principal)
        {
            Principal = principal;
        }

        public override void Build()
        {
            MenuItem PrincipalOption1 = new AssignTeacherToClassroomMenuItem("Assign Teacher to Classroom", Principal);
            MenuItem PrincipalOption2 = new ActiveRoleMenuItem("Enable/Disable User Accounts", Principal);
            MenuItem PrincipalOption3 = new ShowGradeByClassroomMenuItem("View Grades (Read-Only Access)", Principal);

            mainMenu.AddItem(PrincipalOption1);
            mainMenu.AddItem(PrincipalOption2);
            mainMenu.AddItem(PrincipalOption3);
        }
    }

    // Builder class for the staff menu
    class MenuStaff : MenuBuilder
    {
        public Staff Staff { get; set; }

        public MenuStaff(Staff staff)
        {
            Staff = staff;
        }

        public override void Build()
        {
            MenuItem StaffOption1 = new CreateClassroomMenuItem("Create New Classroom", Staff);
            MenuItem StaffOption2 = new AssignStudentToClassroomMenuItem("Assign Student to Classroom", Staff);
            MenuItem StaffOption3 = new StaffMarkStudentAttendanceMenuItem("Mark Student Attendance", Staff);

            mainMenu.AddItem(StaffOption1);
            mainMenu.AddItem(StaffOption2);
            mainMenu.AddItem(StaffOption3);
        }
    }

    // Builder class for the teacher menu
    class MenuTeacher : MenuBuilder
    {
        public Teacher Teacher { get; set; }

        public MenuTeacher(Teacher teacher)
        {
            Teacher = teacher;
        }
        public override void Build()
        {
            MenuItem TeacherOption1 = new AddAssignmentMenuItem("Add Assignments for Class", Teacher);
            MenuItem TeacherOption2 = new SetStudentGradeMenuItem("Set Student Grades", Teacher);
            MenuItem TeacherOption3 = new TeacherMarkStudentAttendanceMenuItem("Mark Student Attendance", Teacher);
            MenuItem TeacherOption4 = new ShowClassroomGradeMenuItem("View Grades (Read-Only Access)", Teacher);

            mainMenu.AddItem(TeacherOption1);
            mainMenu.AddItem(TeacherOption2);
            mainMenu.AddItem(TeacherOption3);
            mainMenu.AddItem(TeacherOption4);
        }
    }

    // Builder class for the student menu
    class MenuStudent : MenuBuilder
    {
        public Student Student { get; set; }

        public MenuStudent(Student student)
        {
            Student = student;
        }

        public override void Build()
        {
            MenuItem StudentOption1 = new AddSubmissionMenuItem("Submit Assignments", Student);
            MenuItem StudentOption2 = new ShowScoreMenuItem("View Grades (Read-Only Access)", Student);

            mainMenu.AddItem(StudentOption1);
            mainMenu.AddItem(StudentOption2);
        }
    }


    // Factory class to create different menus based on the role
    class MenuFactory
    {
        private RoleSchema Role { get; set; }

        public MenuFactory(RoleSchema role)
        {
            Role = role;
        }

        // Builds the appropriate menu based on the role
        public MenuBuilder Build()
        {
            if (Role.RoleType == "admin")
            {
                Admin admin = new Admin(
                    Role.Id,
                    Role.UserName,
                    Role.Password,
                    Role.RoleType,
                    Role.Active
                    );

                this.AttachLogger(admin);

                MenuAdmin menuAdmin = new MenuAdmin(admin);
                menuAdmin.Build();
                return menuAdmin;
            }
            else if (Role.RoleType == "principal")
            {
                Principal principal = new Principal(
                    Role.Id,
                    Role.UserName,
                    Role.Password,
                    Role.RoleType,
                    Role.Active
                    );

                this.AttachLogger(principal);

                MenuPrincipal menuPrincipal = new MenuPrincipal(principal);
                menuPrincipal.Build();
                return menuPrincipal;
            }
            else if (Role.RoleType == "staff")
            {
                Staff staff = new Staff(
                    Role.Id,
                    Role.UserName,
                    Role.Password,
                    Role.RoleType,
                    Role.Active
                    );

                this.AttachLogger(staff);

                MenuStaff menuStaff = new MenuStaff(staff);
                menuStaff.Build();
                return menuStaff;
            }
            else if (Role.RoleType == "teacher")
            {
                Teacher teacher = new Teacher(
                    Role.Id,
                    Role.UserName,
                    Role.Password,
                    Role.RoleType,
                    Role.Active
                    );

                this.AttachLogger(teacher);

                MenuTeacher menuTeacher = new MenuTeacher(teacher);
                menuTeacher.Build();
                return menuTeacher;
            }
            else if (Role.RoleType == "student")
            {
                Student student = new Student(
                    Role.Id,
                    Role.UserName,
                    Role.Password,
                    Role.RoleType,
                    Role.Active
                    );

                this.AttachLogger(student);

                MenuStudent menuStudent = new MenuStudent(student);
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
