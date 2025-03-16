using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApplication1.Dtos.Users;

namespace WebApplication1.Models
{



    public interface IUserService
    {
        Task<IActionResult> CreateUser(CreateUserDto dto);
        Task<IActionResult> DeleteUserById(int id);
        Task<Users?> GetUserByEmail(string email);
        Task<IActionResult> LoginUser(LoginDto dto);
    }


}
