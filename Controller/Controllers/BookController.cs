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
        public List<BookDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public BookDto Get(int id)
        {
            return service.GetById(id);
        }
        // GET: api/Book/simple
        [HttpGet("simple")]
        public List<BookDto> GetSimple()
        {
            return service.GetAllSimple();
        }

        // GET: api/Book/simple/5
        [HttpGet("simple/{id}")]
        public BookDto GetSimpleById(int id)
        {
            return service.GetByIdSimple(id);
        }
        // GET: api/Book/by-title?title=...
        [HttpGet("by-title")]
        public List<BookDto> GetByTitle([FromQuery] string title)
        {
            return service.GetByTitle(title);
        }
        // GET: api/Book/by-author?authorName=...
        [HttpGet("by-author")]
        public List<BookDto> GetByAuthor([FromQuery] string authorName)
        {
            return service.GetByAuthorName(authorName);
        }
        // GET: api/Book/by-category?categoryName=...
        [HttpGet("by-category")]
        public List<BookDto> GetByCategory([FromQuery] string categoryName)
        {
            return service.GetByCategoryName(categoryName);
        }

        [HttpPost]
        public BookDto Post([FromForm] BookDto value)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Images", value.File.FileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
        {
                value.File.CopyTo(fs);
                fs.Close();
            }

            return service.AddItem(value);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BookDto value)
        {
            service.Update(id, value);
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
