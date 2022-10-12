using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.UserService;
using TrackerApi.Services.UserService.ViewModel;
using System.Security.Claims;


namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("skip/{skip:int}/take/{take:int}")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromRoute] int skip = 0
            , [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take);

            return Ok(data);
        }


        [HttpGet("{id:int}")]
        [Authorize]

        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var user = await _service.GetById(id);

            if (user == null)
                return NotFound(new { ErrorMessage = "Not found", model = user });


            return Ok(user);
        }

        [HttpPost]
        [Route("tvshow/favorite")]
        [Authorize]

        public async Task<IActionResult> PostFavoriteAsync([FromBody] FavoriteTvShowViewModel model)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                await _service.Favorite(userEmail, model);

                return Ok();
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

        [HttpPost]
        public async Task<IActionResult> PostAsync( [FromBody] CreateUserViewModel model)
        {


            if (!ModelState.IsValid)
                return BadRequest(new { ErrorMessage = "Must provide a valid object", model = model });

            try
            {
                var user =  await _service.Create(model);

                return Created($"v1/users/{user.Id}", user);
            }
            catch (AlreadyExistsException e)
            {
                return Problem(e.Message);
            }
            catch (Exception e)
            {
                return Problem($"An error occured: {e.Message}");
            }

        }
    }
}
