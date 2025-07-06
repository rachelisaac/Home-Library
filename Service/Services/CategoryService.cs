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

        public async Task<CategoryDto> AddItem(CategoryDto item)
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

            var addedCategory = await repository.AddItem(category);
            return mapper.Map<Category, CategoryDto>(addedCategory);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var userId = currentUserService.GetUserId();
            var categories = await repository.GetAll();

            if (!currentUserService.IsAdmin())
            {
                categories = categories.Where(c => c.UserId == userId).ToList();
            }

            return mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetById(int id)
        {
            var userId = currentUserService.GetUserId();
            var category = await repository.GetById(id);

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

        public async Task Update(int id, CategoryDto item)
        {
            var userId = currentUserService.GetUserId();
            var existing = await repository.GetById(id);

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

            await repository.UpdateItem(id, updated);
        }
    }
}
