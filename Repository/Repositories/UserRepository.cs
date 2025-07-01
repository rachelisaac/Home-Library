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
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.Save();
            }
            return user;
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public IQueryable<User> Query()
        {
            return context.Users.AsQueryable();
        }

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public void UpdateItem(int id, User item)
        {
            var existing = context.Users.FirstOrDefault(u => u.Id == id);
            if (existing != null)
            {
                existing.Password = item.Password;
                existing.Name = item.Name;
                existing.Role = item.Role;
                existing.Email = item.Email;
                
                context.Save();
            }
        }
    }
}
