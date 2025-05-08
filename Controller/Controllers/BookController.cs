using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IService<BookDto> service;

        public BookController(IService<BookDto> service)
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

        // POST api/<BookController>
        [HttpPost]
        public BookDto Post([FromBody] BookDto value)
        {
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
