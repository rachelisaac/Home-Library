using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly IContext context;

        public BookRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Book> AddItem(Book item)
        {
            await context.Books.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task<Book> DeleteItem(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                context.Books.Remove(book);
                await context.Save();
            }
            return book;
        }

        public async Task<List<Book>> GetAll()
        {
            return await context.Books.ToListAsync();
        }

        public IQueryable<Book> Query()
        {
            return context.Books;
        }

        public async Task<Book> GetById(int id)
        {
            return await context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateItem(int id, Book item)
        {
            var existing = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.AuthorId = item.AuthorId;
                existing.CategoryId = item.CategoryId;
                existing.PublishDate = item.PublishDate;

                if (!string.IsNullOrEmpty(item.ImageUrl))
                    existing.ImageUrl = item.ImageUrl;

                await context.Save();
            }
        }
    }
}
