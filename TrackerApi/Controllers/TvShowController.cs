using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
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

        public async Task<IActionResult> GetAsync(
            [FromRoute] int skip = 0
            ,[FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take);

            return Ok(data);
        }


        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
        {
            var tvshow = await _service.GetById(id);

            return tvshow == null ? NotFound() : Ok(tvshow);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> PostAsync( [FromBody] CreateTvShowViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tvshowDb = await _service.GetByTitle(model.Title);

            if (tvshowDb != null)
                return BadRequest(tvshowDb);
        
            try
            {
                var tvshow = await _service.Create(model);

                return Created($"v1/tvshows/{tvshow.Id}",tvshow);
            }
            catch
            {
                return BadRequest();
            }

            
        }



        [HttpPut("{id:int}")]
        [Authorize]

        public async Task<IActionResult> PutAsync( [FromRoute] int id, [FromBody] PutTvShowViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tvshow = await _service.GetById(id );

            if (tvshow == null)
                return NotFound();

            try
            {
                await _service.Update(tvshow,model);

                return Ok(tvshow);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id:int}/episodes")]

        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var tvshow = await _service.GetTvShowWithEpisode(id);

            return tvshow == null ? NotFound() : Ok(tvshow.Episodes);
        }

        [HttpDelete("{id:int}")]
        [Authorize]

        public async Task<IActionResult> DeleteAsync( [FromRoute] int id)
        {

            var tvshow = await _service.GetById(id);

            if (tvshow == null)
                return NotFound();

            try
            {
                _service.Delete(tvshow);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
