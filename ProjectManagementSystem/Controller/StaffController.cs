using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class StaffController : BaseController
    {
        private readonly StaffView _staffView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;

        public string UserName { get; set; }
        public bool IsAutheticated { get; set; }

        public StaffController(StaffView staffView, RoleRepository roleRepository, ClassroomRepository classroomRepository)
        {
            _staffView = staffView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;

            // add logger
            Logger logger = new Logger();
            AddSubscriber(logger);
        }

        public void CreateClassroom()
        {
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>")
                        break;

                    ValidateStringInput(classroom);


                    bool status = _classroomRepository.ClassroomExists(classroom);
                    if (status)
                    {
                        _staffView.DisplayMessage($"Classroom {classroom} already exists");
                        continue;
                    }

                    this._classroomRepository.InsertClassroom(classroom);
                    _staffView.DisplayMessage($"Classroom {classroom} added successfully");
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
                    }, true);
                    // Mostra o erro e solicita novamente
                    _staffView.DisplayError(ex.Message);
                }

                continue;
            }
            
        }


        public void AssignRoleToClassroom()
        {
            //string classroom, string role, string typeRole
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);  
                    

                    // get teacher name and make validation
                    string userName = _staffView.GetInput("What is Student name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);


                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidatePermission(roleResult.RoleType);

                    ClassroomSchema classroomResult = this._classroomRepository.GetClassroomByName(classroom);  // TODO: move ClassroomSchema to Classroom
                    if (classroomResult == null)
                    { 
                        _staffView.DisplayMessage($"'{classroom}' classroom not found");
                        continue;
                    }

                    this._classroomRepository.AddRoleToClassroom(classroomResult.Id, roleResult.Id, roleResult.RoleType);
                    _staffView.DisplayMessage($"Added role '{roleResult.RoleType}' to classroom  {classroomResult.Name}");
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
                    }, true);
                    // Mostra o erro e solicita novamente
                    _staffView.DisplayError(ex.Message);
                }

                continue;
            }
        }


        public void MarkStudentAttendance()
        {
            //string classroom, string role, string typeRole
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);


                    // get teacher name and make validation
                    string userName = _staffView.GetInput("What is Student name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);


                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, $"'User {userName}' not found");

                    ClassroomSchema classroomResult = this._classroomRepository.GetClassroomByName(classroom);  // TODO: move ClassroomSchema to Classroom
                    int? enrollmentId = this._classroomRepository.GetEnrollmentId(classroomResult.Id, roleResult.Id);
                    if (enrollmentId == null)
                    {
                        _staffView.DisplayMessage("Student enrollment not found");
                        break;
                    }

                    _classroomRepository.AddAttendance((int)enrollmentId, DateTime.Now, true);
                    _staffView.DisplayMessage($"Added attendance to {roleResult.UserName} in {classroomResult.Name}");
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
                    }, true);
                    // Mostra o erro e solicita novamente
                    _staffView.DisplayError(ex.Message);
                }

                continue;
            }
        }
    }
}
