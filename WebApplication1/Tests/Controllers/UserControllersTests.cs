using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class UserControllersTests
    {
        [Fact]
        public async Task CreateUser_ShouldReturn_CreatedAtAction_WhenCreated()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Nome = "Higor",
                Email = "Teste@gmail.com",
                Senha = "12345"
            };

            // Configurando o banco de dados em memória para os testes
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Testes")
                .Options;

            using var context = new AppDbContext(options);

            // Criando uma instância do UserController com o contexto
            var controller = new UserController(context);

            // Act
            var result = await controller.CreateUser(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value);

            var user = Assert.IsType<Users>(createdResult.Value);
            Assert.Equal("Higor", user.Nome);
            Assert.Equal("Teste@gmail.com", user.Email);
        }

    }
}
