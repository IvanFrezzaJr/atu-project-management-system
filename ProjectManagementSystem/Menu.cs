using ProjectManagementSystem.Domain.Models;
using System.Data.SQLite;


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
        public abstract void Execute();
    }

    // Class representing the first menu option
    public class Option1MenuItem : MenuItem
    {
        public Option1MenuItem(string name) : base(name) { }

        // Executes the action for Option 1
        public override void Execute()
        {
            System.Console.WriteLine("Option1MenuItem");
        }
    }

    // Class representing the second menu option
    public class Option2MenuItem : MenuItem
    {
        public Option2MenuItem(string name) : base(name) { }

        // Executes the action for Option 2
        public override void Execute()
        {
            System.Console.WriteLine("Option2MenuItem");
        }
    }

    // Class representing the third menu option
    public class Option3MenuItem : MenuItem
    {
        public Option3MenuItem(string name) : base(name) { }

        // Executes the action for Option 3
        public override void Execute()
        {
            System.Console.WriteLine("Option3MenuItem");
        }
    }


    // Class representing the third menu option
    public class CreatePrincipalMenuItem : MenuItem
    {
        public Admin Admin { get; set; }

        public CreatePrincipalMenuItem(string name, Admin admin) : base(name) 
        {
            Admin = admin;
        }

        // Executes the action for Option 3
        public override void Execute()
        {
            System.Console.WriteLine("Init - CreatePrincipalMenuItem");

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            this.Admin.CreatePrincipal();

            System.Console.WriteLine("End - CreatePrincipalMenuItem");

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
        public Admin _Admin { get; set; }

        public MenuAdmin(Admin admin)
        {
            this._Admin = admin;
        }

        public override void Build()
        {
            MenuItem ITOption1 = new CreatePrincipalMenuItem("Create Principal User", this._Admin);
            MenuItem ITOption2 = new Option2MenuItem("Perform Password Reset for Users");
            this.mainMenu.AddItem(ITOption1);
            this.mainMenu.AddItem(ITOption2);
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
            MenuItem PrincipalOption1 = new Option1MenuItem("Add New Student to Class");
            MenuItem PrincipalOption2 = new Option1MenuItem("Add Office Secretary to Class");
            MenuItem PrincipalOption3 = new Option1MenuItem("Add Teacher to Class");
            MenuItem PrincipalOption4 = new Option1MenuItem("Enable/Disable User Accounts");
            MenuItem PrincipalOption5 = new Option1MenuItem("View Grades (Read-Only Access)");

            this.mainMenu.AddItem(PrincipalOption1);
            this.mainMenu.AddItem(PrincipalOption2);
            this.mainMenu.AddItem(PrincipalOption3);
            this.mainMenu.AddItem(PrincipalOption4);
            this.mainMenu.AddItem(PrincipalOption5);
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
            MenuItem StaffOption1 = new Option1MenuItem("Create New Classroom");
            MenuItem StaffOption2 = new Option1MenuItem("Assign Teacher to Classroom");
            MenuItem StaffOption3 = new Option1MenuItem("Assign Student to Classroom");
            MenuItem StaffOption4 = new Option1MenuItem("Mark Student Attendance");

            this.mainMenu.AddItem(StaffOption1);
            this.mainMenu.AddItem(StaffOption2);
            this.mainMenu.AddItem(StaffOption3);
            this.mainMenu.AddItem(StaffOption4);
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
            MenuItem TeacherOption1 = new Option1MenuItem("Update Class Information");
            MenuItem TeacherOption2 = new Option1MenuItem("Add/Remove Content for Class");
            MenuItem TeacherOption3 = new Option1MenuItem("Set Student Grades");
            MenuItem TeacherOption4 = new Option1MenuItem("Mark Student Attendance");

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
            MenuItem StudentOption1 = new Option1MenuItem("Submit Assignments/Work");
            MenuItem StudentOption2 = new Option1MenuItem("View Grades (Read-Only Access)");

            this.mainMenu.AddItem(StudentOption1);
            this.mainMenu.AddItem(StudentOption2);
        }
    }

    // Factory class to create different menus based on the role
    class MenuFactory
    {
        private UserModel UserModel { get; set; }

        public MenuFactory(UserModel userModel)
        {
            this.UserModel = userModel;
        }

        // Builds the appropriate menu based on the role
        public MenuBuilder Build()
        {
            if (this.UserModel.RoleType == "admin")
            {
                Admin admin = new Admin(
                    this.UserModel.Id, 
                    this.UserModel.UserName, 
                    this.UserModel.Password, 
                    this.UserModel.RoleType
                );

                MenuAdmin menuAdmin = new MenuAdmin(admin);
                menuAdmin.Build();
                return menuAdmin;
            }
            //else if (this.UserModel.RoleType == "principal")
            //{
            //    Principal principal = new Principal(
            //        this.UserModel.Id,
            //        this.UserModel.UserName,
            //        this.UserModel.Password,
            //        this.UserModel.RoleType
            //    );

            //    MenuPrincipal menuPrincipal = new MenuPrincipal(principal);
            //    menuPrincipal.Build();
            //    return menuPrincipal;
            //}
            //else if (this.UserModel.RoleType == "staff")
            //{
            //    Staff staff = new Staff(
            //        this.UserModel.Id,
            //        this.UserModel.UserName,
            //        this.UserModel.Password,
            //        this.UserModel.RoleType
            //    );

            //    MenuStaff menuStaff = new MenuStaff(staff);
            //    menuStaff.Build();
            //    return menuStaff;
            //}
            else if (this.UserModel.RoleType == "teacher")
            {
                Teacher teacher = new Teacher(
                    this.UserModel.Id,
                    this.UserModel.UserName,
                    this.UserModel.Password,
                    this.UserModel.RoleType
                );

                MenuTeacher menuTeacher = new MenuTeacher(teacher);
                menuTeacher.Build();
                return menuTeacher;
            }
            else if (this.UserModel.RoleType == "student")
            {
                Student student = new Student(
                    this.UserModel.Id,
                    this.UserModel.UserName,
                    this.UserModel.Password,
                    this.UserModel.RoleType
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
