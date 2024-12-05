using ProjectManagementSystem.Database;
using ProjectManagementSystem.Views;

namespace ProjectManagementSystem.Controllers
{
    public class AuthenticationController
    {
        private readonly UserInterface _userInterface;
        private readonly AuthRepository _authRepository;

        public string UserName { get; set; }
        public bool IsAutheticated { get; set; }

        public AuthenticationController(UserInterface userInterface, AuthRepository authRepository)
        {
            _userInterface = userInterface;
            _authRepository = authRepository;
        }

        public void AuthenticateUser()
        {
            int count = 0;
            int maxAttempts = 3;
            string password = string.Empty;

            while (true)
            {
                this.UserName = _userInterface.GetUsername();
                password = _userInterface.GetPassword();

                // Directly use the repository to validate credentials
                IsAutheticated = _authRepository.ValidateCredentials(this.UserName, password);

                _userInterface.ShowAuthenticationResult(IsAutheticated);


                if (IsAutheticated)
                {
                    break;
                }


                count++;
                _userInterface.ShowAuthenticationAttempts(count, maxAttempts);
                if (count == 3)
                {
                    Environment.Exit(0);

                }
            }
        }
    }
}