using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrackerApi.Services.ActorService;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/actors")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _service;
        public ActorController(IActorService service)
        {
            _service = service;
        }

        
        /// <summary>
        /// Get All actors
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET  /skip/0/take/25
        ///     {
        ///       
        ///       
        ///       
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet("skip/{skip:int}/take/{take:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(CancellationToken token,
        [FromQuery] GetActorFiltersViewModel filter,
        [FromRoute] int skip = 0,
        [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take, filter,token);

            return Ok(data);
        }

        /// <summary>
        /// Add a Actor to a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT  
        ///     {
        ///       
        ///       
        ///       
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpPut("{actorId:int}/AddToTvShow/{tvShowId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> PutAsync([FromRoute] int actorId, [FromRoute] int tvShowId)
        {
            try
            {
                await _service.AddToTvShow(actorId, tvShowId);

                return Ok();
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e.Message);
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
        /// Create a Actor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  
        ///     {
        ///       "Name":string,
        ///       "Description":string,
        ///       
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] CreateActorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { ErrorMessage = "Must provide a valid object", model = model });

            try
            {
                var tvshow = await _service.Create(model);

                return Created($"v1/tvshows/{tvshow.Id}", tvshow);
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
