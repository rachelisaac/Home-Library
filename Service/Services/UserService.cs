using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper map)
        {
            this.repository = repository;
            this.mapper = map;

        }
        public User AddItem(User item)
        {
           return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, User item)
        {
            repository.UpdateItem(id, item);
        }
    }
}
