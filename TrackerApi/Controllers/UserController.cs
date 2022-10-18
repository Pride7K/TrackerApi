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
using System.Threading;
using Microsoft.AspNetCore.Http;


namespace TrackerApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get All the Users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /skip/0/take/25
        ///     {
        ///       
        ///     }
        ///
        /// </remarks>
        [HttpGet("skip/{skip:int}/take/{take:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(CancellationToken token,[FromRoute] int skip = 0
            , [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take,token);

            return Ok(data);
        }


        /// <summary>
        /// Get a specific User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /1
        ///     {
        ///        
        ///     }
        ///
        /// </remarks>
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(CancellationToken token,[FromRoute] int id)
        {
            var user = await _service.GetById(id,token);

            if (user == null)
                return NotFound(new { ErrorMessage = "Not found", model = user });


            return Ok(user);
        }

        /// <summary>
        /// Favorite a Tv Show that a specific logged user liked it
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST tvshow/favorite
        ///     {
        ///        "favorite":bool,
        ///        "tvshowId":int
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("tvshow/favorite")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Create a User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///       "Name":string,
        ///       "Email":string,
        ///       "Password":string
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem($"An error occured: {e.Message}");
            }

        }
    }
}
