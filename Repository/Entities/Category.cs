using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Book> Books { get; set; }

            [ForeignKey("User")]
            public int UserId { get; set; }

    }
}
