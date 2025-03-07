using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.Users
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
