using ProjectManagementSystem.Models;
using System.Data.Entity.Core.Metadata.Edm;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class AdminView : BaseView
    {
        // Method to prompt the user for a username
        public string GetValue(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        public string GetPasswordInput(string question)
        {
            Console.Write(question);
            return Helpers.ReadPassword();
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
            foreach (var log in logs)
            {
                Console.WriteLine($"[{log.CreatedAt}] - '{log.Role}'.{log.Action}: {log.Message}");
            }
        }

        public void DisplayUserInfo(Role role)
        {
            Console.WriteLine($"UserName: {role.UserName}");
            Console.WriteLine($"Password: ****** (secure)");
            Console.WriteLine($"Role: {role.RoleType}");
            Console.WriteLine($"Active: {role.Active}\n");
        }
       
    }
}

