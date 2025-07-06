using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly IContext context;

        public AuthorRepository(IContext context)

        {
            this.context = context;
        }

        public async Task<Author> AddItem(Author item)
        {
            await context.Authors.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task<Author> DeleteItem(int id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author != null)
            {
                context.Authors.Remove(author);
                await context.Save();
            }
            return author;
        }

        public async Task<List<Author>> GetAll()
        {
            return await context.Authors.ToListAsync();
        }

        public IQueryable<Author> Query()
        {
            return context.Authors;
        }

        public async Task<Author> GetById(int id)
        {
            return await context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateItem(int id, Author item)
        {
            var existing = await context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (existing != null)
            {
                existing.Name = item.Name;
                await context.Save();
            }
        }
    }
}
