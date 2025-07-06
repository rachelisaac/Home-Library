using Common.Dto;
using Repository.Entities;

namespace Service.Interfaces
{
    public interface IUserService : IService<UserDto>
    {
        Task<UserDto> AddItem(UserRegisterDto item);
        Task<User> Authenticate(string email, string password);

        Task Update(int id, UserUpdate item);
        Task Update(int id, UserUpdate2 item);
    }
}
