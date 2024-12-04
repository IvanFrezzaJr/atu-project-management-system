using System.ComponentModel.Design;
using System.Reflection;

namespace ProjectManagementSystem.Models
{
    public class Student : Role
    {

        private Database database = new Database();

        public Student(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active) { }


        public bool AddSubmission(int studentId, string classroom, string assignment, string filePath)
        {
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom not found"
                }, true);
                return false;
            }

            int? enrollmentId = this.database.GetEnrollmentId(classroomResult.Id, studentId);
            if (enrollmentId == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Student enrollment not found"
                }, true);
                return false;
            }

            AssignmentSchema assignmentResult = this.database.GetAssignmentByName(assignment);
            if (assignmentResult == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Assignment not found"
                }, true);
                return false;
            }

            this.database.AddSubmission(assignmentResult.Id, studentId, filePath);

            this.NotifyObservers(new Alert
            {
                Role = this.GetType().Name,
                Action = MethodBase.GetCurrentMethod().Name,
                Message = $"Submission added by {studentId} to {assignmentResult.Id}"
            }, true);


            return true;
        }


        public List<AssignmentSchema> GetAssignmantAvailable(string classroom)
        {
            var assessments = new List<AssignmentSchema>();

            List<AssignmentSchema> assignmentResult = this.database.GetAssignmentsByClassroom(classroom);
            return assignmentResult;
        }

        public List<dynamic> DisplayStudentSubmissions(string classroomName, string studentName)
        {
            return this.database.GetStudentSubmissions(classroomName, studentName);

        }
    }
}
