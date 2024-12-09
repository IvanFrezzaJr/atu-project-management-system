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

                    // insert Role in the database
                    Role roleInstance = new Role(
                        userName,
                        password,
                        true,
                        typeRole
                    );

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
