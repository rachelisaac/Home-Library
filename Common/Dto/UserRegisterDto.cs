namespace Common.Dto
{
    public enum UserRole
    {
        User,
        Admin
    }
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}
