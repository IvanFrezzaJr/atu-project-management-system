using ProjectManagementSystem;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Views;

namespace ProjectManagementSystem.Controllers
{
    public class AuthenticationController
    {
        private readonly LoginView _loginView;
        private readonly AuthRepository _authRepository;

        public string UserName { get; set; }
        public bool IsAutheticated { get; set; }

        public AuthenticationController(LoginView loginView, AuthRepository authRepository)
        {
            _loginView = loginView;
            _authRepository = authRepository;
        }

        public void AuthenticateUser()
        {
            int count = 0;
            int maxAttempts = 3;

            while (true)
            {
                _loginView.DisplayTitle("LOGIN SYSTEM");
                UserName = _loginView.GetInput("Enter username:");
                string password = _loginView.GetPasswordInput($"Enter password:");

                // Directly use the repository to validate credentials
                IsAutheticated = _authRepository.ValidateCredentials(this.UserName, password);

                _loginView.ShowAuthenticationResult(IsAutheticated);


                if (IsAutheticated)
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
    }
}