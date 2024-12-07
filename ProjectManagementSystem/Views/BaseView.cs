namespace ProjectManagementSystem.Views
{
    public class BaseView
    {
        public string? Input(string text, string errorMessage)
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

        public bool CheckExit(string input)
        {
            if (input == "0")
            {
                return true;
            }

            return false;
        }

        public string GetInput(string prompt)
        {
            Console.Write(prompt + " > ");
            string text =  Console.ReadLine();

            if (CheckExit(text)){
                return "<EXIT>";
            }

            return text;
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }
    }
}
