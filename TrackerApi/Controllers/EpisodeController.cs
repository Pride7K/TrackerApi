using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.EpisodeService.ViewModel;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService;

namespace TrackerApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/episodes")]
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeService _service;
        public EpisodeController(IEpisodeService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Create a Episode from a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  
        ///     {
        ///       "Description":string,
        ///       "ReleaseDate":datetimeString yyyy-mm-dd,
        ///       "TvShowId":int
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateEpisodeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var tvshow = await _service.Create(model);

                return Created($"v1/episodes/{tvshow.Id}", tvshow);
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
