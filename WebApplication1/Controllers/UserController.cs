using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Users user)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest(new {mensagem = "Usuario não identificado"});
            }
            //gera um RA uníco para cada pessoa
            user.RA = GerarRA();

            //criptograva a senha
            user.SenhaHash = BCrypt.Net.BCrypt.HashPassword(user.SenhaHash);


                _context.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new {id = user.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest(new {mensagem = "Usuario não encontrado"});
            }

            return Ok(new { mensagem = "Usuario encotrado"});
        }
        [HttpDelete]
        public IActionResult DeleteUserById(int id)
        {
            var user = _context.Users.Find(id);
        
            if (user == null)
            {
                return NotFound("Usuario não encontrado");
            }

            _context.Remove(user);
            _context.SaveChanges();

            return Ok("Usuario removido com sucesso");
  
        }

        public string GerarRA()
        {
            string novoRA;
            do
            {
                var anoAtual = DateTime.Now.Year.ToString();
                var numeroAleatorio = Random.Shared.Next(1000, 9999);
                novoRA = $"{anoAtual}{numeroAleatorio}";
            } while (_context.Users.Any(u => u.RA == novoRA));

            return novoRA;
        }



    }
}
