namespace Repository.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Book> Books { get; set; } = new();

    }

}
