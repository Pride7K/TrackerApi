using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services;
using TrackerApi.ViewModel.Login;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] AppDbContext context, [FromBody] AuthenticateUserViewModel model)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == x.Password);

            if (user == null)
                return NotFound();

            if (!user.Active)
                return Unauthorized();

            var token = TokenService.GenerateToken(user);
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };

        }
    }
}
