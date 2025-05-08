using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IService<AuthorDto> service;
        public AuthorController(IService<AuthorDto> service)
        {
            this.service = service;
        }
        // GET: api/<AuthorController>
        [HttpGet]
        public List<AuthorDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}")]
        public AuthorDto Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<AuthorController>
        [HttpPost]
        public AuthorDto Post([FromBody] AuthorDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<AuthorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] AuthorDto value)
        {
            service.Update(id, value);
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
