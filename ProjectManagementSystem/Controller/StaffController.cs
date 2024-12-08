using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace ProjectManagementSystem.Controllers
{
    public class StaffController : BaseController
    {
        private readonly StaffView _staffView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;
        private readonly LogRepository _logRepository;

        public StaffController(StaffView staffView, RoleRepository roleRepository, ClassroomRepository classroomRepository, LogRepository logRepository)
        {
            _staffView = staffView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;
            _logRepository = logRepository;

            // add logger
            Logger logger = new Logger(_logRepository);
            AddSubscriber(logger);
            _logRepository = logRepository;
        }

        private void ValidateClassroomExists(string userName)
        {
            if (this._classroomRepository.ClassroomExists(userName))
            {
                throw new ApplicationException("Classroom already exists");
            }
        }

        public void CreateClassroom()
        {
            while (true)
            {
                try
                {
                    _staffView.DisplayTitle("Classroom registration");
                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // get classroom and validate it
                    Classroom classroomInstance = new Classroom(classroom);

                    // insert classroom
                    this._classroomRepository.InsertClassroom(classroomInstance);
                    _staffView.DisplayTitle($"Classroom added successfully");
                    _staffView.DisplayClassroomInfo(classroomInstance);

                    // Notify observers that the classroom already exists.
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"{Session.LoggedUser.UserName} inserted the {classroomInstance.Name} classroom successful."
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
                    _staffView.DisplayTitle("Assign Role To Classroom");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _staffView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // show students
                    List<Role> roles = _classroomRepository.GetAllRoles("student");
                    _staffView.DisplayRoles(roles);

                    // get student name and make validation
                    string userName = _staffView.GetInput("Enter Student's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, "Role not found");

                    // Check permissions
                    List<string> roleList = new List<string> { "teacher", "student" };
                    ValidatePermission(roleResult.RoleType, roleList);

                    // get classrom and validate it
                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomResult, "Classroom not found");
                    ValidateEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);

                    // add role to classroom
                    this._classroomRepository.AddEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);
                    _staffView.DisplayTitle($"Enrollment added successful");
                    _staffView.DisplayEnrollmentInfo(roleResult, classroomResult);

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
                    _staffView.DisplayTitle("Mark Student Attendance");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _staffView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroom = _staffView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // show students
                    List<Role> roles = _classroomRepository.GetRolesByClassroom(classroom);
                    _staffView.DisplayRoles(roles);

                    // get student name and make validation
                    string userName = _staffView.GetInput("Enter Student's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, "Role not found");

                    // Check permissions
                    List<string> roleList = new List<string> { "student" };
                    ValidatePermission(roleResult.RoleType, roleList);

                    // get classrom and validate it
                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomResult, "Classroom not found");
                    ValidateNoEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);


                    // get enrollment
                    Enrollment enrollmentResult = this._classroomRepository.GetEnrollment(classroomResult.Id, roleResult.Id, roleResult.RoleType);
                    ValidateObjectInstance(classroomResult, "Classroom not found");

                    // Mark attendance
                    _classroomRepository.AddAttendance(enrollmentResult.Id, DateTime.Now, true);
                    _staffView.DisplayTitle($"Mark student attendance successful");

                    List<Attendance> attendances = _classroomRepository.GetAttendances(enrollmentResult.Id);
                    _staffView.DisplayAttendanceInfo(attendances, classroomResult, roleResult);

                    // Notify observers that the classroom already exists.
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"Added attendance to {roleResult.UserName} in {classroomResult.Name}"
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
                    _staffView.DisplayError(ex.Message);
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
