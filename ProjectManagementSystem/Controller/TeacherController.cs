using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly TeacherView _teacherView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;
        private readonly LogRepository _logRepository;

        public TeacherController(TeacherView teacherView, RoleRepository roleRepository, ClassroomRepository classroomRepository, LogRepository logRepository)
        {
            _teacherView = teacherView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;
            _logRepository = logRepository;

            // add logger
            Logger logger = new Logger(_logRepository);
            AddSubscriber(logger);
            _logRepository = logRepository;
        }

        public void MarkStudentAttendance()
        {
            //string classroom, string role, string typeRole
            while (true)
            {
                try
                {
                    _teacherView.DisplayTitle("Mark Student Attendance");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _teacherView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroom = _teacherView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // show students
                    List<Role> roles = _classroomRepository.GetRolesByClassroom(classroom);
                    _teacherView.DisplayRoles(roles);

                    // get student name and make validation
                    string userName = _teacherView.GetInput("Enter Student's name:");
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
                    _teacherView.DisplayTitle($"Mark student attendance successful");

                    List<Attendance> attendances = _classroomRepository.GetAttendances(enrollmentResult.Id);
                    _teacherView.DisplayAttendanceInfo(attendances, classroomResult, roleResult);

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
                    _teacherView.DisplayTitle("Add Assessment");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _teacherView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroom = _teacherView.GetInput("Enter Classroom's name:");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomInstance, "Classroom not found");


                    // define task
                    string description = _teacherView.GetInput("Enter task description:");
                    if (description == "<EXIT>") break;
                    ValidateStringInput(description);

                    // define score
                    float maxScore = float.Parse(_teacherView.GetInput("Enter max Score"));
                    if (maxScore == 0) break;
                    ValidateFloatInput(maxScore);

                    // add assignment
                    this._classroomRepository.AddAssessment(classroomInstance.Id, Session.LoggedUser.Id, description, maxScore);
                    _teacherView.DisplayMessage($"\nAssessment '{description}' added with successful\n");


                    // Notify observers that the classroom already exists.
                    this.NotifyObservers(new Alert
                    {
                        Role = Session.LoggedUser.UserName,
                        Action = MethodBase.GetCurrentMethod().Name,
                        Message = $"Assessment {description} added with successful"
                    });
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
                    });
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
                    _teacherView.DisplayTitle("Display Student Submissions");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _teacherView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroomName = _teacherView.GetInput("Enter Classroom's name:");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomResult = this._classroomRepository.GetClassroomByName(classroomName);
                    ValidateObjectInstance(classroomResult, "Classroom not found");

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
                    });
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
                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _teacherView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroomName = _teacherView.GetInput("Enter Classroom's name:");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroomName);
                    ValidateObjectInstance(classroomInstance, "Classroom not found");

                    // list assessment in the classroom
                    List<Assessment> assignmentResult = this._classroomRepository.GetAssignmentsByClassroom(classroomName);
                    _teacherView.DisplayAssessmentResult(assignmentResult);

                    // get classroom name and make validation
                    string assessment = _teacherView.GetInput("Enter Assessment's name:");
                    if (assessment == "<EXIT>") break;
                    ValidateStringInput(assessment);

                    Assessment assessmentInstance = this._classroomRepository.GetAssignmentByName(assessment);
                    ValidateObjectInstance(assessmentInstance, "Assessment not found");

                    // show students
                    List<Role> roles = _classroomRepository.GetRolesByClassroom(classroomName);
                    _teacherView.DisplayRoles(roles);

                    // get teacher name and make validation
                    string userName = _teacherView.GetInput("Enter student's name:");
                    if (userName == "<EXIT>") break;
                    ValidateStringInput(userName);

                    // add role to classroom
                    Role roleResult = this._roleRepository.GetRoleByUserName(userName);
                    ValidateObjectInstance(roleResult, "Role not found");

                    // check permissions
                    List<string> roleList = new List<string> { "student" };
                    ValidatePermission(roleResult.RoleType, roleList);


                    Enrollment enrollmentInstance = this._classroomRepository.GetEnrollment(classroomInstance.Id, roleResult.Id, "student");
                    ValidateObjectInstance(enrollmentInstance, "Enrollment not found");


                    Submission submissionResult = this._classroomRepository.GetSubmissionById(enrollmentInstance.Id);
                    ValidateObjectInstance(submissionResult, "Student submission not found");


                    float score = float.Parse(_teacherView.GetInput("Set score: "));
                    if (score == 0) break;
                    ValidateFloatInput(score);

                    if (score > assessmentInstance.MaxScore)
                    {
                        this._teacherView.DisplayMessage($"Score should be less than '{assessmentInstance.MaxScore}'");
                        break;
                    }
                    this._classroomRepository.UpdateScore(assessmentInstance.Id, roleResult.Id, score);
                    this._teacherView.DisplayMessage($"\nUpdate score {score} to student {roleResult.UserName}\n");
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
                    _teacherView.DisplayError(ex.Message);
                }

                continue;
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
