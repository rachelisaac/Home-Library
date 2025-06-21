using Common.Dto;
namespace Service.Interfaces
{
    public interface IBookService : IService<BookDto>
    {
        List<BookDto> GetAllSimple();
        BookDto GetByIdSimple(int id);
        List<BookDto> GetByTitle(string title);
        List<BookDto> GetByAuthorName(string authorName);
        List<BookDto> GetByCategoryName(string categoryName);

    }

}
