using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using Xunit;
using BCrypt;

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

            var controller = CriarBancoDeDadosNaMemoria();

            // Act
            var result = await controller.CreateUser(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value);

            var user = Assert.IsType<Users>(createdResult.Value);
            Assert.Equal("Higor", user.Nome);
            Assert.Equal("Teste@gmail.com", user.Email);
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_BadRequest_WhenCreated()
        {
            var dto = new CreateUserDto
            {
                Nome = "",
                Email = "Teste@gmail.com",
                Senha = "12345"
            };

            var controller = CriarBancoDeDadosNaMemoria();

            var result = await controller.CreateUser(dto);

            //Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);


        }



        private UserController CriarBancoDeDadosNaMemoria()
        {
            var context = new AppDbContext(
                    new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Testes")
            .Options);

            return new UserController(context);

        }

    }
}
