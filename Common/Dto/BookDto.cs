using Microsoft.AspNetCore.Http;
namespace Common.Dto

{
    public class BookDto
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; } 
        public DateTime? PublishDate { get; set; }
        public byte[]? ArrImage { get; set; }
        public IFormFile? File { get; set; }
        public int? UserId { get; set; }
    }
}
