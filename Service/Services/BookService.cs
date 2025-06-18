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
            // חיפוש מחבר קיים לפי שם
            var author = authorRepository.GetAll().FirstOrDefault(a => a.Name == item.AuthorName);
            if (author == null)
            {
                author = new Author { Name = item.AuthorName };
                author = authorRepository.AddItem(author);
            }

            // חיפוש קטגוריה קיימת לפי שם
            var category = categoryRepository.GetAll().FirstOrDefault(c => c.Name == item.CategoryName);
            if (category == null)
            {
                category = new Category { Name = item.CategoryName };
                category = categoryRepository.AddItem(category);
            }

            // מיפוי הספר מה-DTO ל-Entity והוספת הקשרים
            var book = mapper.Map<BookDto, Book>(item);
            book.AuthorId = author.Id;
            book.CategoryId = category.Id;

            // הוספת הספר למסד הנתונים
            var addedBook = repository.AddItem(book);

            // החזרת הספר שנוסף כ-DTO
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

        public BookDto GetByIdSimple(int id)
        {
            var book = repository.GetById(id); 
            return mapper.Map<BookDto>(book);
        }

        //public void Update(int id, BookDto item)
        //{
        //    repository.UpdateItem(id, mapper.Map<BookDto, Book>(item));
        //}
        public void Update(int id, BookDto item)
        {
            // שליפת הספר הקיים
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

            // מיפוי הנתונים מה-DTO לאובייקט ספר
            var bookToUpdate = mapper.Map<BookDto, Book>(item);
            bookToUpdate.Id = id; // חשוב: לעדכן את ה-ID
            bookToUpdate.AuthorId = author.Id;
            bookToUpdate.CategoryId = category.Id;

            // עדכון במסד
            repository.UpdateItem(id, bookToUpdate);
        }
    }
}

//using AutoMapper;
//using Common.Dto;
//using Repository.Entities;
//using Repository.Interfaces;
//using Service.Interfaces;


//namespace Service.Services
//{
//    public class BookService : IService<BookDto>
//    {

//        private readonly IRepository<Book> repository;
//        private readonly IMapper mapper;
//        public BookService(IRepository<Book> repository, IMapper map)
//        {
//            this.repository = repository;
//            this.mapper = map;
//        }

//        public BookDto AddItem(BookDto item)
//        {
//            return mapper.Map<Book, BookDto>(repository.AddItem(mapper.Map<BookDto, Book>(item)));
//        }

//        public void DeleteItem(int id)
//        {
//            repository.DeleteItem(id);
//        }

//        public List<BookDto> GetAll()
//        {
//            return mapper.Map<List<Book>, List<BookDto>>(repository.GetAll());
//        }

//        public BookDto GetById(int id)
//        {
//            return mapper.Map<Book, BookDto>(repository.GetById(id));

//        }

//        public void Update(int id, BookDto item)
//        {
//            repository.UpdateItem(id, mapper.Map<BookDto, Book>(item));
//        }
//    }
//}
