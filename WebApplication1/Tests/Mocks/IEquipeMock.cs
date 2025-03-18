using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos.Equipe;
using WebApplication1.Models;

namespace WebApplication1.Tests.Mocks
{
    public interface IEquipeService
    {
        Task<IActionResult> CadastrarEquipe(CadastrarEquipeDto cadastrarDto);
        Task<IActionResult> DeletarEquipe(DeletarEquipeDto deletarDto);
    }
}
