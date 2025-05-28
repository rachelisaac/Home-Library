using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> repository;
        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
        }
        public User AddItem(User item)
        {
            repository.AddItem(item);
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
