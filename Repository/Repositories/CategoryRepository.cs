using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IContext context;

        public CategoryRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Category> AddItem(Category item)
        {
            await context.Categories.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task<Category> DeleteItem(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.Save();
            }
            return category;
        }

        public async Task<List<Category>> GetAll()
        {
            return await context.Categories.ToListAsync();
        }

        public IQueryable<Category> Query()
        {
            return context.Categories;
        }

        public async Task<Category> GetById(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateItem(int id, Category item)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existing != null)
            {
                existing.Name = item.Name;
                await context.Save();
            }
        }
    }
}
