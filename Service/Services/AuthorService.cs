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

        public async Task<AuthorDto> AddItem(AuthorDto item)
        {
            var currentUserId =  currentUserService.GetUserId();
            var author = mapper.Map<AuthorDto, Author>(item);

            if ( currentUserService.IsAdmin())
            {
                if (item.UserId == null)
                {
                    throw new Exception("כמנהל חובה לציין למי שייך המחבר (UserId).");
                }
                author.UserId = item.UserId.Value;
            }
            else
            {
                author.UserId = (int)currentUserId;
            }

            var addedAuthor = await repository.AddItem(author);
            return mapper.Map<Author, AuthorDto>(addedAuthor);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<AuthorDto>> GetAll()
        {
            var userId =  currentUserService.GetUserId();
            var authors = await repository.GetAll();

            if (! currentUserService.IsAdmin())
            {
                authors = authors.Where(a => a.UserId == userId).ToList();
            }

            return mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetById(int id)
        {
            var userId =  currentUserService.GetUserId();
            var author = await repository.GetById(id);

            if (author == null)
            {
                throw new Exception("המחבר לא נמצא.");
            }

            if (author.UserId != userId && ! currentUserService.IsAdmin())
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לצפות במחבר הזה.");
            }

            return mapper.Map<AuthorDto>(author);
        }

        public async Task Update(int id, AuthorDto item)
        {
            var userId =  currentUserService.GetUserId();
            var existing = await repository.GetById(id);

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
            await repository.UpdateItem(id, updated);
        }
    }
}
