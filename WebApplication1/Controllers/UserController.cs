using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos.Users;
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

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {

            if (!ModelState.IsValid)
            {

            }
            try
            {
                var user = new Users
                {
                    Nome = createUserDto.Nome,
                    Email = createUserDto.Email,
                    //criptografia da senha
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Senha)
                };

                //gera um RA uníco para cada pessoa
                user.RA = GerarRA();
                _context.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateUser), new
                {
                    id = user.Id
                }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensagem = "Usuario não identificado"});
            }


        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest(new
                {
                    mensagem = "Usuario não encontrado"
                });
            }

            return Ok(new
            {
                Id = user.Id,
                mensagem = $"Nome:{user.Nome} Id: {user.Id}"
            });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Usuario não encontrado");
            }

             _context.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario removido com sucesso");

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto logindto)
        {
            if (string.IsNullOrEmpty(logindto.Email) || string.IsNullOrEmpty(logindto.Senha))
            {
                return BadRequest(new
                {
                    mensagem = "O campo de email e senha são obrigatorios"
                });
            }

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == logindto.Email);

            if (usuario == null)
            {
                return NotFound(new
                {
                    mensagem = "Usuário não encontrado"
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(logindto.Senha, usuario.SenhaHash))
            {
                return Unauthorized(new
                {
                    mensagem = "Senha inválida"
                });
            }


            return Ok(new
            {
                mensagem = "Login efetuado com sucesso"
            });

        }

        private string GerarRA()
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
