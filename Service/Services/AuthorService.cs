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
        public AuthorService(IRepository<Author> repository, IRepository<Book> bookRepository, IMapper map)
        {
            this.repository = repository;
            this.bookRepository = bookRepository;
            this.mapper = map;
        }
        public AuthorDto AddItem(AuthorDto item)
        {
            return mapper.Map<Author, AuthorDto>(repository.AddItem(mapper.Map<AuthorDto, Author>(item)));
        }

        public void DeleteItem(int id)
        {
            var hasBooks = bookRepository.GetAll().Any(b => b.AuthorId == id);
            if (hasBooks)
                throw new Exception("לא ניתן למחוק סופר שיש לו ספרים.");
            repository.DeleteItem(id);
        }


        public List<AuthorDto> GetAll()
        {
            return mapper.Map<List<Author>, List<AuthorDto>>(repository.GetAll());
        }

        public AuthorDto GetById(int id)
        {
            return mapper.Map<Author, AuthorDto>(repository.GetById(id));

        }

        public void Update(int id, AuthorDto item)
        {
            repository.UpdateItem(id, mapper.Map<AuthorDto, Author>(item));

        }
    }
}
