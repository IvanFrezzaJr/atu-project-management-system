using ProjectManagementSystem;

namespace ProjectManagementSystem
{


    interface IMenuItem
    {
        abstract void Execute();
    }

    interface IMenu
    {
        void Show();
        void AddItem(MenuItem item);
        void AddSubMenu(Menu submenu);
    }


    public abstract class BaseMenu
    {
        public string Name { get; set; }

        protected BaseMenu(string name)
        {
            this.Name = name;
        }
    }


    public abstract class MenuItem : BaseMenu, IMenuItem
    {
        protected MenuItem(string name) : base(name) { }

        public abstract void Execute();
    }


    public class Option1MenuItem : MenuItem
    {
        public Option1MenuItem(string name) : base(name) { }

        public override void Execute()
        {
            System.Console.WriteLine("Option1MenuItem");
        }
    }

   
    public class Option2MenuItem : MenuItem
    {
        public Option2MenuItem(string name) : base(name) { }

        public override void Execute()
        {
            System.Console.WriteLine("Option2MenuItem");
        }
    }


    public class Option3MenuItem : MenuItem
    {
        public Option3MenuItem(string name) : base(name) { }

        public override void Execute()
        {
            System.Console.WriteLine("Option3MenuItem");
        }
    }


    public class Menu : BaseMenu, IMenu
    {
        List<MenuItem> _items = new List<MenuItem>();
        List<Menu> _submenus = new List<Menu>();
        List<BaseMenu> _current = new List<BaseMenu>();
        Menu? parent = null;

        public Menu(string name) : base(name) { }


        private void UpdateCurrent()
        {
            this._current = this._current
           .Concat(this._items.Except(this._current))   
           .Concat(this._submenus.Except(this._current))
           .ToList();
        }

        public void AddItem(MenuItem item)
        {
            this._items.Add(item);
            this.UpdateCurrent();

        }

        public void AddSubMenu(Menu submenu)
        {
            submenu.parent = this;
            this._submenus.Add(submenu);
            this.UpdateCurrent();

        }

        public void DisplayOptions()
        {
            System.Console.WriteLine($"\n--- {this.Name} menu ---\n");

            int index = 0;

            for (int i = 0; i < this._current.Count; i++)
            {
                index++;
                System.Console.WriteLine($"{index}. {this._current[i].Name}");
            }


            if (this.parent is not null)
            {
                System.Console.WriteLine("0. Go Back");
            } else
            {
                System.Console.WriteLine("0. Exit");
            }
        }

        public bool HandleChoice(int option)
        {
            if (option == 0)
            {
                return false;
            } else
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
                    if (!result) {
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


    public abstract class MenuBuilder
    {
        protected Menu mainMenu = new Menu("Main");

        public MenuBuilder()
        {
            this.Build();
        }

        public void Show()
        {
            this.mainMenu.Show();
        }

        public abstract void Build();
    }


    class MenuIT : MenuBuilder
    {
        public override void Build()
        {
            MenuItem ITOption1 = new Option1MenuItem("Create Principal User");
            MenuItem ITOption2 = new Option2MenuItem("Perform Password Reset for Users");
            this.mainMenu.AddItem(ITOption1);
            this.mainMenu.AddItem(ITOption2);
        }
    }


    class MenuPrincipal : MenuBuilder
    {
        public override void Build()
        {;
            MenuItem Principalption1 = new Option1MenuItem("Add New Student to Class");
            MenuItem Principalption2 = new Option1MenuItem("Add Office Secretary to Class");
            MenuItem Principalption3 = new Option1MenuItem(" Add Teacher to Class");
            MenuItem Principalption4 = new Option1MenuItem("Enable/Disable User Accounts");
            MenuItem Principalption5 = new Option1MenuItem("view Grades (Read-Only Access)");

            this.mainMenu.AddItem(Principalption1);
            this.mainMenu.AddItem(Principalption2);
            this.mainMenu.AddItem(Principalption3);
            this.mainMenu.AddItem(Principalption4);
            this.mainMenu.AddItem(Principalption5);
        }
    }


    class MenuOfficeSecretary : MenuBuilder
    {

        public override void Build()
        {
            MenuItem OfficeSecretaryOption1 = new Option1MenuItem("Create New Classroom");
            MenuItem OfficeSecretaryOption2 = new Option1MenuItem("Assign Teacher to Classroom");
            MenuItem OfficeSecretaryOption3 = new Option1MenuItem("Assign Student to Classroom");
            MenuItem OfficeSecretaryOption4 = new Option1MenuItem("Mark Student Attendance");

            this.mainMenu.AddItem(OfficeSecretaryOption1);
            this.mainMenu.AddItem(OfficeSecretaryOption2);
            this.mainMenu.AddItem(OfficeSecretaryOption3);
            this.mainMenu.AddItem(OfficeSecretaryOption4);
        }
    }


    class MenuTeacher : MenuBuilder
    {
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


    class MenuStudent : MenuBuilder
    {
        public override void Build()
        {
            MenuItem StudentOption1 = new Option1MenuItem("Submit Assignments/Work");
            MenuItem StudentOption2 = new Option1MenuItem("View Grades (Read-Only Access)");

            this.mainMenu.AddItem(StudentOption1);
            this.mainMenu.AddItem(StudentOption2);
        }
    }


}