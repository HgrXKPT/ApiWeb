using System.ComponentModel.DataAnnotations;
using WebApplication1.Utils;

namespace WebApplication1.Models
{
    public class Users
    {


        private string _nome;
        private string _email;
        private string _ra;
        private string _senhahash;


        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome
        {
            get => _nome;
            set
            {
               
                Validacoes.VerificarEspacoVazioOuNulo(value, "Campo nome nulo ou com somente espaços em branco");
                _nome = value.Trim();
            }
        }


        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        public string Email
        {
            get => _email;
            set
            {

                _email = value.Trim();
            }
        }


        [Required(ErrorMessage = "O Campo de RA é obrigatorio")]
        public string RA
        {
            get => _ra;
            set
            {

                _ra = value;
            }
        }

        [Required(ErrorMessage = "O campo SenhaHash é obrigatório.")]

        public string SenhaHash
        {
            get => _senhahash;

            set
            {

                _senhahash = value;
            }
        }

        public int? EquipeId
        {
            get; set;
        }
        public Equipes? Equipe
        {
            get; set;
        }

        


    }
}
