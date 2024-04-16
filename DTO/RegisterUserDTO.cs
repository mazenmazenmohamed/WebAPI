using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
    
{
    public class RegisterUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; } 
        public string Email { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmed { get; set; }
        
    }
}
