using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Services.ActorService
{
    public interface IActorService
    {
        Task<GetActorViewModel> GetAll(int skip, int take, GetActorFiltersViewModel filter,CancellationToken token);

        Task<Actor> Create(CreateActorViewModel model);
        Task AddToTvShow(int actorId,int tvShowId);

    }
}
