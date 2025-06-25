using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class UserLoginDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
