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
    public class BookService : IService<BookDto>
    {

        private readonly IRepository<Book> repository;
        private readonly IMapper mapper;
        public BookService(IRepository<Book> repository, IMapper map)
        {
            this.repository = repository;
            this.mapper = map;
        }
        public BookDto AddItem(BookDto item)
        {
            return mapper.Map<Book, BookDto>(repository.AddItem(mapper.Map<BookDto, Book>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<BookDto> GetAll()
        {
            return mapper.Map<List<Book>, List<BookDto>>(repository.GetAll());
        }

        public BookDto GetById(int id)
        {
            return mapper.Map<Book, BookDto>(repository.GetById(id));

        }

        public void Update(int id, BookDto item)
        {
            repository.UpdateItem(id, mapper.Map<BookDto, Book>(item));

        }
    }
}
