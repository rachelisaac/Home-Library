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
        private readonly ICurrentUserService currentUserService; 

        public CategoryService(IRepository<Category> repository, IMapper map, ICurrentUserService currentUserService)
        {
            this.repository = repository;
            this.mapper = map;
            this.currentUserService = currentUserService; 
        }


        public CategoryDto AddItem(CategoryDto item)
        {
            var currentUserId = currentUserService.GetUserId();
            var category = mapper.Map<CategoryDto, Category>(item);

            if (!currentUserService.IsAdmin())
            {
                category.UserId = (int)currentUserId;
            }
            else
            {
                if (item.UserId == null)
                {
                    throw new Exception("כמנהל, חובה לציין למי שייכת הקטגוריה (UserId).");
                }
                category.UserId = item.UserId.Value;
            }
            var addedCategory = repository.AddItem(category);
            return mapper.Map<Category, CategoryDto>(addedCategory);
        }


        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<CategoryDto> GetAll()
        {
            var userId = currentUserService.GetUserId();

            var categories = repository.GetAll();
            if (!currentUserService.IsAdmin())
            {
                categories = categories.Where(c => c.UserId == userId).ToList();
            }

            return mapper.Map<List<CategoryDto>>(categories);
        }

        public CategoryDto GetById(int id)
        {
            var userId = currentUserService.GetUserId();
            var category = repository.GetById(id);

            if (category == null)
            {
                throw new Exception("הקטגוריה לא נמצאה.");
            }

            if (category.UserId != userId && !currentUserService.IsAdmin())
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לצפות בקטגוריה הזו.");
            }

            return mapper.Map<CategoryDto>(category);
        }

        public void Update(int id, CategoryDto item)
        {
            var userId = currentUserService.GetUserId();
            var existing = repository.GetById(id);

            if (existing == null)
            {
                throw new Exception("הקטגוריה לא נמצאה.");
            }

            if (existing.UserId != userId && !currentUserService.IsAdmin())
            {
                throw new UnauthorizedAccessException("אין לך הרשאה לעדכן את הקטגוריה הזו.");
            }

            var updated = mapper.Map<Category>(item);
            updated.Id = id;
            updated.UserId = existing.UserId;
            repository.UpdateItem(id, updated);
        }


    }
}

