using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.Erros;
using TrackerApi.Services.Login.ViewModel;
using TrackerApi.Services.LoginService;

namespace TrackerApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        public LoginController(ILoginService service)
        {
            _service = service;
        }
        /// <summary>
        /// Authenticate a User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  /user
        ///     {
        ///       "Name":"test",
        ///       "Email":"test@gmail.com",
        ///       "Password":"test"
        ///     }
        ///
        /// </remarks>
        [HttpPost("user")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
