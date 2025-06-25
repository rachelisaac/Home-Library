using Common.Dto;

namespace Service.Interfaces
{
    public interface IUserService: IService<UserDto>
    {
        UserDto AddItem(UserRegisterDto item);
        public UserLoginDto Authenticate(string username, string password);


    }
}
