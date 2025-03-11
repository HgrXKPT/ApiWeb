using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.UserControllerTests
{
    public class LoginUserTests
    {
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
