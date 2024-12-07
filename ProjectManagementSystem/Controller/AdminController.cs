using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminInterface _adminInterface;
        private readonly RoleRepository _roleRepository;
        private readonly LogRepository _logRepository;

        public string UserName { get; set; }
        public bool IsAutheticated { get; set; }

        public AdminController(AdminInterface userInterface, RoleRepository roleRepository, LogRepository logRepository)
        {
            _adminInterface = userInterface;
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

            this._adminInterface.ShowLogs(allLogs);
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

        private bool CreateRole(string typeRole)
        {
            while (true)
            {
                string title = CapitalizeFirstLetter(typeRole);
                string username = _adminInterface.GetValue($"{title} username: ");
                // check username is valid
                bool result = _adminInterface.ShowResult(username);
                if (!result)
                {
                    continue;
                }

                // check user already exists
                bool exists = this._roleRepository.RoleExists(username);
                result = this._adminInterface.ShowUserExistsResult(exists);
                if (result)
                {
                    continue;
                }

                // check username is valid
                string password = _adminInterface.GetValue($"{title} password: ");
                result = _adminInterface.ShowResult(username);
                if (!result)
                {
                    continue;
                }

                // insert Role in the database
                Role role = new Role(
                    username,
                    password,
                    true,
                    typeRole
                );
                this._roleRepository.AddRole(role);

                // Notify observers that the classroom already exists.
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"User {username} was created by {Session.LoggedUser.UserName}"
                }, true);

                return true;
            }
        }

        public bool ResetPassword()
        {
            while (true)
            {
                string username = _adminInterface.GetValue("Enter role username: ");
                bool exists = this._roleRepository.RoleExists(username);
                bool result = this._adminInterface.ShowUserExistsResult(exists);
                if (exists)
                {
                    return false;
                }

                string password = _adminInterface.GetValue($"Enter role password: ");

 
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

    }
}
