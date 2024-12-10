using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System;
using System.Data;
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
            Logger logger = new Logger(_logRepository);
            AddSubscriber(logger);
        }


        /// <summary>
        /// Prints the logs of all alerts received by the admin.
        /// </summary>
        public void PrintLogs()
        {
            List<Alert> allLogs = this._logRepository.GetAllLogs();

            _adminView.DisplayTitle($"Show logs");
            this._adminView.ShowLogs(allLogs);
        }

        public void CreateRole(string typeRole)
        {
            while (true)
            {
                try
                {
                    _adminView.DisplayTitle($"{typeRole} registration");
                    // get role name and make validation
                    string userName = _adminView.GetInput($"Enter user's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // check user already exists
                    ValidateRoleExists(userName);

                    // check username is valid
                    string password = _adminView.GetPasswordInput($"Enter user's password:");
                    if (password == "<EXIT>") break;
                    ValidateStringInput(password);

                    // Retrieve the factory function and use it to create the Role instance later
                    var roleFactory = GetRoleFactory(typeRole);
                    Role roleInstance = roleFactory(userName, password);

                    // persist role into the database
                    this._roleRepository.AddRole(roleInstance);

                    _adminView.DisplaySuccess($"Registration Successful!\n");
                    _adminView.DisplayUserInfo(roleInstance);
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"{Session.LoggedUser.UserName} inserted the {roleInstance.UserName} user successful."
                    });
                    // Mostra o erro e solicita novamente
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
                    });
                    // Mostra o erro e solicita novamente
                    _adminView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        public void ResetPassword()
        {
            while (true)
            {
                try { 
                    _adminView.DisplayTitle($"Perform password reset");
                    string userName = _adminView.GetValue("Enter role username: ");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // check user already exists
                    ValidateRoleNotExists(userName);

                    string password = _adminView.GetPasswordInput($"Enter user's password:");
                    if (password == "<EXIT>") break;
                    ValidateStringInput(password);

                    // reset password
                    this._roleRepository.UpdateRolePassword(userName, password);
                    _adminView.DisplaySuccess($"Password was reseted successful!\n");

                    // Notify observers that the classroom already exists.
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"{Session.LoggedUser.UserName} updated the {userName} password successful."
                    });

                    break;

                }
                catch (Exception ex) when(
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
                    });
                    // Mostra o erro e solicita novamente
                    _adminView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        private Func<string, string, Role> GetRoleFactory(string typeRole)
        {
            return typeRole switch
            {
                "student" => (userName, password) => new Student(userName, password),
                "teacher" => (userName, password) => new Teacher(userName, password),
                "staff" => (userName, password) => new Staff(userName, password),
                "principal" => (userName, password) => new Principal(userName, password),
                "admin" => (userName, password) => new Admin(userName, password),
                _ => throw new ArgumentException($"Unknown role type: {typeRole}")
            };
        }



        private void ValidateRoleExists(string userName)
        {
            if (this._roleRepository.RoleExists(userName))
            {
                throw new ApplicationException("Role already exists");
            }
        }


        private void ValidateRoleNotExists(string userName)
        {
            if (!this._roleRepository.RoleExists(userName))
            {
                throw new ApplicationException("Role already exists");
            }
        }
    }
}
