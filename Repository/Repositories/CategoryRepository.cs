using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IContext context;

        public CategoryRepository(IContext context)
        {
            this.context = context;
        }

        public Category AddItem(Category item)
        {
            context.Categories.Add(item);
            context.Save();
            return item;
        }

        public Category DeleteItem(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.Save();
            }
            return category;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public Category UpdateItem(Category item)
        {
            var existing = context.Categories.FirstOrDefault(c => c.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                context.Save();
            }
            return existing;
        }
    }
}
