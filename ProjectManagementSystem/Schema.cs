namespace ProjectManagementSystem
{
    public class RoleSchema
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }
    }

    public class ClassroomSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }


    public class AttendanceSchema
    {
        DateTime Date { get; set; }
        bool Present { get; set; }
    }

    public class AssignmentSchema
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string Description { get; set; }
        public float MaxScore { get; set; }
    }

}
