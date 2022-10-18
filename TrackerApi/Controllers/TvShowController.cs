using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/tvshows")]
    public class TvShowController : ControllerBase
    {
        private readonly ITvShowService _service;
        public TvShowController(ITvShowService service)
        {
            _service = service;
        }

        [HttpGet("skip/{skip:int}/take/{take:int}")]

        public async ValueTask<IActionResult> GetAsync(
            CancellationToken token,
            [FromQuery] GetTvShowFiltersViewModel filter,
            [FromRoute] int skip = 0,
            [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take, filter,token);

            return Ok(data);
        }

        [HttpGet("recomendations/skip/{skip:int}/take/{take:int}")]

        public async Task<IActionResult> GetRecomendationsAsync(
            CancellationToken token,
    [FromQuery] GetTvShowFiltersViewModel filter,
    [FromRoute] int skip = 0,
    [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetRecomendationsAll(skip, take, filter,token);

            return Ok(data);
        }



        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetByIdAsync(CancellationToken token,[FromRoute]int id)
        {
            var tvshow = await _service.GetById(id,token);

            return tvshow == null ? NotFound() : Ok(tvshow);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> PostAsync( [FromBody] CreateTvShowViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { ErrorMessage = "Must provide a valid object", model = model });

            try
            {
                var tvshow = await _service.Create(model);

                return Created($"v1/tvshows/{tvshow.Id}",tvshow);
            }
            catch(AlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return Problem( $"An error occured: {e.Message}");
            }

            
        }

        [HttpPost]
        [Route("load")]

        public async Task<IActionResult> PostLoadAsync(CancellationToken token)
        {
            await _service.Load(token);

            return Ok();
        }



        [HttpPut("{id:int}")]
        [Authorize]

        public async Task<IActionResult> PutAsync( [FromRoute] int id, [FromBody] PutTvShowViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { ErrorMessage = "Must provide a valid object", model = model });

            try
            {
                var tvshow = await _service.Update(id,model);

                return Ok(tvshow);
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

        [HttpGet]
        [Route("{id:int}/episodes")]

        public async Task<IActionResult> GetAsync(CancellationToken token,[FromRoute] int id)
        {
            var tvshow = await _service.GetTvShowWithEpisode(id,token);

            return tvshow == null ? NotFound() : Ok(tvshow.Episodes);
        }


        [HttpGet]
        [Route("{id:int}/actors")]

        public async Task<IActionResult> GetActorsAsync(CancellationToken token,[FromRoute] int id)
        {

            try
            {
                var actors = await _service.GetAllActors(id,token);

                return Ok(actors);
            }
            catch(NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem($"An error occured: {e.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]

        public async Task<IActionResult> DeleteAsync( [FromRoute] int id)
        {

            try
            {
                await _service.Delete(id);

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

    }
}
