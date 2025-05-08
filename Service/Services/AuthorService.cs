using AutoMapper;
using Common.Dto;
using Nest;
using Repository.Entities;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthorService : IService<AuthorDto>
    {

        private readonly IRepository<Author> repository;
        private readonly IMapper mapper;
        public AuthorService(IRepository<Author> repository, IMapper map)
        {
            this.repository = repository;
            this.mapper = map;
        }
        public AuthorDto AddItem(AuthorDto item)
        {
            return mapper.Map<Author, AuthorDto>(repository.AddItem(mapper.Map<AuthorDto, Author>(item)));
        }

        public void DeleteItem(int id)
        {
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
