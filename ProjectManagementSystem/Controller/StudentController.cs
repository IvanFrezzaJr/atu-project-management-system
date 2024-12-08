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
                    // get classroom name and make validation
                    string classroom = _studentView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    // list assessment in the classroom
                    List<Assessment> assessments = this._classroomRepository.GetAssignmentsByClassroom(classroom);
                    _studentView.ShowAssignmentsMenu(assessments);

                    // get classroom name and make validation
                    string assessment = _studentView.GetInput("What is the assessment description?");
                    if (assessment == "<EXIT>") break;
                    ValidateStringInput(assessment);

                    Assessment assessmentInstance = this._classroomRepository.GetAssignmentByName(assessment);
                    ValidateObjectInstance(assessmentInstance, "Assessment not found");

                    // get filepath
                    string filePath = _studentView.GetInput("What is the filepath?");
                    if (filePath == "<EXIT>") break;
                    ValidateStringInput(filePath);

                    
                    bool status = _classroomRepository.AddSubmission(assessmentInstance.Id, Session.LoggedUser.Id, filePath);
                    if (status)
                    {
                        _studentView.DisplayMessage($"\nAssignment added with successful\n");
                        break;
                    }
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
                    // get classroom name and make validation
                    string classroom = _studentView.GetInput("What is the Classroom name?");
                    if (classroom == "<EXIT>") break;
                    ValidateStringInput(classroom);

                    Classroom classroomInstance = this._classroomRepository.GetClassroomByName(classroom);
                    ValidateObjectInstance(classroomInstance, "Classroom not found");

                    int? enrollmentId = this._classroomRepository.GetEnrollmentId(classroomInstance.Id, Session.LoggedUser.Id);
                    if (enrollmentId == null)
                    {
                        this._studentView.DisplayMessage("Student enrollment not found.");
                        break;
                    }

                    List<dynamic> studentSubmissions = this._roleRepository.GetStudentSubmissions(classroom);

                    List<dynamic> studentSubmissionsResult = null;
                    foreach (dynamic studentSubmission in studentSubmissions) 
                    {
                        if (studentSubmission.ClassName == classroom)
                        {
                            studentSubmissionsResult.Add(studentSubmission);
                            
                        }
                    }

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
