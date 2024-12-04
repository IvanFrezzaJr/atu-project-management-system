namespace ProjectManagementSystem.Models
{


    public class Login
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RoleType { get; set; }
    }

}