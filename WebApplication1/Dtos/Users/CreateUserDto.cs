using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.Users
{
    public class CreateUserDto
    {

        
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
