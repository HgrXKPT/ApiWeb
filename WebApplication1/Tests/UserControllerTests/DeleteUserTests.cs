using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Utils;
using Xunit;

namespace WebApplication1.Tests.UserControllerTests
{
    public class DeleteUserTests
    {

        //Criação de teste para validar o Delete User
        [Theory(DisplayName = "Deleted User Should Retorn Not Found")]
        [InlineData("teste@gmail.com", "12345")]
        [InlineData("naoexiste@gmail.com", "54321")]
        public async Task DeleteUser_ShouldReturn_Ok_WhenDeleted(string email, string senha)
        {
            //arrange

            var context = TestsFunc.CriarDbNaMemoria();
            await TestsFunc.AddUserToContext(context, "teste@gmail.com", "12345");

            var controller = new UserController(context);

            var user = await controller.GetUserByEmail(email);

            //Act
            if (user is OkObjectResult okResult)
            {
                var userId = okResult.Value.GetType().GetProperty("Id")?.GetValue(okResult.Value, null);
                Assert.NotNull(userId);

                var result = await controller.DeleteUserById((int)userId);

                //Assert
                Assert.IsType<OkObjectResult>(result);

            }
            else
            {
                var result = await controller.DeleteUserById(-1);
                Assert.IsType<NotFoundObjectResult>(result);
            }

        }


    }
}
