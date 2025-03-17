using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
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
        public IActionResult CadastrarEquipe([FromBody] Equipes equipes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(equipes);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CadastrarEquipe), new { id = equipes.Id }, equipes);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarEquipe(string nomeEquipe)
        {
            //Pegando o nome da equipe de forma assíncrona para realizar o delete
            var equipe = await _context.Equipes.FirstOrDefaultAsync(e => e.NomeEquipe == nomeEquipe);

            //Verificação para validar se existe um usuario
            if (equipe == null)
            {
                return NotFound(new {mensagem = "Equipe não encontrada"});
            }

            //removendo e dando update no Database
            _context.Remove(equipe);
            _context.SaveChangesAsync();

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
