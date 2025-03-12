using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
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
        public async Task LoginUser_ShouldReturn_OK_WhenValidCredentials()
        {
            // Arrange
            var dto = CriarDto("teste@gmail.com", "12345");


            //database na memoria
            var context = CriarDbNaMemoria();

            await AddUserToContext(context, "teste@gmail.com", "12345");



            var controller = new UserController(context);


            //Act
            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var responseObject = okResult.Value;
            Assert.Equal("Login efetuado com sucesso", responseObject.GetType().GetProperty("mensagem")?.GetValue(responseObject, null));
        }


        [Fact(DisplayName = "Login user should return badrequest")]
        public async Task LoginUser_ShouldReturn_BadRequest_WhenEmptyField()
        {
            //Arranger
            var dto = CriarDto("", "1234");

            //crio o database e o contexto
            var context = CriarDbNaMemoria();

            var controller = new UserController(context);

            //act

            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O campo de email e senha são obrigatorios", okResult.Value?.GetType().GetProperty("mensagem")?.GetValue(okResult.Value, null).ToString());
        }


        [Fact(DisplayName = "Login User Should return Not Found")]
        public async Task LoginUser_ShouldReturn_NotFound_WhenNonExistingUser()
        {
            //Arrange
            var dto = CriarDto("teste@gmail.com", "123");

            var context = CreateContextNull();

            await AddUserToContext(context, "teste2@gmail.com", "123");


            var controller = new UserController(context);

            var result = await controller.LoginUser(dto);

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Usuário não encontrado", okResult.Value?.GetType().GetProperty("mensagem")?.GetValue(okResult.Value, null).ToString());
        }

        [Fact(DisplayName = "Login user should return unauthorized")]
        public async Task LoginUser_ShouldReturn_Unauthorized_WhenPasswordDoenstMatch()
        {
            //Arrange

            using var context = CriarDbNaMemoria();

            var dto = CriarDto("teste@gmail.com", "senha-certa");



            await AddUserToContext(context, "teste@gmail.com", "senha-incorreta");


            var controller = new UserController(context);

            //act
            var result = await controller.LoginUser(dto);

            var UnauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal("Senha inválida", UnauthorizedResult.Value?.GetType().GetProperty("mensagem")?.GetValue(UnauthorizedResult.Value,null).ToString());

        }


        private LoginDto CriarDto(string email, string senha)
        {
            
            var dto = new LoginDto()
            {
                Email = email,
                Senha = senha
            };

            return dto;
        }

        private AppDbContext CriarDbNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "Testes")
                    .Options;

            

            return new AppDbContext(options);
        }

        private async Task AddUserToContext(AppDbContext context, string email, string senha)
        {
            var senhahash = BCrypt.Net.BCrypt.HashPassword(senha);

            context.Add(new Users
            {
                Nome = "Higor",
                Email = email,
                SenhaHash = senhahash,
                RA = "20254952"
            });

            Console.WriteLine($"Senha: {senha}, Hash: {senhahash}");
            Console.WriteLine(BCrypt.Net.BCrypt.Verify(senha, BCrypt.Net.BCrypt.HashPassword(senha)));


            await context.SaveChangesAsync();

        }

        private AppDbContext CreateContextNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nome único para cada teste
                .Options;

            return new AppDbContext(options); // Banco de dados vazio


        }
    }
}
