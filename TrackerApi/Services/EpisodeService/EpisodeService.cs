using Microsoft.EntityFrameworkCore;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.EpisodeService.ViewModel;
using TrackerApi.Services.Erros;

namespace TrackerApi.Services.EpisodeService
{
    public class EpisodeService : IEpisodeService
    {
        private readonly AppDbContext _context;
        public EpisodeService(AppDbContext context)
        {
            _context = context;
        }
        public void Create(CreateEpisodeViewModel model)
        {

            var tvshow  = _context.TvShows.FirstOrDefaultAsync(x => x.Id == model.TvShowId);

            if (tvshow == null)
                throw new NotFoundException("Tv Show Not Found");

            var episode = new Episode()
            {
                Description = model.Description,
                ReleaseDate = model.ReleaseDate,
                TvShowId = tvshow.Id
            };

             _context.Episodes.AddAsync(episode);
             _context.SaveChangesAsync();

            return tvshow;


        }
    }
}
