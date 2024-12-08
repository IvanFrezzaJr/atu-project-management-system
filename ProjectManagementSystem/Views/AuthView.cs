using ProjectManagementSystem;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class LoginView : BaseView
    {

        // Method to prompt the user for a password (securely)
        public string GetPasswordInput(string question)
        {
            Console.Write(question);
            return Helpers.ReadPassword();
        }


        // Method to display authentication result
        public void ShowAuthenticationResult(bool isAuthenticated)
        {
            if (isAuthenticated)
            {

                DisplayTitle("Authentication successful!\n");
            }
            else
            {
                Console.WriteLine("[WARNING] Authentication failed! Invalid username or password.\n");
            }
            
        }

        public void ShowAuthenticationAttempts(int attempts, int maxAttempts = 3)
        {
            if (attempts > maxAttempts)
            {
                throw new Exception("'maxAttempts' shouldn't be greater than 'attempts'");
            }

            if (attempts == maxAttempts)
            {
                Console.WriteLine("You have reached the maximum number of attempts. The program will be terminated.");
            }

            Console.WriteLine($"You have {maxAttempts - attempts} more attempts.");


        }
    }
}

