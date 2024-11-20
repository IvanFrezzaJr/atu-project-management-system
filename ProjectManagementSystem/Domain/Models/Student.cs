namespace ProjectManagementSystem.Domain.Models
{
    public class Student : Role
    {

        private Database database = new Database();

        public Student(int id, string username, string password, string roleType, bool active) : base(id, username, password, roleType, active) { }


        public bool AddSubmission(int studentId, string classroom, string description, string filePath)
        {
            ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
            if (classroomResult == null)
            {
                System.Console.WriteLine("\nClassroom not found\n");
                return false;
            }

            AssignmentSchema assignmentResult = this.database.GetAssignmentByClassroomName(classroom, description);
            if (assignmentResult == null)
            {
                System.Console.WriteLine("\nAssignment not found\n");
                return false;
            }

            this.database.AddSubmission(assignmentResult.Id, studentId, filePath);



            // TODO: AUDIT LOGS
            //// Notifica os assinantes sobre a submissão
            //string message = $"Student '{this.Name}' submitted: {title}";

            //// Create an alert and notify observers
            //Alert alert = new Alert(this, "Submit", message);
            //this.NotifyObservers(alert);

            return true;
        }


    }
}
