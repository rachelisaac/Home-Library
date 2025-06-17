using Repository.Entities;
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

        public Book AddItem(Book item)
        {
            context.Books.Add(item);
            context.Save();
            return item;
        }

        public Book DeleteItem(int id)
        {
            var book = context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                context.Books.Remove(book);
                context.Save();
            }
            return book;
        }

        public List<Book> GetAll()
        {
            return context.Books.ToList();
        }

        public IQueryable<Book> Query()
        {
            return context.Books;
        }


        public Book GetById(int id)
        {
            return context.Books.FirstOrDefault(b => b.Id == id);
        }

        public void UpdateItem(int id, Book item)
        {
            var existing = context.Books.FirstOrDefault(b => b.Id == id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.AuthorId = item.AuthorId;
                existing.CategoryId = item.CategoryId;
                existing.PublishDate = item.PublishDate;

                context.Save();
            }
        }

    }
}
