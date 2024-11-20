using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Domain.Models;


/* how to create a menu

    MenuItem option1 = new Option1MenuItem("Option 1");
    MenuItem option2 = new Option2MenuItem("Option 2");
    MenuItem option3 = new Option3MenuItem("Option 3");

    //create menus and submenus
    Menu mainMenu = new Menu("Main menu");
    mainMenu.AddItem(option1);
    mainMenu.AddItem(option2);

    // example of submenu
    Menu submenu = new Menu("Submenu example");
    submenu.AddItem(option1);
    submenu.AddItem(option2);

    // add submenu to main menu
    mainMenu.AddSubMenu(submenu);
    mainMenu.AddItem(option3);

    // show main menu
    mainMenu.Show();

 */

/* how to instantiate a menu
 * 
    MenuStudent menuStudent = new MenuStudent();
    menuStudent.Show();

 */

namespace ProjectManagementSystem
{
    interface IMenuItem
    {
        // Abstract method to execute the menu item
        abstract void Execute();
    }

    interface IMenu
    {
        // Method to display the menu options
        void Show();

        // Method to add a menu item to the menu
        void AddItem(MenuItem item);

        // Method to add a submenu to the menu
        void AddSubMenu(Menu submenu);
    }

    // Abstract class for a base menu with a name
    public abstract class BaseMenu
    {
        public string Name { get; set; }

        protected BaseMenu(string name)
        {
            this.Name = name;
        }
    }

    // Abstract class for a menu item, which extends from BaseMenu and implements IMenuItem
    public abstract class MenuItem : BaseMenu, IMenuItem
    {
        protected MenuItem(string name) : base(name) { }

        // Abstract method to execute the menu item
        public virtual void Execute()
        {
            System.Console.WriteLine($"--- {this.Name} ---");
            System.Console.WriteLine($"quit: 0 + Enter\n");
        }

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
    public class Menu : BaseMenu, IMenu
    {
        List<MenuItem> _items = new List<MenuItem>();
        List<Menu> _submenus = new List<Menu>();
        List<BaseMenu> _current = new List<BaseMenu>();
        Menu? parent = null;

        public Menu(string name) : base(name) { }

        // Updates the current list of menu items and submenus
        private void UpdateCurrent()
        {
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
            if (option == 0)
            {
                return false;
            }
            else
            {
                if (option >= 1 && option <= this._current.Count)
                {
                    var item = this._current[option - 1];

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
                this.DisplayOptions();
                try
                {
                    Console.Write("\nInput a number: ");
                    int option = int.Parse(Console.ReadLine());
                    Console.Clear();
                    bool result = this.HandleChoice(option);
                    if (!result)
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
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
            this.mainMenu.AddItem(ITOption1);
            this.mainMenu.AddItem(ITOption2);
            this.mainMenu.AddItem(ITOption3);
            this.mainMenu.AddItem(ITOption4);
            this.mainMenu.AddItem(ITOption5);
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
            MenuItem PrincipalOption3 = new Option1MenuItem("View Grades (Read-Only Access)");

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
            MenuItem TeacherOption2 = new Option1MenuItem("Set Student Grades");
            MenuItem TeacherOption3 = new TeacherMarkStudentAttendanceMenuItem("Mark Student Attendance", this.Teacher);

            this.mainMenu.AddItem(TeacherOption1);
            this.mainMenu.AddItem(TeacherOption2);
            this.mainMenu.AddItem(TeacherOption3);
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
            MenuItem StudentOption2 = new Option1MenuItem("View Grades (Read-Only Access)");

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

                MenuStudent menuStudent = new MenuStudent(student);
                menuStudent.Build();
                return menuStudent;
            }
            else
            {
                throw new ArgumentException("Role type not found");
            }
        }
    }
}
