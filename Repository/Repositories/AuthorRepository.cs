using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly IContext context;

        public AuthorRepository(IContext context)
        {
            this.context = context;
        }

        public Author AddItem(Author item)
        {
            context.Authors.Add(item);
            context.Save();
            return item;
        }

        public Author DeleteItem(int id)
        {
            var author = context.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                context.Authors.Remove(author);
                context.Save();
            }
            return author;
        }

        public List<Author> GetAll()
        {
            return context.Authors.ToList();
        }

        public Author GetById(int id)
        {
            return context.Authors.FirstOrDefault(a => a.Id == id);
        }

        public Author UpdateItem(Author item)
        {
            var existing = context.Authors.FirstOrDefault(a => a.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                existing.BirthYear = item.BirthYear;
                context.Save();
            }
            return existing;
        }

    }
}
