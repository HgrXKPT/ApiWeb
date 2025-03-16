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
using Moq;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Tests.UserControllerTests
{
    public class LoginUserTests
    {
        [Theory(DisplayName = "Login User Should Return OK/NOTFOUND/BADREQUEST/UNAUTHORIZED")]
        [InlineData("login@gmail.com", "123456")]
        [InlineData("naoexiste@gmail.com", "543210")]
        [InlineData("", "12345")]
        [InlineData("SenhaInvalida@gmail.com", "1234567")]
        public async Task LoginUser_ShouldReturn_OK_WhenValidCredentials(string email, string senha)
        {

            var mockService = new Mock<IUserService>();

            mockService.Setup(s => s.LoginUser(It.IsAny<LoginDto>()))
                .ReturnsAsync((LoginDto dto) =>
                {
                    if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Senha))
                        return new BadRequestObjectResult("O campo de email e senha são obrigatorios");

                    if (dto.Email == "naoexiste@gmail.com")
                        return new NotFoundObjectResult("Usuário não encontrado");

                    if (dto.Email == "SenhaInvalida@gmail.com" && dto.Senha != "123")
                        return new UnauthorizedObjectResult("Senha incorreta");

                    return dto.Email == "login@gmail.com" && dto.Senha == "123456"
                       ? new OkObjectResult("Login efetuado com sucesso")
                       : throw new ArgumentException("Error inesperado");

                });
                

            // Arrange
            var dto = TestsFunc.CriarDto(email, senha);

  

            //Act
            var result = await mockService.Object.LoginUser(dto);

            switch (result)
            {
                case OkObjectResult okResult:
                    Assert.IsType<OkObjectResult>(okResult);
                    Assert.Equal("Login efetuado com sucesso", okResult.Value);
                    break;

                case UnauthorizedObjectResult unauthorizedresult:
                    Assert.IsType<UnauthorizedObjectResult>(unauthorizedresult);
                    Assert.Equal("Senha incorreta", unauthorizedresult.Value);
                    break;

                case BadRequestObjectResult badRequest:
                    Assert.IsType<BadRequestObjectResult>(badRequest);
                    Assert.Equal("O campo de email e senha são obrigatorios", badRequest.Value);
                    break;
                case NotFoundObjectResult notfoundresult:
                    Assert.IsType<NotFoundObjectResult>(notfoundresult);
                    Assert.Equal("Usuário não encontrado", notfoundresult.Value);
                    break;

              
            }
            
        }



        
        
    }
}
