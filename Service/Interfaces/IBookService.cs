using Common.Dto;
namespace Service.Interfaces
{
    public interface IBookService : IService<BookDto>
    {
        List<BookDto> GetAllSimple();
        BookDto GetByIdSimple(int id);
    }

}
