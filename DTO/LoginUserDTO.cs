using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class LoginUserDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
