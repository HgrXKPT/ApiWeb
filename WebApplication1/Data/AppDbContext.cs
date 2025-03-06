using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Equipes> Equipes { get; set; }
        public DbSet<Projetos> Projetos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Equipe)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.EquipeId);

            modelBuilder.Entity<Projetos>()
                .HasOne(p => p.Equipe)
                .WithMany(e => e.Projetos)
                .HasForeignKey(e => e.EquipeId);
                
        }
    }
}
