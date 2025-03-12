using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using Xunit;
using WebApplication1.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1.Tests.UserControllerTests
{
    public class LoginUserTests
    {
        [Theory(DisplayName = "Deleted User Should Retorn Not Found")]
        [InlineData("teste@gmail.com", "12345")]
        [InlineData("naoexiste@gmail.com", "54321")]
        [InlineData("", "12345")]
        [InlineData("teste@gmail.com", "123")]
        public async Task LoginUser_ShouldReturn_OK_WhenValidCredentials(string email, string senha)
        {
            // Arrange
            var dto = TestsFunc.CriarDto(email, senha);

            //database na memoria
            var context = TestsFunc.CriarDbNaMemoria();

            await TestsFunc.AddUserToContext(context, "teste@gmail.com", "12345");

            var controller = new UserController(context);

            //Act
            var result = await controller.LoginUser(dto);

            switch (result)
            {
                case OkObjectResult okResult:
                    Assert.IsType<OkObjectResult>(result);
                    Assert.NotNull(okResult.Value);
                    Assert.Equal("Login efetuado com sucesso", GetMensagem(okResult.Value));
                    break;

                case UnauthorizedObjectResult unauthorizedresult:
                    Assert.IsType<UnauthorizedObjectResult>(result);
                    Assert.Equal("Senha inválida", GetMensagem(unauthorizedresult.Value));
                    break;

                case BadRequestObjectResult badRequest:
                    Assert.IsType<BadRequestObjectResult>(result);
                    Assert.Equal("O campo de email e senha são obrigatorios", GetMensagem(badRequest.Value));
                    break;
                case NotFoundObjectResult notfoundresult:
                    Assert.IsType<NotFoundObjectResult>(result);
                    Assert.Equal("Usuário não encontrado", GetMensagem(notfoundresult.Value));
                    break;

                default:
                    Assert.True(false, "Resultado inesperado: " + result.GetType().Name);
                    break;
            }
        }



        private string? GetMensagem(object? value)
        {
            return value?.GetType().GetProperty("mensagem")?.GetValue(value, null).ToString();
        }
        
    }
}
