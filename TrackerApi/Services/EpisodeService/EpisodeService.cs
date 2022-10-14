using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.EpisodeService.ViewModel;
using TrackerApi.Services.Erros;
using TrackerApi.Services.SharedServices;
using TrackerApi.Transaction;

namespace TrackerApi.Services.EpisodeService
{
    public class EpisodeService : DbContextService,IEpisodeService
    {
        public EpisodeService(AppDbContext context, IUnitOfWork uow) : base(context, uow)
        {
        }

        public async Task<TvShow> Create(CreateEpisodeViewModel model)
        {

            var tvshow  = await _context.TvShows.FirstOrDefaultAsync(x => x.Id == model.TvShowId);

            if (tvshow == null)
                throw new NotFoundException("Tv Show Not Found");

            var episode = new Episode()
            {
                Description = model.Description,
                ReleaseDate = model.ReleaseDate,
                TvShowId = tvshow.Id
            };

             await _context.Episodes.AddAsync(episode);
            await _uow.Commit();

            return tvshow;


        }
    }
}
