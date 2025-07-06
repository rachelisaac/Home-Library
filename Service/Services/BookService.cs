using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> repository;
        private readonly IRepository<Author> authorRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public BookService(ICurrentUserService currentUserService,IRepository<Book> repository, IRepository<Author> authorRepository, IRepository<Category> categoryRepository, IMapper map)
        {
            this.currentUserService = currentUserService;
            this.repository = repository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = map;
        }

        public BookDto AddItem(BookDto item)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();
            int targetUserId;
            if (!isAdmin)
            {
                targetUserId = (int)currentUserId;
            }
            else
            {
                if (item.UserId == null)
                {
                    throw new Exception("כמנהל, חובה לציין למי שייך הספר (UserId).");
                }
                targetUserId = (int)item.UserId;
            }
            var author = authorRepository.GetAll()
                .FirstOrDefault(a => a.Name == item.AuthorName && a.UserId == targetUserId);
            if (author == null)
            {
                author = new Author { Name = item.AuthorName, UserId = targetUserId };
                author = authorRepository.AddItem(author);
            }
            var category = categoryRepository.GetAll()
                .FirstOrDefault(c => c.Name == item.CategoryName && c.UserId == targetUserId);
            if (category == null)
            {
                category = new Category { Name = item.CategoryName, UserId = targetUserId };
                category = categoryRepository.AddItem(category);
            }
            var book = mapper.Map<BookDto, Book>(item);
            book.AuthorId = author.Id;
            book.CategoryId = category.Id;
            book.UserId = targetUserId;
            var addedBook = repository.AddItem(book);
            return mapper.Map<Book, BookDto>(addedBook);
        }


        public void DeleteItem(int id)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var book = repository.GetById(id);
            if (book == null)
                throw new Exception($"ספר עם מזהה {id} לא נמצא.");

            if (!isAdmin && book.UserId != currentUserId)
                throw new UnauthorizedAccessException("אין לך הרשאה למחוק ספר זה.");

            var authorId = book.AuthorId;
            var categoryId = book.CategoryId;
            var userId = book.UserId;

            repository.DeleteItem(id); 
            var isAuthorUsed = repository.Query().Any(b => b.AuthorId == authorId && b.UserId == userId);
            if (!isAuthorUsed)
            {
                authorRepository.DeleteItem(authorId);
            }
            var isCategoryUsed = repository.Query().Any(b => b.CategoryId == categoryId && b.UserId == userId);
            if (!isCategoryUsed)
            {
                categoryRepository.DeleteItem(categoryId);
            }
        }

        public List<BookDto> GetAll()
        {
            var userId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            IQueryable<Book> booksQuery = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category); 

            if (!isAdmin)
            {
                booksQuery = booksQuery.Where(b => b.UserId == userId); 
            }

            var list = booksQuery.ToList();

            return mapper.Map<List<BookDto>>(list);
        }




        public BookDto GetById(int id)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();
            var query = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Id == id);
            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == currentUserId);
            }
            var book = query.FirstOrDefault();
            return mapper.Map<BookDto>(book);
        }



        public List<BookDto> GetAllSimple()
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var books = repository.GetAll();

            if (!isAdmin)
            {
                books = books.Where(b => b.UserId == currentUserId).ToList();
            }

            return mapper.Map<List<BookDto>>(books);
        }

        public BookDto GetByIdSimple(int id)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var book = repository.GetById(id);

            if (book == null)
                return null;

            if (!isAdmin && book.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לגשת לספר הזה.");
            }

            return mapper.Map<BookDto>(book);
        }



        public List<BookDto> GetByAuthorName(string authorName)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var booksQuery = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Author.Name.Contains(authorName));

            if (!isAdmin)
            {
                booksQuery = booksQuery.Where(b => b.UserId == currentUserId);
            }

            var books = booksQuery.ToList();
            return mapper.Map<List<BookDto>>(books);
        }

        public List<BookDto> GetByCategoryName(string categoryName)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var booksQuery = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Category.Name.Contains(categoryName));

            if (!isAdmin)
            {
                booksQuery = booksQuery.Where(b => b.UserId == currentUserId);
            }

            var books = booksQuery.ToList();
            return mapper.Map<List<BookDto>>(books);
        }


        public List<BookDto> GetByTitle(string title)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var booksQuery = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Title.Contains(title));

            if (!isAdmin)
            {
                booksQuery = booksQuery.Where(b => b.UserId == currentUserId);
            }

            var books = booksQuery.ToList();
            return mapper.Map<List<BookDto>>(books);
        }







        public void Update(int id, BookDto item)
        {
            var currentUserId = currentUserService.GetUserId();
            var isAdmin = currentUserService.IsAdmin();

            var existingBook = repository.GetById(id);
            var bookOwnerId = existingBook.UserId;

            if (existingBook == null)
                throw new Exception($"ספר עם מזהה {id} לא נמצא.");

            if (!isAdmin && existingBook.UserId != currentUserId)
                throw new UnauthorizedAccessException("אין לך הרשאה לעדכן ספר זה.");

            var oldCategoryId = existingBook.CategoryId;
            var oldAuthorId = existingBook.AuthorId;
            var author = authorRepository.GetAll().FirstOrDefault(a => a.Name == item.AuthorName);
            if (author == null)
            {
                author = new Author { Name = item.AuthorName  ,UserId = bookOwnerId };
                author = authorRepository.AddItem(author);
            }
            var category = categoryRepository.GetAll().FirstOrDefault(c => c.Name == item.CategoryName);
            if (category == null)
            {
                category = new Category { Name = item.CategoryName, UserId = bookOwnerId };
                category = categoryRepository.AddItem(category);
            }
            var updatedBook = mapper.Map<BookDto, Book>(item);
            updatedBook.Id = id;
            updatedBook.AuthorId = author.Id;
            updatedBook.CategoryId = category.Id;
            updatedBook.UserId = existingBook.UserId; 

            repository.UpdateItem(id, updatedBook);
            if (oldCategoryId != category.Id)
            {
                var isCategoryUsed = repository.Query()
                    .Any(b => b.CategoryId == oldCategoryId && b.UserId == existingBook.UserId);
                if (!isCategoryUsed)
                {
                    categoryRepository.DeleteItem(oldCategoryId);
                }
            }
            if (oldAuthorId != author.Id)
            {
                var isAuthorUsed = repository.Query()
                    .Any(b => b.AuthorId == oldAuthorId && b.UserId == existingBook.UserId);
                if (!isAuthorUsed)
                {
                    authorRepository.DeleteItem(oldAuthorId);
                }
            }
        }

    }
}
