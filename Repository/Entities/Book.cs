using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Repository.Entities;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [ForeignKey("Author")]
    public int AuthorId { get; set; }

    public Author Author { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime PublishDate { get; set; }

    public string? ImageUrl { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }
}

