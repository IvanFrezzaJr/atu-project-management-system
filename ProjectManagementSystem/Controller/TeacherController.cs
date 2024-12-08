using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly TeacherView _teacherView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;

        public TeacherController(TeacherView teacherView, RoleRepository roleRepository, ClassroomRepository classroomRepository)
        {
            _teacherView = teacherView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;

            // add logger
            Logger logger = new Logger();
            AddSubscriber(logger);
        }

        public void MarkStudentAttendance()
        {
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _teacherView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroom);  // TODO: move Classroom to Classroom
                    ValidateObjectInstance(classroomResult);

                    // get teacher name and make validation
                    string userName = _teacherView.GetInput("What is Student name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, $"'User {userName}' not found");

                    // get enrollment
                    int? enrollmentId = this._classroomRepository.GetEnrollmentId(classroomResult.Id, roleResult.Id);
                    if (enrollmentId == null)
                    {
                        _teacherView.DisplayMessage("Student enrollment not found");
                        break;
                    }

                    // add attendance
                    _classroomRepository.AddAttendance((int)enrollmentId, DateTime.Now, true);
                    _teacherView.DisplayMessage($"Added attendance to {roleResult.UserName} in {classroomResult.Name}");
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
                    _teacherView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        public void AddAssignment()
        {
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _teacherView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomInstance);


                    // define task
                    string description = _teacherView.GetInput("What is the task description?");
                    if (description == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // define score
                    float maxScore = float.Parse(_teacherView.GetInput("Max Score"));
                    if (maxScore == 0) break;
                    ValidateFloatInput(maxScore);

                    // add assignment
                    this._classroomRepository.AddAssessment(classroomInstance.Id, Session.LoggedUser.Id, description, maxScore);
                    _teacherView.DisplayMessage($"\nAssignment {description} added with successful\n");
                    break;

                }
                catch (Exception ex) when (
                    ex is FormatException||
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
                    _teacherView.DisplayError(ex.Message);
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
                    string classroomName = _teacherView.GetInput("What is the Classroom name?");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroomName);  // TODO: move Classroom to Classroom
                    ValidateObjectInstance(
                        classroomResult,
                        $"'{classroomName}' classroom not found"
                        );

                    _teacherView.DisplayMessage($"\nSubmissions for Classroom: {classroomName}\n");

                    List<dynamic> studentSubmissions = this._roleRepository.GetStudentSubmissions(classroomName);
                    _teacherView.ShowSubmissionsResult(studentSubmissions);
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
                    _teacherView.DisplayError(ex.Message);
                }
                continue;
            }

        }

        public void SetStudentGrade()
        {
            while (true)
            {
                try
                {
                    // get classroom name and make validation
                    string classroom = _teacherView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomInstance);

                    // list assessment in the classroom
                    List<Assessment> assignmentResult = this._classroomRepository.GetAssignmentsByClassroom(classroom);
                    _teacherView.ShowAssignmentsMenu(assignmentResult);

                    // get classroom name and make validation
                    string assessment = _teacherView.GetInput("What is the assessment description?");
                    if (assessment == "<EXIT>") break;
                    ValidateStringInput(assessment);

                    Assessment assessmentInstance = this._classroomRepository.GetAssignmentByName(assessment);
                    ValidateObjectInstance(assessmentInstance);

                    // get teacher name and make validation
                    string userName = _teacherView.GetInput("What is Student name?");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, $"'User {userName}' not found");

                    // check permissions
                    List<string> roleList = new List<string> { "student" };
                    ValidatePermission(roleResult.RoleType, roleList);


                    int? enrollmentId = this._classroomRepository.GetEnrollmentId(classroomInstance.Id, roleResult.Id);
                    if (enrollmentId == null)
                    {
                        this._teacherView.DisplayMessage("Student enrollment not found.");
                        break;
    
                    }

                    float score = float.Parse(_teacherView.GetInput("Set score: "));
                    if (score == 0) break;
                    ValidateFloatInput(score);

                    if (score > assessmentInstance.MaxScore)
                    {
                        this._teacherView.DisplayMessage($"Score should be less than '{assessmentInstance.MaxScore}'");
                        break;
                    }
                    this._classroomRepository.UpdateScore(assessmentInstance.Id, roleResult.Id, score);
                    this._teacherView.DisplayMessage($"Update score {score} to student {roleResult.UserName}");

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
                    _teacherView.DisplayError(ex.Message);
                }

                continue;
            }
        }

    }
}
