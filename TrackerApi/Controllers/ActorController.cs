using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TrackerApi.Services.ActorService;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/actors")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _service;
        public ActorController(IActorService service)
        {
            _service = service;
        }

        [HttpGet("skip/{skip:int}/take/{take:int}")]

        public async Task<IActionResult> GetAsync(
        [FromQuery] GetActorFiltersViewModel filter,
        [FromRoute] int skip = 0,
        [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take, filter);

            return Ok(data);
        }


        [HttpPut("{actorId:int}/AddToTvShow/{tvShowId:int}")]
        [Authorize]

        public async Task<IActionResult> PutAsync([FromRoute] int actorId, [FromRoute] int tvShowId)
        {
            try
            {
                await _service.AddToTvShow(actorId, tvShowId);

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
        [Authorize]

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
