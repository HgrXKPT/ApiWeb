namespace WebApplication1.Models
{
    public class Equipes
    {
        public int Id { get; set; }

        public string NomeEquipe { get; set; }
        public int QuantidadeMembros { get; set; }

        public int QuantidadeProjetos { get; set; }

        public ICollection<Users> Users { get; set; }

        public ICollection<Projetos> Projetos { get; set; }
    }
}
