using System.ComponentModel.DataAnnotations;
using WebApplication1.Utils;

namespace WebApplication1.Models
{
    public class Projetos
    {
        private string _nomeProjeto;
        private string _descricao;
        private DateTime _dataCriacao;
        private DateTime _dataFinal;

        public int Id
        {
            get; set;
        }

        
        [Required(ErrorMessage = "O nome do projeto é obrigatório")]
        public string NomeProjeto
        {
            get => _nomeProjeto;
            set
            {
                Validacoes.VerificarEspacoVazioOuNulo(value, "O Campo 'NomeProjeto' está nulo ou com espaço em branco ");
                _nomeProjeto = value;
            }
        }

        [Required(ErrorMessage = "O projeto deve possui uma descrição")]
        public string Descricao
        {
            get => _descricao;
            set
            {
                Validacoes.VerificarEspacoVazioOuNulo(value, "O Campo 'Descrição' está nulo ou com espaço em branco ");
                _descricao = value;
            }
        }

        [Required(ErrorMessage = "O projeto deve possuir data de criação")]
        public DateTime DataCriacao
        {
            get => _dataCriacao;
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("A data de criação não pode ser no futuro");
                }
                _dataCriacao = value;
            }
        }

        [Required(ErrorMessage = "O projeto deve possuir uma data final")]
        public DateTime DataFinal
        {
            get=> _dataFinal;
            set
            {
                if (value < _dataCriacao)
                {
                    throw new Exception("A data final deve ser depois que a data inicial");
                }

                _dataFinal = value;
            }
        }

        public int EquipeId
        {
            get; set;
        }
        public Equipes Equipe
        {
            get; set;
        }
    }
}
