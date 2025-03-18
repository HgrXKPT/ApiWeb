using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Dtos.Equipe;
using WebApplication1.Tests.Mocks;
using Xunit;

namespace WebApplication1.Tests.EquipeControllerTests
{
    public class DeletarEquipeTest
    {

        [Theory(DisplayName ="Delete user Should return OK/NotFound")]
        [InlineData("NotFound")]
        [InlineData("EquipeReal")]
        [InlineData("")]

        public async Task DeleteEquipe_ShouldReturnOkOrNotFound_BasedOnExistence(string nomeEquipe)
        {
            var mockService = new Mock<IEquipeService>();
            MockConfigure(mockService);

            //Arrange
            var dto = new DeletarEquipeDto { NomeEquipe = nomeEquipe};

            //act

            var result = await mockService.Object.DeletarEquipe(dto);

            switch (result)
            {
                case BadRequestObjectResult badRequest:
                    Assert.Equal("Favor inserir valor válido", badRequest.Value);
                    break;
                case NotFoundObjectResult notFound:
                    Assert.Equal("Equipe não encontrada", notFound.Value);
                    break;
                case OkObjectResult okResult:
                    Assert.Equal("Usuario Deletado", okResult.Value);
                    break;
                default:
                    Assert.Fail("Tipo de resultado inesperado.");
                    break;
            }
            

        }





        private void MockConfigure(Mock<IEquipeService> mockService)
        {
            mockService.Setup(s => s.DeletarEquipe(It.IsAny<DeletarEquipeDto>()))
                .ReturnsAsync((DeletarEquipeDto dto) =>
                {
                    if (string.IsNullOrEmpty(dto.NomeEquipe))
                        return new BadRequestObjectResult("Favor inserir valor válido");
                    if (dto.NomeEquipe == "NotFound")
                        return new NotFoundObjectResult("Equipe não encontrada");
                    

                    return new OkObjectResult("Usuario Deletado");
                }
                    
                    

                );
        }
    }
}
