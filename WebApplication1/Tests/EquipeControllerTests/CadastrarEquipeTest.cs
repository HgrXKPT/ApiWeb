using Moq;
using WebApplication1.Dtos.Equipe;
using WebApplication1.Tests.Mocks;
using Xunit;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;
using BCrypt;
using WebApplication1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net;

namespace WebApplication1.Tests.EquipeControllerTests
{
    public class CadastrarEquipeTest
    {

        [Theory]
        [InlineData("EquipeTeste",1,0)]
        [InlineData("TesteConflito", 1, 1)]
        [InlineData("", 1, null)]

        public async Task Create_Equipe_ShouldReturn_CreatedAtAction_WhenCreate(string nomeEquipe, int QuantidadeMembro, int QuantidadeProjeto)
        {
            var mockService = new Mock<IEquipeService>();
            ConfigureMock(mockService);


            //Arrange
            var dto = CadastrarDtoEquipe(nomeEquipe, QuantidadeProjeto, QuantidadeMembro);

            //act

            var result = await mockService.Object.CadastrarEquipe(dto);

            switch (result)
            {
                case BadRequestObjectResult badRequest:
                    Assert.Equal("Algum dos valores é nulo", badRequest.Value);
                    break;

                case ConflictObjectResult conflictResult:
                    Assert.Equal("Já existe uma equipe com esse nome", conflictResult.Value);
                    break;
                case CreatedAtActionResult createdResult:
                    var equipe = Assert.IsType<Equipes>(createdResult.Value);
                    Assert.NotNull(createdResult.Value);

                    Assert.Equal("EquipeTeste", equipe.NomeEquipe);
                    Assert.Equal(1, equipe.QuantidadeMembros);
                    Assert.Equal(0, equipe.QuantidadeProjetos);


                    break;

            }





        }

       private static CadastrarEquipeDto CadastrarDtoEquipe(string nomeEquipe, int quantidadeProjeto, int quantidadeMembros)
        {
            var dto = new CadastrarEquipeDto()
            {
                NomeEquipe = nomeEquipe,
                QuantidadeMembros = quantidadeMembros,
                QuantidadeProjetos = quantidadeProjeto
            };
            return dto;

        }

        private void ConfigureMock(Mock<IEquipeService> mockService)
        {
            mockService.Setup(s => s.CadastrarEquipe(It.IsAny<CadastrarEquipeDto>()))
                .ReturnsAsync((CadastrarEquipeDto dto) =>
                {
                    if (string.IsNullOrEmpty(dto.NomeEquipe) || dto.QuantidadeMembros == null || dto.QuantidadeProjetos == null)
                        return new BadRequestObjectResult("Algum dos valores é nulo");
                    if (dto.NomeEquipe == "TesteConflito")
                        return new ConflictObjectResult("Já existe uma equipe com esse nome");
                    return !(string.IsNullOrEmpty(dto.NomeEquipe) && dto.QuantidadeMembros != null && dto.QuantidadeProjetos != null)

                       ? new CreatedAtActionResult
                       ("GetEquipe",
                       "Equipe",
                       new { id = 1 },
                       new Equipes
                       {
                           NomeEquipe = dto.NomeEquipe,
                           QuantidadeMembros = dto.QuantidadeMembros,
                           QuantidadeProjetos = dto.QuantidadeProjetos
                       })
                       : throw new InvalidOperationException("Erro inesperado: os dados fornecidos não correspondem a nenhum cenário definido.");
                });
        }

    }
}
