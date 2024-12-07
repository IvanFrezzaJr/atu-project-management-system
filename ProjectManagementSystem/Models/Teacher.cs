namespace ProjectManagementSystem.Models
{
    public class Teacher : Role
    {

        private Database_ database = new Database_();

        public Teacher(int id, string userName, string password, bool active, string roleType) : base(id, userName, password, active, roleType) { }

        //public bool MarkStudentAttendance(string classroom, string role, string typeRole)
        //{
        //    ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
        //    if (classroomResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Classroom not found"
        //        }, true);
        //        return false;
        //    }

        //    RoleSchema roleResult = this.database.GetRoleByUsername(role);
        //    if (roleResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Role not found"
        //        }, true);
        //        return false;
        //    }

        //    if (roleResult.RoleType != typeRole)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
        //        }, true);
        //        return false;
        //    }

        //    int? enrollmentId = this.database.GetEnrollmentId(classroomResult.Id, roleResult.Id);
        //    if (enrollmentId == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Student enrollment not found"
        //        }, true);
        //        return false;
        //    }

        //    this.database.AddAttendance((int)enrollmentId, DateTime.Now, true);
        //    this.NotifyObservers(new Alert
        //    {
        //        Role = this.GetType().Name,
        //        Action = MethodBase.GetCurrentMethod().Name,
        //        Message = $"Added attendance to {roleResult.UserName} in {classroomResult.Name}"
        //    }, false);
        //    return true;
        //}

        //public bool AddAssignment(string classroom, string role, string description, float maxScore)
        //{
        //    ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
        //    if (classroomResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Classroom not found"
        //        }, true);
        //        return false;
        //    }

        //    RoleSchema roleResult = this.database.GetRoleByUsername(role);
        //    if (roleResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Role not found"
        //        }, true);
        //        return false;
        //    }

        //    this.database.AddAssessment(classroomResult.Id, roleResult.Id, description, maxScore);
        //    this.NotifyObservers(new Alert
        //    {
        //        Role = this.GetType().Name,
        //        Action = MethodBase.GetCurrentMethod().Name,
        //        Message = $"Added assessment {description} to {classroomResult.Id} with {maxScore}"
        //    }, true);
        //    return true;
        //}


        //public bool SetStudentGrade(string classroom, string assignment, string student, string typeRole, float score)
        //{
        //    ClassroomSchema classroomResult = this.database.GetClassroomByName(classroom);
        //    if (classroomResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Classroom not found"
        //        }, true);
        //        return false;
        //    }

        //    Assessment assignmentResult = this.database.GetAssignmentByName(assignment);
        //    if (assignmentResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Assessment not found"
        //        }, true);
        //        return false;
        //    }

        //    RoleSchema studentResult = this.database.GetRoleByUsername(student);
        //    if (studentResult == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Role not found"
        //        }, true);
        //        return false;
        //    }

        //    if (studentResult.RoleType != typeRole)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
        //        }, true);
        //        return false;
        //    }

        //    int? enrollmentId = this.database.GetEnrollmentId(classroomResult.Id, studentResult.Id);
        //    if (enrollmentId == null)
        //    {
        //        this.NotifyObservers(new Alert
        //        {
        //            Role = this.GetType().Name,
        //            Action = MethodBase.GetCurrentMethod().Name,
        //            Message = "Student enrollment not found."
        //        }, true);
        //        return false;
        //    }

        //    this.database.UpdateScore(assignmentResult.Id, studentResult.Id, score);
        //    this.NotifyObservers(new Alert
        //    {
        //        Role = this.GetType().Name,
        //        Action = MethodBase.GetCurrentMethod().Name,
        //        Message = $"Update score {score} to student {studentResult.UserName}"
        //    }, true);
        //    return true;
        //}


        //public List<dynamic> DisplayStudentSubmissions(string classroomName)
        //{
        //    return this.database.GetStudentSubmissions(classroomName);

        //}

        //public List<Assessment> GetAssignmantAvailable(string classroom)
        //{
        //    var assessments = new List<Assessment>();

        //    List<Assessment> assignmentResult = this.database.GetAssignmentsByClassroom(classroom);
        //    return assignmentResult;
        //}

    }
}
