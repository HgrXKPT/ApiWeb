using System.ComponentModel.DataAnnotations;
using WebApplication1.Utils;

namespace WebApplication1.Models
{
    public class Equipes
    {
        private string _nomeEquipe;
        private int _quantidadeMembros;
        private int _quantidadeProjetos;


        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "O Nome da equipe é obrigatório")]
        public string NomeEquipe
        {
            get => _nomeEquipe;
            set
            {
                Validacoes.VerificarEspacoVazioOuNulo(value, "O Campo 'NomeEquipe' está nulo ou com espaço em branco ");
                _nomeEquipe = value;
            }
        }
        [Required(ErrorMessage = "É obrigatório informar a quantidade de membros na equipe")]
        public int? QuantidadeMembros
        {
            get => _quantidadeMembros;
            set
            {
                ValidarQuantidadeNegativa(value, "A quantidade de membros não pode ser negativa.");
                _quantidadeMembros = value ?? 0;
            }
        }

        [Required(ErrorMessage = "É obrigatório informar a quantidade de projetos da equipe")]
        public int? QuantidadeProjetos
        {
            get => _quantidadeProjetos;
            set
            {
                ValidarQuantidadeNegativa(value, "A quantidade de projetos não pode ser negativa.");
                _quantidadeProjetos = value ?? 0;
            }
        }

        public ICollection<Users> Users
        {
            get; set;
        }

        public ICollection<Projetos> Projetos
        {
            get; set;
        }

        private void ValidarQuantidadeNegativa(int? valor, string mensagem)
        {
            if(valor.HasValue && valor < 0){
                throw new ArgumentException(mensagem);
            }
        }
    }
}
