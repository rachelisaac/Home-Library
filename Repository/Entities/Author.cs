using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Book> Books { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
