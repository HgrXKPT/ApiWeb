namespace WebApplication1.Models
{
    public class Projetos
    {
        public int Id { get; set; }

        public string NomeProjeto { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataFinal { get; set; }

        public int EquipeId { get; set; }
        public Equipes Equipe { get; set; }
    }
}
