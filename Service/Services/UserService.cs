using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserDto> AddItem(UserRegisterDto item)
        {
            var user = mapper.Map<User>(item);
            user.Role = Repository.Entities.UserRole.User;
            var added = await repository.AddItem(user);
            return mapper.Map<UserDto>(added);
        }

        // סתם כדי לממש את הממשק - אין צורך בפונקציה הזו
        public Task<UserDto> AddItem(UserDto item)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var users = await repository.GetAll();
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await repository.GetAll();
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await repository.GetById(id);
            return mapper.Map<UserDto>(user);
        }

        public async Task Update(int id, UserUpdate item)
        {
            var existingUser = await repository.GetById(id);
            if (existingUser == null) return;
            mapper.Map(item, existingUser);
            await repository.UpdateItem(id, existingUser);
        }

        public async Task Update(int id, UserUpdate2 item)
        {
            var existingUser = await repository.GetById(id);
            if (existingUser == null) return;
            mapper.Map(item, existingUser);
            await repository.UpdateItem(id, existingUser);
        }

        // סתם בשביל לממש את הממשק - אין צורך בפונקציה הזו
        public Task Update(int id, UserDto item)
        {
            throw new NotImplementedException();
        }
    }
}
