namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class AdminInterface : BaseView
    {
        // Method to prompt the user for a username
        public string GetValue(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        public bool ShowResult(string value)
        { 
            if (value == "" || value == null)
            {
                Console.WriteLine("\nPlease, enter a valid value\n");
                return false;
            }

            return true;
        }

        public bool ShowUserExistsResult(bool isCreated)
        {
            if (isCreated) { 
                System.Console.WriteLine("\nUser already exists\n");
                return false;
            }
            else
            {
              return true;
            }

        }

        public void ShowLogs(List<Alert> logs)
        {
            Console.WriteLine($"\nSystem logs:\n");

            foreach (var log in logs)
            {
                Console.WriteLine($"[{log.CreatedAt}] - '{log.Role}'.{log.Action}: {log.Message}");
            }
        }



       
    }
}

