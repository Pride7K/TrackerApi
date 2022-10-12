using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.EpisodeService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.EpisodeService
{
    public interface IEpisodeService
    {
        Task<TvShow> Create(CreateEpisodeViewModel model);

    }
}
