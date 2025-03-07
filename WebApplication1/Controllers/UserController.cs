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
                return BadRequest(ModelState);
            }

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
                return BadRequest(ModelState);
            }

            return Ok(user);
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

        
    }
}
