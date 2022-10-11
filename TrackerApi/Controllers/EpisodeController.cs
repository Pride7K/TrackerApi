using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.ViewModel.Episode;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/episodes")]
    public class EpisodeController : ControllerBase
    {

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateEpisodeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tvshow = await context.TvShows.FirstOrDefaultAsync(x => x.Id == model.TvShowId);

            if (tvshow == null)
                return NotFound();

            var episode = new Episode()
            {
                Description = model.Description,
                ReleaseDate = model.ReleaseDate,
                TvShowId = tvshow.Id
            };

            try
            {
                await context.Episodes.AddAsync(episode);

                await context.SaveChangesAsync();

                return Created($"v1/episodes/{tvshow.Id}", tvshow);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
