using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;


namespace Service.Services
{
    public class AuthorService : IService<AuthorDto>
    {
        private readonly IRepository<Author> repository;
        private readonly IRepository<Book> bookRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        public AuthorService(IRepository<Author> repository, IRepository<Book> bookRepository, IMapper map, ICurrentUserService currentUserService)
        {
            this.repository = repository;
            this.bookRepository = bookRepository;
            this.mapper = map;
            this.currentUserService = currentUserService;
        }
        public AuthorDto AddItem(AuthorDto item)
        {
            var userId = currentUserService.GetUserId();
            var author = mapper.Map<AuthorDto, Author>(item);
            author.UserId = (int)userId;
            var addedAuthor = repository.AddItem(author);
            return mapper.Map<Author, AuthorDto>(addedAuthor);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<AuthorDto> GetAll()
        {
            var userId = currentUserService.GetUserId();

            var authors = repository.GetAll();
            if (!currentUserService.IsAdmin())
            {
                authors = authors.Where(a => a.UserId == userId).ToList();
            }

            return mapper.Map<List<AuthorDto>>(authors);
        }


        public AuthorDto GetById(int id)
        {
            var userId = currentUserService.GetUserId();
            var author = repository.GetById(id);

            if (author == null)
            {
                throw new Exception("המחבר לא נמצא.");
            }

            if (author.UserId != userId && !currentUserService.IsAdmin())
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לצפות במחבר הזה.");
            }

            return mapper.Map<AuthorDto>(author);
        }


        public void Update(int id, AuthorDto item)
        {
            var userId = currentUserService.GetUserId();
            var existing = repository.GetById(id);

            if (existing == null)
            {
                throw new Exception("המחבר לא נמצא.");
            }

            if (existing.UserId != userId && !currentUserService.IsAdmin())
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לעדכן את המחבר הזה.");
            }

            var updated = mapper.Map<Author>(item);
            updated.Id = id;
            updated.UserId = existing.UserId;
            repository.UpdateItem(id, updated);
        }

    }
}
