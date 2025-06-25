using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IConfiguration config;

        public UserController(IUserService service)
        {
            this.service = service;
            this.config = config;
        }

        // GET: api/<UserController>
        [HttpGet]
        public List<UserDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public UserDto Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public UserDto Post([FromBody] UserRegisterDto value)
        {
            return service.AddItem(value);
        }


        // POST: User login and JWT generation
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            UserLoginDto validatedUser = Authenticate(user);
            if (validatedUser != null)
            {
                var token = GenerateToken(validatedUser);
                return Ok(token);
            }
            return BadRequest("user not found");
        }


        //יש בעיה בפונקציה הזו היא אמורה לקבל userdto
        private string GenerateToken(UserLoginDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.Email, user.Email ?? ""),
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserLoginDto Authenticate(UserLoginDto user)
        {
            return service.Authenticate(user.Email, user.Password);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserDto value)
        {
            service.Update(id, value);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
