namespace Common.Dto
{
    public class BookDto
    {
            public string Title { get; set; }
            public string AuthorName { get; set; }  
            public string CategoryName { get; set; } 
            public DateTime PublishDate { get; set; }
            public byte[]? ArrImage { get; set; }
    }
}
