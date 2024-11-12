namespace ProjectManagementSystem
{


    interface IMenuItem
    {
        abstract void Execute();
    }

    interface IMenu
    {
        void Show();
        void AddItem(IMenuItem item);
        void AddSubMenu(Menu submenu);
    }


    public abstract class MenuItem : IMenuItem
    {
        string _name = string.Empty;

        public MenuItem(string name)
        {
            this._name = name;
        }

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


    class Menu : IMenu
    {
        string _title = string.Empty;
        List<IMenuItem> _items = new List<IMenuItem>();
        List<IMenu> _submenus = new List<IMenu>();
        List<object> _current = new List<object>();
        Menu? parent = null;

        public Menu(string title)
        {
            this._title = title;
        }

        public void AddItem(IMenuItem item)
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
            for (int i = 0; i < this._current.Count; i++)
            {
                System.Console.WriteLine($"{this._current[i]}");
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
                try
                {
                    int option = int.Parse(Console.ReadLine());  // Lê a entrada e converte para int
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

        private void UpdateCurrent()
        {
            this._current.AddRange(this._items);
            this._current.AddRange(this._submenus);
        }

    }
}