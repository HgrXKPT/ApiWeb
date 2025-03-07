using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.Users
{
    public class CreateUserDto
    {

        [Required(ErrorMessage = "O campo nome é obrigatorio")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatorio")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "O campo senha é obrigatorio")]
        [MinLength(6,ErrorMessage ="A senha deve ter no minimo 6 caracteres")]
        [MaxLength(50,ErrorMessage ="A senha deve ter no máximo 50 caracteres")]
        public string Senha { get; set; }
    }
}
