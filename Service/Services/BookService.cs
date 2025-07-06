using AutoMapper;
using Common.Dto;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

public class BookService : IBookService
{
    private readonly IRepository<Book> repository;
    private readonly IRepository<Author> authorRepository;
    private readonly IRepository<Category> categoryRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IMapper mapper;

    public BookService(ICurrentUserService currentUserService, IRepository<Book> repository, IRepository<Author> authorRepository, IRepository<Category> categoryRepository, IMapper map)
    {
        this.currentUserService = currentUserService;
        this.repository = repository;
        this.authorRepository = authorRepository;
        this.categoryRepository = categoryRepository;
        this.mapper = map;
    }

    public async Task<BookDto> AddItem(BookDto item)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();
        int targetUserId = isAdmin ? (int)(item.UserId ?? throw new Exception("כמנהל, חובה לציין למי שייך הספר (UserId).")) : (int)currentUserId;

        var author = await authorRepository.Query()
            .FirstOrDefaultAsync(a => a.Name == item.AuthorName && a.UserId == targetUserId);

        if (author == null)
        {
            author = new Author { Name = item.AuthorName, UserId = targetUserId };
            author = await authorRepository.AddItem(author);
        }

        var category = await categoryRepository.Query()
            .FirstOrDefaultAsync(c => c.Name == item.CategoryName && c.UserId == targetUserId);

        if (category == null)
        {
            category = new Category { Name = item.CategoryName, UserId = targetUserId };
            category = await categoryRepository.AddItem(category);
        }

        var book = mapper.Map<BookDto, Book>(item);
        book.AuthorId = author.Id;
        book.CategoryId = category.Id;
        book.UserId = targetUserId;

        var addedBook = await repository.AddItem(book);
        return mapper.Map<Book, BookDto>(addedBook);
    }

    public async Task DeleteItem(int id)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var book = await repository.GetById(id);
        if (book == null)
            throw new Exception($"ספר עם מזהה {id} לא נמצא.");

        if (!isAdmin && book.UserId != currentUserId)
            throw new UnauthorizedAccessException("אין לך הרשאה למחוק ספר זה.");

        var authorId = book.AuthorId;
        var categoryId = book.CategoryId;
        var userId = book.UserId;

        await repository.DeleteItem(id);

        var isAuthorUsed = await repository.Query()
            .AnyAsync(b => b.AuthorId == authorId && b.UserId == userId);

        if (!isAuthorUsed)
            await authorRepository.DeleteItem(authorId);

        var isCategoryUsed = await repository.Query()
            .AnyAsync(b => b.CategoryId == categoryId && b.UserId == userId);

        if (!isCategoryUsed)
            await categoryRepository.DeleteItem(categoryId);
    }

  
    public async Task<List<BookDto>> GetAll()
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

        var list = await booksQuery.ToListAsync();

        return mapper.Map<List<BookDto>>(list);
    }

    public async Task<BookDto> GetById(int id)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var query = repository.Query()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.Id == id);

        if (!isAdmin)
            query = query.Where(b => b.UserId == currentUserId);

        var book = await query.FirstOrDefaultAsync();
        return mapper.Map<BookDto>(book);
    }

    public async Task<List<BookDto>> GetAllSimple()
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var books = await repository.GetAll();

        if (!isAdmin)
            books = books.Where(b => b.UserId == currentUserId).ToList();

        return mapper.Map<List<BookDto>>(books);
    }

    public async Task<BookDto> GetByIdSimple(int id)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var book = await repository.GetById(id);
        if (book == null)
            return null;

        if (!isAdmin && book.UserId != currentUserId)
            throw new UnauthorizedAccessException("אין לך הרשאה לגשת לספר הזה.");

        return mapper.Map<BookDto>(book);
    }

    public async Task<List<BookDto>> GetByAuthorName(string authorName)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var booksQuery = repository.Query()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.Author.Name.Contains(authorName));

        if (!isAdmin)
            booksQuery = booksQuery.Where(b => b.UserId == currentUserId);

        var books = await booksQuery.ToListAsync();
        return mapper.Map<List<BookDto>>(books);
    }

    public async Task<List<BookDto>> GetByCategoryName(string categoryName)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var booksQuery = repository.Query()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.Category.Name.Contains(categoryName));

        if (!isAdmin)
            booksQuery = booksQuery.Where(b => b.UserId == currentUserId);

        var books = await booksQuery.ToListAsync();
        return mapper.Map<List<BookDto>>(books);
    }

    public async Task<List<BookDto>> GetByTitle(string title)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var booksQuery = repository.Query()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.Title.Contains(title));

        if (!isAdmin)
            booksQuery = booksQuery.Where(b => b.UserId == currentUserId);

        var books = await booksQuery.ToListAsync();
        return mapper.Map<List<BookDto>>(books);
    }

    public async Task Update(int id, BookDto item)
    {
        var currentUserId = currentUserService.GetUserId();
        var isAdmin = currentUserService.IsAdmin();

        var existingBook = await repository.GetById(id);
        if (existingBook == null)
            throw new Exception($"ספר עם מזהה {id} לא נמצא.");

        if (!isAdmin && existingBook.UserId != currentUserId)
            throw new UnauthorizedAccessException("אין לך הרשאה לעדכן ספר זה.");

        var bookOwnerId = existingBook.UserId;
        var oldCategoryId = existingBook.CategoryId;
        var oldAuthorId = existingBook.AuthorId;

        var author = await authorRepository.Query()
            .FirstOrDefaultAsync(a => a.Name == item.AuthorName);
        if (author == null)
        {
            author = new Author { Name = item.AuthorName, UserId = bookOwnerId };
            author = await authorRepository.AddItem(author);
        }

        var category = await categoryRepository.Query()
            .FirstOrDefaultAsync(c => c.Name == item.CategoryName);
        if (category == null)
        {
            category = new Category { Name = item.CategoryName, UserId = bookOwnerId };
            category = await categoryRepository.AddItem(category);
        }

        var updatedBook = mapper.Map<BookDto, Book>(item);
        updatedBook.Id = id;
        updatedBook.AuthorId = author.Id;
        updatedBook.CategoryId = category.Id;
        updatedBook.UserId = existingBook.UserId;

        await repository.UpdateItem(id, updatedBook);

        if (oldCategoryId != category.Id)
        {
            var isCategoryUsed = await repository.Query()
                .AnyAsync(b => b.CategoryId == oldCategoryId && b.UserId == bookOwnerId);
            if (!isCategoryUsed)
                await categoryRepository.DeleteItem(oldCategoryId);
        }

        if (oldAuthorId != author.Id)
        {
            var isAuthorUsed = await repository.Query()
                .AnyAsync(b => b.AuthorId == oldAuthorId && b.UserId == bookOwnerId);
            if (!isAuthorUsed)
                await authorRepository.DeleteItem(oldAuthorId);
        }
    }
}
