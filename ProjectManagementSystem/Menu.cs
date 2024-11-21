using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Domain.Models;
using ProjectManagementSystem.Interfaces;
using System.Drawing;


namespace ProjectManagementSystem
{

    // Abstract class for a base menu with a name
    public abstract class BaseMenuName
    {
        // Property to store the name of the menu
        public string Name { get; set; }

        // Constructor to initialize the menu name
        protected BaseMenuName(string name)
        {
            this.Name = name;
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
            System.Console.WriteLine($"--- {this.Name} ---");
            System.Console.WriteLine($"quit: 0 + Enter\n");
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
                System.Console.WriteLine($"\n{errorMessage}\n");
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
            this._current = this._current
               .Concat(this._items.Except(this._current))
               .Concat(this._submenus.Except(this._current))
               .ToList();
        }

        // Adds a menu item to the current menu
        public void AddItem(MenuItem item)
        {
            this._items.Add(item);
            this.UpdateCurrent();
        }

        // Adds a submenu to the current menu
        public void AddSubMenu(Menu submenu)
        {
            submenu.parent = this;
            this._submenus.Add(submenu);
            this.UpdateCurrent();
        }

        // Displays the available options in the current menu
        public void DisplayOptions()
        {
            System.Console.WriteLine($"\n--- {this.Name} menu ---\n");

            int index = 0;

            // Loop through current menu items and display them with an index
            for (int i = 0; i < this._current.Count; i++)
            {
                index++;
                System.Console.WriteLine($"{index}. {this._current[i].Name}");
            }

            // Show the back option or exit option based on the presence of a parent menu
            if (this.parent is not null)
            {
                System.Console.WriteLine("0. Go Back");
            }
            else
            {
                System.Console.WriteLine("0. Exit");
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
                if (option >= 1 && option <= this._current.Count)
                {
                    var item = this._current[option - 1];

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
                this.DisplayOptions();  // Display current menu options
                try
                {
                    // Prompt the user to input a number
                    Console.Write("\nInput a number: ");
                    string optionString = Console.ReadLine();
                    int option = int.Parse(optionString.Trim());

                    Console.Clear();  // Clear the console for a fresh display
                    bool result = this.HandleChoice(option);  // Handle the choice
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
            this.mainMenu.Show();
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
            this.Admin = admin;
        }

        public override void Build()
        {
            MenuItem ITOption1 = new CreatePrincipalMenuItem("Create Principal User", this.Admin);
            MenuItem ITOption2 = new CreateStaffMenuItem("Create Staff User", this.Admin);
            MenuItem ITOption3 = new CreateTeacherMenuItem("Create Teacher User", this.Admin);
            MenuItem ITOption4 = new CreateStudentMenuItem("Create Student User", this.Admin);
            MenuItem ITOption5 = new ResetPasswordMenuItem("Perform Password Reset for Users", this.Admin);
            MenuItem ITOption6 = new ShowLogsMenuItem("Show logs", this.Admin);

            this.mainMenu.AddItem(ITOption1);
            this.mainMenu.AddItem(ITOption2);
            this.mainMenu.AddItem(ITOption3);
            this.mainMenu.AddItem(ITOption4);
            this.mainMenu.AddItem(ITOption5);
            this.mainMenu.AddItem(ITOption6);
        }
    }

    // Builder class for the principal menu
    class MenuPrincipal : MenuBuilder
    {
        public Principal Principal { get; set; }

        public MenuPrincipal(Principal principal)
        {
            this.Principal = principal;
        }

        public override void Build()
        {
            MenuItem PrincipalOption1 = new AssignTeacherToClassroomMenuItem("Assign Teacher to Classroom", this.Principal);
            MenuItem PrincipalOption2 = new ActiveRoleMenuItem("Enable/Disable User Accounts", this.Principal);
            MenuItem PrincipalOption3 = new ShowGradeByClassroomMenuItem("View Grades (Read-Only Access)", this.Principal);

            this.mainMenu.AddItem(PrincipalOption1);
            this.mainMenu.AddItem(PrincipalOption2);
            this.mainMenu.AddItem(PrincipalOption3);
        }
    }

    // Builder class for the staff menu
    class MenuStaff : MenuBuilder
    {
        public Staff Staff { get; set; }

        public MenuStaff(Staff staff)
        {
            this.Staff = staff;
        }

        public override void Build()
        {
            MenuItem StaffOption1 = new CreateClassroomMenuItem("Create New Classroom", this.Staff);
            MenuItem StaffOption2 = new AssignStudentToClassroomMenuItem("Assign Student to Classroom", this.Staff);
            MenuItem StaffOption3 = new StaffMarkStudentAttendanceMenuItem("Mark Student Attendance", this.Staff);

            this.mainMenu.AddItem(StaffOption1);
            this.mainMenu.AddItem(StaffOption2);
            this.mainMenu.AddItem(StaffOption3);
        }
    }

    // Builder class for the teacher menu
    class MenuTeacher : MenuBuilder
    {
        public Teacher Teacher { get; set; }

        public MenuTeacher(Teacher teacher)
        {
            this.Teacher = teacher;
        }
        public override void Build()
        {
            MenuItem TeacherOption1 = new AddAssignmentMenuItem("Add Assignments for Class", this.Teacher);
            MenuItem TeacherOption2 = new SetStudentGradeMenuItem("Set Student Grades", this.Teacher);
            MenuItem TeacherOption3 = new TeacherMarkStudentAttendanceMenuItem("Mark Student Attendance", this.Teacher);
            MenuItem TeacherOption4 = new ShowClassroomGradeMenuItem("View Grades (Read-Only Access)", this.Teacher);

            this.mainMenu.AddItem(TeacherOption1);
            this.mainMenu.AddItem(TeacherOption2);
            this.mainMenu.AddItem(TeacherOption3);
            this.mainMenu.AddItem(TeacherOption4);
        }
    }

    // Builder class for the student menu
    class MenuStudent : MenuBuilder
    {
        public Student Student { get; set; }

        public MenuStudent(Student student)
        {
            this.Student = student;
        }

        public override void Build()
        {
            MenuItem StudentOption1 = new AddSubmissionMenuItem("Submit Assignments", this.Student);
            MenuItem StudentOption2 = new ShowScoreMenuItem("View Grades (Read-Only Access)", this.Student);

            this.mainMenu.AddItem(StudentOption1);
            this.mainMenu.AddItem(StudentOption2);
        }
    }


    // Factory class to create different menus based on the role
    class MenuFactory
    {
        private RoleSchema Role { get; set; }

        public MenuFactory(RoleSchema role)
        {
            this.Role = role;
        }

        // Builds the appropriate menu based on the role
        public MenuBuilder Build()
        {
            if (this.Role.RoleType == "admin")
            {
                Admin admin = new Admin(
                    this.Role.Id,
                    this.Role.UserName,
                    this.Role.Password,
                    this.Role.RoleType,
                    this.Role.Active
                    );

                this.AttachLogger(admin);

                MenuAdmin menuAdmin = new MenuAdmin(admin);
                menuAdmin.Build();
                return menuAdmin;
            }
            else if (this.Role.RoleType == "principal")
            {
                Principal principal = new Principal(
                    this.Role.Id,
                    this.Role.UserName,
                    this.Role.Password,
                    this.Role.RoleType,
                    this.Role.Active
                    );

                this.AttachLogger(principal); 

                MenuPrincipal menuPrincipal = new MenuPrincipal(principal);
                menuPrincipal.Build();
                return menuPrincipal;
            }
            else if (this.Role.RoleType == "staff")
            {
                Staff staff = new Staff(
                    this.Role.Id,
                    this.Role.UserName,
                    this.Role.Password,
                    this.Role.RoleType,
                    this.Role.Active
                    );

                this.AttachLogger(staff);

                MenuStaff menuStaff = new MenuStaff(staff);
                menuStaff.Build();
                return menuStaff;
            }
            else if (this.Role.RoleType == "teacher")
            {
                Teacher teacher = new Teacher(
                    this.Role.Id,
                    this.Role.UserName,
                    this.Role.Password,
                    this.Role.RoleType,
                    this.Role.Active
                    );

                this.AttachLogger(teacher);

                MenuTeacher menuTeacher = new MenuTeacher(teacher);
                menuTeacher.Build();
                return menuTeacher;
            }
            else if (this.Role.RoleType == "student")
            {
                Student student = new Student(
                    this.Role.Id,
                    this.Role.UserName,
                    this.Role.Password,
                    this.Role.RoleType,
                    this.Role.Active
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
