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
            // Book → BookDto
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ArrImage, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ImageUrl) ? null : File.ReadAllBytes(src.ImageUrl)));

            // BookDto → Book
            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())       
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

            // User → UserDto (למנהל שרואה רק את השם)
            CreateMap<User, UserDto>();

            // UserLoginDto → User (לצורך אימות התחברות, אם יש צורך)
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // לא מקבלים Id בהתחברות
        }
    }
}
