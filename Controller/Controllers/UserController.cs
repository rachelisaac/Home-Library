using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
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

        public UserController(IUserService service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }

        // GET: api/<UserController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public List<UserDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<UserController>/5
        [Authorize(Roles = "Admin")]
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

        //[HttpPost("add-admin")]
        //public IActionResult AddAdmin(CreateAdminDto dto)
        //{

        //  //כאן צריך להוסיף פונקציה להוספת מנהל יש צןרך לטפל גם בשכבות האחרות אם ממשים את הפונקציה הזו
        //}

        // POST: User login and JWT generation
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userlogin)
        {
            User user = Authenticate(userlogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return BadRequest("User not found or wrong password");
        }




        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),     
                 new Claim(ClaimTypes.Email, user.Email ?? ""),
                 new Claim(ClaimTypes.Name, user.Name ?? ""),
                 new Claim(ClaimTypes.Role, user.Role.ToString())               
            };

            // יצירת הטוקן
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            // המרה לטקסט
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private User Authenticate(UserLoginDto user)
        {
            return service.Authenticate(user.Email, user.Password); 
        }


        // PUT api/<UserController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserUpdate value)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != id.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); 
            }
            service.Update(id, value);
            return Ok(); 
        }

        // DELETE api/<UserController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
