using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.Erros;
using TrackerApi.Services.Login.ViewModel;
using TrackerApi.Services.LoginService;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] AppDbContext context, [FromBody] AuthenticateUserViewModel model)
        {
            try
            {
                var data = await _service.Authenticate(model);

                return Ok(data);
            }
            catch (UnauthorizedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem($"An error occured: {e.Message}");
            }

        }
    }
}
