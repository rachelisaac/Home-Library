namespace Repository.Entities
{
    public enum UserRole
    {
        User,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;

        public List<Book> Books { get; set; } = new();

    }

}
