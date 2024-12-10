using ProjectManagementSystem;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Views;

namespace ProjectManagementSystem.Controllers
{
    public class AuthenticationController
    {
        private readonly LoginView _loginView;
        private readonly AuthRepository _authRepository;

        private string _userName;
        private bool _isAutheticated;

        public AuthenticationController(LoginView loginView, AuthRepository authRepository)
        {
            _loginView = loginView;
            _authRepository = authRepository;
            _isAutheticated = false;
            _userName = "";
        }

        public void AuthenticateUser()
        {
            int count = 0;
            int maxAttempts = 3;

            while (true)
            {
                _loginView.DisplayMessage("Press 0 + Enter to exit.");
                _loginView.DisplayTitle("LOGIN SYSTEM");

                _userName = _loginView.GetInput("Enter username:");
                CheckExit(_userName);

                string password = _loginView.GetPasswordInput($"Enter password:");
                CheckExit(password);

                // Directly use the repository to validate credentials
                _isAutheticated = _authRepository.ValidateCredentials(_userName, password);

                _loginView.ShowAuthenticationResult(_isAutheticated);


                if (_isAutheticated)
                {
                    break;
                }


                count++;
                _loginView.ShowAuthenticationAttempts(count, maxAttempts);
                if (count == 3)
                {
                    Environment.Exit(0);

                }
            }
        }

        public string GetLoggedUserName()
        {
            return _userName;
        }

        public bool IsAuthenticated()
        {
            return _isAutheticated;
        }

        private void CheckExit(string input)
        {
            if (input == "<EXIT>"){
                _loginView.DisplayTitle("\nBYE BYE!\n");
                Environment.Exit(0);
            }
        }
    }
}