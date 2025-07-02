using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IService<CategoryDto> service;

        public CategoryController(IService<CategoryDto> service)
        {
            this.service = service;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public List<CategoryDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public CategoryDto Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public CategoryDto Post([FromBody] CategoryDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CategoryDto value)
        {
            service.Update(id, value);
        }

        //// DELETE api/<CategoryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    service.DeleteItem(id);
        //}
    }
}
