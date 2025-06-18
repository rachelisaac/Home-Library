using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext context;

        public UserRepository(IContext context)
        {
            this.context = context;
        }
        public User AddItem(User item)
        {
            context.Users.Add(item);
            context.Save();
            return item;
        }

        public User DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }
        public IQueryable<User> Query()
        {
            return context.Users;
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int id, User item)
        {
            var existing = context.Users.FirstOrDefault(u => u.Id == id);
            if (existing != null)
            {
                existing.Password = item.Password;
                existing.Name = item.Name;

                context.Save();
            }
        }

    }
}
