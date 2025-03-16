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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebApplication1.Tests.Controllers
{




    public class CreateUserTests
    {








        [Theory(DisplayName = "Create User Should Retorn Bad Request/ Created at Action")]
        [InlineData("higor", "TesteConflito@gmail.com", "12345")]
        [InlineData("", "Teste@gmail.com", "12345")]
        [InlineData("Higor", "Teste@gmail.com", "12345")]
        public async Task CreateUser_ShouldReturn_CreatedAtAction_WhenCreated(string nome, string email, string senha)
        {

            var mockService = new Mock<IUserService>();
            //Mock
            mockService.Setup(s => s.CreateUser(It.IsAny<CreateUserDto>()))
                .ReturnsAsync((CreateUserDto dto) =>
                {
                    if (string.IsNullOrEmpty(dto.Nome) || string.IsNullOrEmpty(dto.Email))

                        return new BadRequestObjectResult("Campo nome ou email vazio");

                    if (dto.Email == "TesteConflito@gmail.com")

                        return new ConflictObjectResult("Usuario já cadastrado");

                    return !string.IsNullOrEmpty(dto.Nome) || !string.IsNullOrEmpty(dto.Email) || !string.IsNullOrEmpty(dto.Senha)

                        ? new CreatedAtActionResult("GetUser", "Users", new { id = 1 }, new Users { Nome = dto.Nome, Email = dto.Email, SenhaHash = dto.Senha })
                        : throw new InvalidOperationException("Erro inesperado: os dados fornecidos não correspondem a nenhum cenário definido.");
                }

            );

            // Arrange
            var dto = CriarDtoUser(nome, email, senha);

            // Act

            var result = await mockService.Object.CreateUser(dto);

            switch (result)
            {
                case CreatedAtActionResult createdAtActionResult:
                    Assert.IsType<Users>(createdAtActionResult.Value);
                    Assert.NotNull(createdAtActionResult.Value);
                    var user = Assert.IsType<Users>(createdAtActionResult.Value);
                    Assert.Equal("Higor", user.Nome);
                    Assert.Equal("Teste@gmail.com", user.Email);
                    break;

                case BadRequestObjectResult badRequestObjectResult:
                    Assert.NotNull(badRequestObjectResult.Value);
                    Assert.Equal("Campo nome ou email vazio", badRequestObjectResult.Value);
                    break;

                case ConflictObjectResult conflictObjectResult:
                    Assert.NotNull(conflictObjectResult.Value);
                    Assert.Equal("Usuario já cadastrado", conflictObjectResult.Value);
                    break;

                default:
                    Assert.True(false, "Resultado inesperado!");
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
