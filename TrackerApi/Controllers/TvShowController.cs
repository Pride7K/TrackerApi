using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/tvshows")]
    public class TvShowController : ControllerBase
    {
        private readonly ITvShowService _service;
        public TvShowController(ITvShowService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get All the Tv Shows
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET  /skip/0/take/25
        ///     {
        ///       
        ///       
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpGet("skip/{skip:int}/take/{take:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Get Tv Shows Recomendations
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET  /recomendations/skip/0/take/25
        ///     {
        ///       
        ///       
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpGet("recomendations/skip/{skip:int}/take/{take:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        /// <summary>
        /// Get Specific Tv Show By Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET  /id
        ///     {
        ///       
        ///       
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(CancellationToken token,[FromRoute]int id)
        {
            var tvshow = await _service.GetById(id,token);

            return tvshow == null ? NotFound() : Ok(tvshow);
        }

        /// <summary>
        /// Create a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST  
        ///     {
        ///       "Title":string,
        ///       "Description":string
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Create Tv Shows from a endpoint
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /load 
        ///     {
        ///       
        ///       
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("load")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostLoadAsync(CancellationToken token)
        {
            await _service.Load(token);

            return Ok();
        }



        /// <summary>
        /// Update a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT 
        ///     {
        ///       "Description":"string,
        ///       "Available":bool
        ///         
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Get All the episodes from a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /id/episodes 
        ///     {
        ///      
        ///       
        ///       
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        [Route("{id:int}/episodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(CancellationToken token,[FromRoute] int id)
        {
            var tvshow = await _service.GetTvShowWithEpisode(id,token);

            return tvshow == null ? NotFound() : Ok(tvshow.Episodes);
        }

        /// <summary>
        /// Get All the actors from a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /id/actors 
        ///     {
        ///       
        ///       
        ///       
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        [Route("{id:int}/actors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        
        /// <summary>
        /// Delete a Tv Show
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /id
        ///     {
        ///       
        ///       
        ///       
        ///     }
        ///
        /// </remarks>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
