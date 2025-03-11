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

            // Configurando o banco de dados em memória para os teste
            using var context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "Testes")
        .Options
);


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

        [Fact]
        public async Task LoginUser_ShouldReturn_OK_WhenLogged()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "Higor@gmail.com",
                Senha = "123"
            };


            //database na memoria
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Testes")
                .Options;

            var context = new AppDbContext(options);

            
            //add user
            context.Add(new Users
            {
                Nome = "Higor",
                Email = "Higor@gmail.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                RA = "20254952"


            });

            await context.SaveChangesAsync();


            var controller = new UserController(context);


            //Act
            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var responseObject = okResult.Value;
            Assert.Equal("Login efetuado com sucesso", responseObject.GetType().GetProperty("mensagem")?.GetValue(responseObject, null));



        }

    }
}
