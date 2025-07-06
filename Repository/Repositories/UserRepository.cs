using Microsoft.EntityFrameworkCore;
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

        public async Task<User> AddItem(User item)
        {
            await context.Users.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task<User> DeleteItem(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.Save();
            }
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public IQueryable<User> Query()
        {
            return context.Users.AsQueryable();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateItem(int id, User item)
        {
            var existing = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existing != null)
            {
                existing.Password = item.Password;
                existing.Name = item.Name;
                existing.Role = item.Role;
                existing.Email = item.Email;

                await context.Save();
            }
        }
    }
}

