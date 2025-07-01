using AutoMapper;
using Common.Dto;
using Repository.Entities;

namespace Service.Services
{
    public class MyMapper : Profile
    {
        string path = Environment.CurrentDirectory + "/Images/";
        public MyMapper()
        {
            // Book → BookDto (הצגה ללקוח)
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ArrImage, opt => opt.MapFrom(src =>
                 string.IsNullOrEmpty(src.ImageUrl) ? null : File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Images", src.ImageUrl))));

            // BookDto → Book (קבלה מהלקוח)
            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.File != null ? src.File.FileName : null))
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());



            // Author → AuthorDto
            CreateMap<Author, AuthorDto>();

            // AuthorDto → Author
            CreateMap<AuthorDto, Author>();

            // Category → CategoryDto
            CreateMap<Category, CategoryDto>();

            // CategoryDto → Category
            CreateMap<CategoryDto, Category>();

            // User → UserDto 
            CreateMap<User, UserDto>();

            // UserDto → User
            CreateMap<UserDto, User>()
               .ForMember(dest => dest.Password, opt => opt.Ignore());

            // UserRegisterDto -> User
            CreateMap<UserRegisterDto, User>();

            // UserLoginDto → User (לצורך אימות התחברות, אם יש צורך)
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // UserUpdate → User
            CreateMap<UserUpdate, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
