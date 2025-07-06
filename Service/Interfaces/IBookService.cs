using Common.Dto;
namespace Service.Interfaces
{
    public interface IBookService : IService<BookDto>
    {
        Task<List<BookDto>> GetAllSimple();
        Task<BookDto> GetByIdSimple(int id);
        Task<List<BookDto>> GetByTitle(string title);
        Task<List<BookDto>> GetByAuthorName(string authorName);
        Task<List<BookDto>> GetByCategoryName(string categoryName);
    }
}
