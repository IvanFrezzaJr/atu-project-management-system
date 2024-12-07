using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminView _adminView;
        private readonly RoleRepository _roleRepository;
        private readonly LogRepository _logRepository;

        public AdminController(AdminView userInterface, RoleRepository roleRepository, LogRepository logRepository)
        {
            _adminView = userInterface;
            _roleRepository = roleRepository;
            _logRepository = logRepository;

            // add logger
            Logger logger = new Logger();
            AddSubscriber(logger);
        }


        /// <summary>
        /// Prints the logs of all alerts received by the admin.
        /// </summary>
        public void PrintLogs()
        {
            List<Alert> allLogs = this._logRepository.GetAllLogs();

            this._adminView.ShowLogs(allLogs);
        }

        public void CreatePrincipal()
        {
            CreateRole("principal");
        }

        public void CreateStaff()
        {
            CreateRole("staff");
        }

        public void CreateTeacher()
        {
            CreateRole("teacher");
        }


        public void CreateStudent()
        {
            CreateRole("student");
        }

        private void CreateRole(string typeRole)
        {
            while (true)
            {
                try
                {
                    // get teacher name and make validation
                    string title = CapitalizeFirstLetter(typeRole);
                    string userName = _adminView.GetInput($"What is the {title} name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // check user already exists
                    ValidateRoleExists(userName);


                    // check username is valid
                    string password = _adminView.GetPasswordInput($"{title} password: ");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // insert Role in the database
                    Role role = new Role(
                        userName,
                        password,
                        true,
                        typeRole
                    );


                    this._roleRepository.AddRole(role);
                    _adminView.DisplayMessage($"User {userName} was created by {Session.LoggedUser.UserName}");

                    break;
                }
                catch (Exception ex) when (
                    ex is SQLiteException ||
                    ex is ArgumentException ||
                    ex is AccessViolationException ||
                    ex is ArgumentNullException ||
                    ex is ApplicationException)
                {
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = ex.Message
                    }, false);
                    // Mostra o erro e solicita novamente
                    _adminView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        public bool ResetPassword()
        {
            while (true)
            {
                string username = _adminView.GetValue("Enter role username: ");
                bool exists = this._roleRepository.RoleExists(username);
                bool result = this._adminView.ShowUserExistsResult(exists);
                if (exists)
                {
                    return false;
                }

                string password = _adminView.GetValue($"Enter role password: ");

 
                this._roleRepository.UpdateRolePassword(username, password);
     

                // Notify observers that the classroom already exists.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"{Session.LoggedUser.UserName} updated the {username} password successful."
                }, true);

                return true;
            }
        }


        private void ValidateRoleExists(string userName)
        {
            if (this._roleRepository.RoleExists(userName))
            {
                throw new ApplicationException("Role already exists");
            }
        }
    }
}
