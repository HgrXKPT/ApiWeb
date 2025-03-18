using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos.Equipe;
using WebApplication1.Dtos.Users;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : Controller
    {
        private readonly AppDbContext _context;


        public EquipeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Registrar Equipe")]
        public async Task<IActionResult> CadastrarEquipe([FromBody] CadastrarEquipeDto cadastrarEquipeDto)
        {
            var nomeEquipeExistente = await _context.Equipes.AnyAsync(e => e.NomeEquipe == cadastrarEquipeDto.NomeEquipe);

            if (nomeEquipeExistente)
                return Conflict("Nome de Equipe Já existe");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);


            var equipes = new Equipes
            {
                NomeEquipe = cadastrarEquipeDto.NomeEquipe,
                QuantidadeMembros = cadastrarEquipeDto.QuantidadeMembros,
                QuantidadeProjetos = cadastrarEquipeDto.QuantidadeProjetos

            };

            _context.Add(equipes);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CadastrarEquipe), new { nome = cadastrarEquipeDto.NomeEquipe }, cadastrarEquipeDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarEquipe([FromBody]DeletarEquipeDto dto)
        {

            if(string.IsNullOrEmpty(dto.NomeEquipe))
                return BadRequest(new { mensagem = "Favor não deixar campos em branco" });

            //Pegando o nome da equipe de forma assíncrona para realizar o delete
            var equipe = await _context.Equipes.FirstOrDefaultAsync(e => e.NomeEquipe == dto.NomeEquipe);

            //Verificação para validar se existe um usuario
            if (equipe == null)
                return NotFound(new { mensagem = "Equipe não encontrada" });
            

            //removendo e dando update no Database
            _context.Remove(equipe);
            await _context.SaveChangesAsync();

            //Confirmação que houve sucesso
            return Ok(new { mensagem = "Equipe removida com sucesso" });
        }

        [HttpPatch]
        public IActionResult AdicionarMembroNaEquipe(int id, int equipeid)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                user.EquipeId = equipeid;
                _context.SaveChanges();
                return Ok("Usuario adicionado na equipe");
            }
            else
            {
                return NotFound("Usuario não encontrado");
            }

        }

        


    }
}
