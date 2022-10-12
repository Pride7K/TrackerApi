using TrackerApi.Models;
using TrackerApi.Services.EpisodeService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.EpisodeService
{
    public interface IEpisodeService
    {
        TvShow Create(CreateEpisodeViewModel model);

    }
}
