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
        private readonly IMapper mapper;

        public BookService(IRepository<Book> repository, IRepository<Author> authorRepository, IRepository<Category> categoryRepository, IMapper map)
        {
            this.repository = repository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = map;
        }

        public BookDto AddItem(BookDto item)
        {
            var author = authorRepository.GetAll().FirstOrDefault(a => a.Name == item.AuthorName);
            if (author == null)
            {
                author = new Author { Name = item.AuthorName };
                author = authorRepository.AddItem(author);
            }

            var category = categoryRepository.GetAll().FirstOrDefault(c => c.Name == item.CategoryName);
            if (category == null)
            {
                category = new Category { Name = item.CategoryName };
                category = categoryRepository.AddItem(category);
            }

            var book = mapper.Map<BookDto, Book>(item);
            book.AuthorId = author.Id;
            book.CategoryId = category.Id;

            var addedBook = repository.AddItem(book);

            return mapper.Map<Book, BookDto>(addedBook);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<BookDto> GetAll()
        {
            var books = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToList();
            return mapper.Map<List<Book>, List<BookDto>>(books);
        }


        public BookDto GetById(int id)
        {
            var book = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            return mapper.Map<BookDto>(book);
        }


        public List<BookDto> GetAllSimple()
        {
            var books = repository.GetAll();
            return mapper.Map<List<BookDto>>(books);
        }

        public List<BookDto> GetByAuthorName(string authorName)
        {
            var books = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToList();

            return mapper.Map<List<Book>, List<BookDto>>(books);
        }
        public List<BookDto> GetByCategoryName(string categoryName)
        {
            var books = repository.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToList();

            return mapper.Map<List<Book>, List<BookDto>>(books);
        }

        public List<BookDto> GetByTitle(string title)
        {
            var books = repository.Query()
                            .Include(b => b.Author)     
                            .Include(b => b.Category)  
                            .Where(b => b.Title.Contains(title))
                            .ToList();

            return mapper.Map<List<BookDto>>(books);
        }



        public BookDto GetByIdSimple(int id)
        {
            var book = repository.GetById(id);
            return mapper.Map<BookDto>(book);
        }

        public void Update(int id, BookDto item)
        {
            var existingBook = repository.GetById(id);
            if (existingBook == null)
                throw new Exception($"ספר עם מזהה {id} לא נמצא.");

            // טיפול במחבר
            var author = authorRepository.GetAll().FirstOrDefault(a => a.Name == item.AuthorName);
            if (author == null)
            {
                author = new Author { Name = item.AuthorName };
                author = authorRepository.AddItem(author);
            }

            // טיפול בקטגוריה
            var category = categoryRepository.GetAll().FirstOrDefault(c => c.Name == item.CategoryName);
            if (category == null)
            {
                category = new Category { Name = item.CategoryName };
                category = categoryRepository.AddItem(category);
            }

            var updatedBook = mapper.Map<BookDto, Book>(item);
            updatedBook.Id = id;
            updatedBook.AuthorId = author.Id;
            updatedBook.CategoryId = category.Id;

            repository.UpdateItem(id, updatedBook);
        }


    }
}
