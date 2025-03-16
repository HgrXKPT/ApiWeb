using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Formats.Asn1;
using WebApplication1.Controllers;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using WebApplication1.Utils;
using Xunit;

namespace WebApplication1.Tests.UserControllerTests
{
    public class DeleteUserTests
    {

        //Criação de teste para validar o Delete User
        [Theory(DisplayName = "Deleted User Should Retorn Not Found")]
        [InlineData("teste@gmail.com", "12345", 1)]
        [InlineData("naoexiste@gmail.com", "54321", 0)]
        public async Task DeleteUser_ShouldReturn_Ok_WhenDeleted(string email, string senha, int idValid)
        {

            var mockService = new Mock<IUserService>();

            mockService.Setup(s => s.DeleteUserById(It.IsAny<int>()))
                .ReturnsAsync((int id) => id == 1
                    ? new OkObjectResult("Usuario Deletado")
                    : new NotFoundObjectResult("Usuario Nao encontrado "));


            //arrange

            var user = await mockService.Object.DeleteUserById(idValid);
            //Act
            switch (user)
            {
                case OkObjectResult okResult:

                    Assert.IsType<OkObjectResult>(okResult);
                    Assert.Equal("Usuario Deletado", okResult.Value);
                    break;

                case NotFoundObjectResult notFound:

                    Assert.IsType<NotFoundObjectResult>(notFound);
                    Assert.Equal("Usuario Nao encontrado", notFound.Value);

                    break;
                default:
                    Assert.Fail("Tipo de resultado inesperado.");
                    break;
            }
            ;

        }


    }
}
