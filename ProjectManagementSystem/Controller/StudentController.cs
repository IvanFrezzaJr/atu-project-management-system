using ProjectManagementSystem.Controller;
using ProjectManagementSystem.Database;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System.Data.SQLite;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class StudentController : BaseController
    {
        private readonly StudentView _studentView;
        private readonly RoleRepository _roleRepository;
        private readonly ClassroomRepository _classroomRepository;

        public StudentController(StudentView studentView, RoleRepository roleRepository, ClassroomRepository classroomRepository)
        {
            _studentView = studentView;
            _roleRepository = roleRepository;
            _classroomRepository = classroomRepository;

            // add logger
            Logger logger = new Logger();
            AddSubscriber(logger);
        }

        public void AddSubmission()
        {
            while (true)
            {
                try
                {
                    _studentView.DisplayTitle("Add Submission");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _studentView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroomName = _studentView.GetInput("Enter Classroom's name:");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroomName);
                    ValidateObjectInstance(classroomInstance, "Classroom not found");

                    // validate enrollment
                    Enrollment enrollmentInstance = this._classroomRepository.GetEnrollment(classroomInstance.Id, Session.LoggedUser.Id, "student");
                    ValidateObjectInstance(enrollmentInstance, "Enrollment not found");


                    // list assessment in the classroom
                    List<Assessment> assessments = this._classroomRepository.GetAssignmentsByClassroom(classroomName);
                    _studentView.DisplayAssessmentResult(assessments);

                    // get classroom name and make validation
                    string assessment = _studentView.GetInput("Enter Assessment's name:");
                    if (assessment == "<EXIT>") break;
                    ValidateStringInput(assessment);

                    Assessment assessmentInstance = this._classroomRepository.GetAssignmentByName(assessment);
                    ValidateObjectInstance(assessmentInstance, "Assessment not found");

                    // get filepath
                    string filePath = _studentView.GetInput("Enter the filepath to the assessment:");
                    if (filePath == "<EXIT>") break;
                    ValidateStringInput(filePath);

                    _classroomRepository.AddSubmission(assessmentInstance.Id, Session.LoggedUser.Id, filePath);
                    _studentView.DisplaySuccess($"\nAssignment added with successful\n");
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
                    _studentView.DisplayError(ex.Message);
                }

                continue;
            }
        }

        public void ShowScore()
        {
            while (true)
            {
                try
                {
                    _studentView.DisplayTitle("Show Score");

                    // show classrooms
                    List<Classroom> classrooms = _classroomRepository.GetAllClassroom();
                    _studentView.DisplayClassrooms(classrooms);

                    // get classroom name and make validation
                    string classroomName = _studentView.GetInput("Enter Classroom's name:");
                    if (classroomName == "<EXIT>") break;
                    ValidateStringInput(classroomName);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroomName);
                    ValidateObjectInstance(classroomInstance, "Classroom not found");

                    // validate enrollment
                    Enrollment enrollmentInstance = this._classroomRepository.GetEnrollment(classroomInstance.Id, Session.LoggedUser.Id, "student");
                    ValidateObjectInstance(enrollmentInstance, "Enrollment not found");

                    List<dynamic> studentSubmissions = this._roleRepository.GetStudentSubmissions(classroomName, Session.LoggedUser.UserName);

                    _studentView.ShowSubmissionsResult(studentSubmissions);
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
                    _studentView.DisplayError(ex.Message);
                }

                continue;
            }
        }

    }
}
