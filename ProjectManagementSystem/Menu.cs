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


    class Menu : BaseMenu, IMenu
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
            System.Console.WriteLine($"{this.Name} menu");

            int index = 0;

            for (int i = 0; i < this._current.Count; i++)
            {
                System.Console.WriteLine($"{index++}. {this._current[i].Name}");
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
                Console.WriteLine($"{option}");
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
                    int option = int.Parse(Console.ReadLine()); 
                    Console.WriteLine($"Input: {option}");
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
}