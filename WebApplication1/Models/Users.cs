using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Users
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "O Campo de RA é obrigatorio")]
        public int RA { get; set; }

        [Required(ErrorMessage = "O campo SenhaHash é obrigatório.")]

        public string SenhaHash { get; set; }

        public int? EquipeId { get; set; }
        public Equipes? Equipe { get; set; }

    }
}
