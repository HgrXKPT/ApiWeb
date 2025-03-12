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
        [Fact(DisplayName = "Login User should return Ok")]
        public async Task LoginUser_ShouldReturn_OK_WhenLogged()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "teste@gmail.com",
                Senha = "123"
            };


            //database na memoria
            var context = CriarDbNaMemoria();

            await AddUserToContext(context, dto);

            await context.SaveChangesAsync();


            var controller = new UserController(context);


            //Act
            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var responseObject = okResult.Value;
            Assert.Equal("Login efetuado com sucesso", responseObject.GetType().GetProperty("mensagem")?.GetValue(responseObject, null));
        }


        [Fact(DisplayName = "Login user should return badrequest")]
        public async Task LoginUser_ShouldReturn_BadRequest_WhenLogged()
        {
            //Arranger
            var dto = new LoginDto()
            {
                Email = "",
                Senha = "123"
            };

            //crio o database e o contexto
            var context = CriarDbNaMemoria();
            await AddUserToContext(context, dto);

            var controller = new UserController(context);

            //act

            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O campo de email e senha são obrigatorios", okResult.Value?.GetType().GetProperty("mensagem")?.GetValue(okResult.Value, null).ToString());


        }

        private AppDbContext CriarDbNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "Testes")
                    .Options;

            ;

            return new AppDbContext(options);
        }

        private async Task AddUserToContext(AppDbContext context, LoginDto dto)
        {
      

            context.Add(new Users
            {
                Nome = "Higor",
                Email = "teste@gmail.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                RA = "20254952"
            });

            await context.SaveChangesAsync();

        }

    }
}
