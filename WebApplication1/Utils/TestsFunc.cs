using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;

namespace WebApplication1.Utils
{
    public class TestsFunc
    {

        public static string? GetMensagem(object? value)
        {
            return value?.GetType().GetProperty("mensagem")?.GetValue(value, null).ToString();
        }

        public static LoginDto CriarDto(string email, string senha)
        {

            var dto = new LoginDto()
            {
                Email = email,
                Senha = senha
            };

            return dto;
        }

        public static AppDbContext CriarDbNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "Testes")
                    .Options;



            return new AppDbContext(options);
        }

        public static async Task AddUserToContext(AppDbContext context, string nome, string email, string senha)
        {
            var senhahash = BCrypt.Net.BCrypt.HashPassword(senha);

            context.Add(new Users
            {
                Nome = nome,
                Email = email,
                SenhaHash = senhahash,
                RA = "20254952"
            });

            await context.SaveChangesAsync();

        }
    }
}
