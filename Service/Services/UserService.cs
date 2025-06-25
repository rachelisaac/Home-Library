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

        public UserDto AddItem(UserRegisterDto item)
        {
            var user = mapper.Map<User>(item);
            var added = repository.AddItem(user);
            return mapper.Map<UserDto>(added);
        }

        //סתם כדי לממש את הממשק-אין צורך בפונקציה הזו
        public UserDto AddItem(UserDto item)
        {
            throw new NotImplementedException();
        }

        public UserLoginDto Authenticate(string Email, string password)
        {
            var user = repository.GetAll()
                .FirstOrDefault(u => u.Email == Email && u.Password == password);

            if (user == null) return null;

            return new UserLoginDto
            {
                Email = user.Email,
                Password = password
            };
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<UserDto> GetAll()
        {
            var users = repository.GetAll();
            return mapper.Map<List<UserDto>>(users);
        }

        public UserDto GetById(int id)
        {
            var user = repository.GetById(id);
            return mapper.Map<UserDto>(user);
        }


        public void Update(int id, UserDto item)
        {
            var existingUser = repository.GetById(id); 
            if (existingUser == null) return;

            mapper.Map(item, existingUser);

            repository.UpdateItem(id, existingUser);
        }

    }
}
