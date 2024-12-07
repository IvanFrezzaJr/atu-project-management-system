using ProjectManagementSystem.Database;
using ProjectManagementSystem.Views;
using ProjectManagementSystem.Core;
using ProjectManagementSystem.Models;
using System.Reflection;
using ProjectManagementSystem.Controller;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProjectManagementSystem.Controllers
{
    public class PrincipalController : BaseController
    {
        private readonly PrincipalView _principalView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;

        public string UserName { get; set; }
        public bool IsAutheticated { get; set; }

        public PrincipalController(PrincipalView principalView, RoleRepository roleRepository, ClassroomRepository classroomRepository)
        {
            _principalView = principalView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;

            // add logger
            Logger logger = new Logger();
            AddSubscriber(logger);
        }

        public void AssignRoleToClassroom()
        {
            //string classroom, string role, string typeRole
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _principalView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);  
                    

                    // get teacher name and make validation
                    string userName = _principalView.GetInput("What is Teacher name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);


                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidatePermission(roleResult.RoleType);

                    ClassroomSchema classroomResult = this._classroomRepository.GetClassroomByName(classroom);  // TODO: move ClassroomSchema to Classroom
                    ValidateObjectInstance(classroomResult, $"'{classroom}' classroom not found");

                    bool status = this._classroomRepository.AddRoleToClassroom(classroomResult.Id, roleResult.Id, roleResult.RoleType);
                    ValidateCondition(
                        status,
                        $"Added role '{roleResult.RoleType}' to classroom  {classroomResult.Name}",
                        $"Fail to add role '{roleResult.RoleType}' to classroom  {classroomResult.Name}",
                         _principalView.DisplayMessage
                     );
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
                    }, true);
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
                    // get username
                    string userName = _principalView.GetInput("What is username?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // get role from database
                    Role role = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(role);

                    // toggle role 
                    bool active = (role.Active) ? false : true;
                    bool status = this._roleRepository.ActivateRole(userName, active);
                    ValidateCondition(
                        status,
                        $"Role '{role.RoleType}' is '{active}'",
                        $"Role '{role.RoleType}' is '{active}'",
                         _principalView.DisplayMessage
                     );
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
                    }, true);
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
                    // get classroom name and make validation
                    string classroomName = _principalView.GetInput("What is the Classroom name?");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    ClassroomSchema classroomResult = this._classroomRepository.GetClassroomByName(classroomName);  // TODO: move ClassroomSchema to Classroom
                    ValidateObjectInstance(
                        classroomResult, 
                        $"'{classroomName}' classroom not found"
                        );

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
                    }, true);
                    // Mostra o erro e solicita novamente
                    _principalView.DisplayError(ex.Message);
                }
                continue;
            }

        }

    }
}
