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
using WebApplication1.Utils;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Tests.Controllers
{


    public class CreateUserTests
    {
        [Theory(DisplayName = "Create User Should Retorn Bad Request/ Created at Action")]
        [InlineData("higor", "Teste@gmail.com", "12345")]
        [InlineData("", "Teste@gmail.com", "12345")]
        [InlineData("Higor", "Teste@gmail.com", "12345")]
        public async Task CreateUser_ShouldReturn_CreatedAtAction_WhenCreated(string nome, string email, string senha)
        {
            // Arrange
            var dto = CriarDtoUser(nome, email, senha);

            var context = TestsFunc.CriarDbNaMemoria();
            await TestsFunc.AddUserToContext(context, nome, "Teste@gmail.com", "12345");

            var controller = new UserController(context);

            // Act
            var result = await controller.CreateUser(dto);

            

            switch (result)
            {
                case CreatedAtActionResult createdAtActionResult:

                    var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                    Assert.NotNull(createdAtActionResult.Value);

                    var user = Assert.IsType<Users>(createdResult.Value);
                    Assert.Equal("Higor", user.Nome);
                    Assert.Equal("Teste@gmail.com", user.Email);

                    break;

                case BadRequestObjectResult badRequestObjectResult:

                    Assert.IsType<BadRequestObjectResult>(result);
                    Assert.NotNull(badRequestObjectResult.Value);

                    Assert.Equal("Usuario não identificado", TestsFunc.GetMensagem(badRequestObjectResult.Value));
                    break;

                case ConflictObjectResult conflictObjectResult:
                    Assert.IsType<ConflictObjectResult>(result);
                    Assert.NotNull(conflictObjectResult.Value);

                    Assert.Equal("Esse email já está cadastrado", TestsFunc.GetMensagem(conflictObjectResult.Value));

                    break;
                default:
                    break;

            }

        }



        private static CreateUserDto CriarDtoUser(string nome, string email, string senha)
        {

            var dto = new CreateUserDto()
            {
                Nome = nome,
                Email = email,
                Senha = senha
            };

            return dto;
        }

    }
}
