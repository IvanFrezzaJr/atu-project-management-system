namespace ProjectManagementSystem.Views
{
    public class BaseView
    {
        const int MaxWidth = 40;


        public string GettextCenter(string text, int maxWidth)
        {
            int totalPadding = maxWidth - text.Length;
            int padLeft = totalPadding / 2; // Espaços à esquerda
            int padRight = totalPadding - padLeft; // Espaços à direita
            return text.PadLeft(padLeft + text.Length).PadRight(maxWidth);
        }

        public void DisplayTitle(string title)
        {
            Console.WriteLine("");
            Console.WriteLine(new string('=', MaxWidth));
            Console.WriteLine($"{GettextCenter(title, MaxWidth)}");
            Console.WriteLine(new string('=', MaxWidth));
        }

        //public string? Input(string text, string errorMessage)
        //{
        //    Console.Write($"{text}: ");
        //    string name = Console.ReadLine();
        //    if (name == "0")
        //    {
        //        return "0";
        //    }

        //    if (name == "" || name == null)
        //    {
        //        Console.WriteLine($"\n{errorMessage}\n");
        //        return null;
        //    }

        //    return name;
        //}

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
            Console.WriteLine(prompt);
            Console.Write("> ");
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
