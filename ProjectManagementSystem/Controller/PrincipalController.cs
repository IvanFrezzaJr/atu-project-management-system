using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class PrincipalController : BaseController
    {
        private readonly PrincipalView _principalView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;
        private readonly LogRepository _logRepository;

        public PrincipalController(PrincipalView principalView, RoleRepository roleRepository, ClassroomRepository classroomRepository, LogRepository logRepository)
        {
            _principalView = principalView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;
            _logRepository = logRepository;

            // add logger
            Logger logger = new Logger(_logRepository);
            AddSubscriber(logger);
        }

        public void AssignRoleToClassroom()
        {
            //string classroom, string role, string typeRole
            while (true)
            {
                try
                {
                    _principalView.DisplayTitle("Assign Role To Classroom");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _principalView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroom = _principalView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // show students
                    List<Role> roles = _classroomRepository.GetAllRoles("teacher");
                    _principalView.DisplayRoles(roles);

                    // get student name and make validation
                    string userName = _principalView.GetInput("Enter Teacher's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, "Role not found");

                    // Check permissions
                    List<string> roleList = new List<string> { "teacher"};
                    ValidatePermission(roleResult.RoleType, roleList);

                    // get classrom and validate it
                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomResult, "Classroom not found");
                    ValidateEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);

                    // add role to classroom
                    this._classroomRepository.AddEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);
                    _principalView.DisplayTitle($"Enrollment added successful");
                    _principalView.DisplayEnrollmentInfo(roleResult, classroomResult);

                    // Notify observers that the classroom already exists.
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"Added role '{roleResult.RoleType}' to classroom  {classroomResult.Name}"
                    });

                    break;
                }
                catch (Exception ex) when (
                    ex is ArgumentException ||
                    ex is AccessViolationException ||
                    ex is ArgumentNullException ||
                    ex is SQLiteException ||
                    ex is ApplicationException)
                {
                    this.NotifyObservers(new Alert
                    {
                        Role = this.GetType().Name,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = ex.Message
                    });
                    // Mostra o erro e solicita novamente
                    _principalView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        public void ToggleRole()
        {
            while (true)
            {
                try
                {
                    _principalView.DisplayTitle("Enable/Disable Role");
                    // get classroom name and make validation
                    string userName = _principalView.GetInput("Enter User's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);


                    // get role from database
                    Role role = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(role, "Role not found");

                    // toggle role 
                    bool active = (role.Active) ? false : true;
                    bool status = this._roleRepository.ActivateRole(userName, active);
                    _principalView.DisplayMessage($"User {role.UserName}: set activate {active}");
                    
                    break;
                }
                catch (Exception ex) when (
                    ex is ArgumentException ||
                    ex is AccessViolationException ||
                    ex is ArgumentNullException ||
                    ex is ApplicationException)
                {
                    this.NotifyObservers(new Alert
                    {
                        Role = this.GetType().Name,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = ex.Message
                    });
                    // Mostra o erro e solicita novamente
                    _principalView.DisplayError(ex.Message);
                }
                continue;
            }
        }

        public void DisplayStudentSubmissions()
        {
            while (true)
            {
                try
                {
                    _principalView.DisplayTitle("Display Student Submissions");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _principalView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroomName = _principalView.GetInput("Enter Classroom's name:");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroomName);  // TODO: move Classroom to Classroom
                    ValidateObjectInstance(classroomResult, "Classroom not found");

                    _principalView.DisplayMessage($"\nSubmissions for Classroom: {classroomName}\n");

                    List<dynamic> studentSubmissions = this._roleRepository.GetStudentSubmissions(classroomName);
                    _principalView.ShowSubmissionsResult(studentSubmissions);
                    break;
                }
                catch (Exception ex) when (
                    ex is ArgumentException ||
                    ex is AccessViolationException ||
                    ex is ArgumentNullException ||
                    ex is ApplicationException)
                {
                    this.NotifyObservers(new Alert
                    {
                        Role = this.GetType().Name,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = ex.Message
                    });
                    // Mostra o erro e solicita novamente
                    _principalView.DisplayError(ex.Message);
                }
                continue;
            }

        }


        private void ValidateEnrollment(int classroomId, int roleId, string roletype)
        {
            if (_classroomRepository.EnrollmentExists(classroomId, roleId, roletype))
            {
                throw new ApplicationException("The enrollment for this user already exists");
            }
        }

        private void ValidateNoEnrollment(int classroomId, int roleId, string roletype)
        {
            if (!_classroomRepository.EnrollmentExists(classroomId, roleId, roletype))
            {
                throw new ApplicationException("Enrollment not exists");
            }
        }


    }
}
