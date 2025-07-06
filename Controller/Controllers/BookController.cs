using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService service;

        public BookController(IBookService service)
        {
            this.service = service;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<List<BookDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public async Task<BookDto> Get(int id)
        {
            return await service.GetById(id);
        }
        // GET: api/Book/simple
        [HttpGet("simple")]
        public async Task<List<BookDto>> GetSimple()
        {
            return await service.GetAllSimple();
        }

        // GET: api/Book/simple/5
        [HttpGet("simple/{id}")]
        public async Task<BookDto> GetSimpleById(int id)
        {
            return await service.GetByIdSimple(id);
        }
        // GET: api/Book/by-title?title=...
        [HttpGet("by-title")]
        public async Task<List<BookDto>> GetByTitle([FromQuery] string title)
        {
            return await service.GetByTitle(title);
        }

        // GET: api/Book/by-author?authorName=...
        [HttpGet("by-author")]
        public async Task<List<BookDto>> GetByAuthor([FromQuery] string authorName)
        {
            return await service.GetByAuthorName(authorName);
        }
        // GET: api/Book/by-category?categoryName=...
        [HttpGet("by-category")]
        public async Task<List<BookDto>> GetByCategory([FromQuery] string categoryName)
        {
            return await service.GetByCategoryName(categoryName);
        }

        [HttpPost]
        public async Task<BookDto> Post([FromForm] BookDto value)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Images/", value.File.FileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                 await value.File.CopyToAsync(fs);
                fs.Close();
            }

            return await service.AddItem(value);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromForm] BookDto value)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Images/", value.File.FileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await value.File.CopyToAsync(fs);  
            }

            await service.Update(id, value);  
        }


        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
        }

    }
}