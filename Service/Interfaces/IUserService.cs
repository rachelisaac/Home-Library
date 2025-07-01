using Common.Dto;
using Repository.Entities;

namespace Service.Interfaces
{
    public interface IUserService: IService<UserDto>
    {
        UserDto AddItem(UserRegisterDto item);
        User Authenticate(string email, string password);

        void Update(int id, UserUpdate item);

    }
}
