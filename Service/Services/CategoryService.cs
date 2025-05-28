using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;


namespace Service.Services
{
    public class CategoryService : IService<CategoryDto>
    {

        private readonly IRepository<Category> repository;
        private readonly IMapper mapper;
        public CategoryService(IRepository<Category> repository, IMapper map)
        {
            this.repository = repository;
            this.mapper = map;
        }
        public CategoryDto AddItem(CategoryDto item)
        {
            return mapper.Map<Category, CategoryDto>(repository.AddItem(mapper.Map<CategoryDto, Category>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<CategoryDto> GetAll()
        {
            return mapper.Map<List<Category>, List<CategoryDto>>(repository.GetAll());
        }

        public CategoryDto GetById(int id)
        {
            return mapper.Map<Category, CategoryDto>(repository.GetById(id));

        }

        public void Update(int id, CategoryDto item)
        {
            repository.UpdateItem(id, mapper.Map<CategoryDto, Category>(item));

        }
    }
}

