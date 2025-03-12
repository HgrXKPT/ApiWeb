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
        [Fact(DisplayName = "Deleted User Should Return Ok")]
        public async Task DeleteUser_ShouldReturn_Ok_WhenDeleted()
        {
            //arrange
            var dto = TestsFunc.CriarDto("teste@gmail.com", "12345");
            var context = TestsFunc.CriarDbNaMemoria();
            await TestsFunc.AddUserToContext(context, "teste@gmail.com", "12345");

            var controller = new UserController(context);

            var user = await controller.GetUserByEmail(dto.Email);

            //act
            if (user is OkObjectResult okResult)
            {
                var userId = okResult.Value.GetType().GetProperty("Id")?.GetValue(okResult.Value, null);
                Assert.NotNull(userId);

                var result = await controller.DeleteUserById((int)userId);

                //Assert
                Assert.IsType<OkObjectResult>(result);

            }

        }

        //Criação de teste para validar o notfound do Delete User

        [Fact(DisplayName = "Deleted User Should Retorn Not Found")]
        public async Task DeleteUser_ShouldReturn_NotFound_WhenTryingDeleteNonExistingUser()
        {
            //arrange
            var dto = TestsFunc.CriarDto("higor@gmail.com", "123");
            var context = TestsFunc.CriarDbNaMemoria();
            await TestsFunc.AddUserToContext(context, "teste@gmail.com", "12345");

            var controller = new UserController(context);
            //act
            var result = await controller.DeleteUserById(-1);
            //assert
            Assert.IsType<NotFoundObjectResult>(result);

           
        }
    }
}
