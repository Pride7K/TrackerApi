using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.TvShowService
{
    public interface ITvShowService
    {
        ValueTask<GetTvShowViewModel> GetAll(int skip, int take, GetTvShowFiltersViewModel filter,
            CancellationToken token);
        Task<GetTvShowViewModel> GetRecomendationsAll(int skip, int take, GetTvShowFiltersViewModel filter,CancellationToken token);
        Task<GetActorViewModel> GetAllActors(int tvShowId,CancellationToken token);
        Task<TvShow> GetById(int id,CancellationToken token);
        Task<TvShow> GetById(int id);
        Task<TvShow> GetByTitle(string title,CancellationToken token);
        Task<TvShow> GetByTitle(string title);
        Task<TvShow> GetTvShowWithEpisode(int tvShowId,CancellationToken token);
        Task<TvShow> Update(int tvShowId,PutTvShowViewModel model);

        Task<TvShow> Create(CreateTvShowViewModel model);
        Task Delete(int id);
        Task Load(CancellationToken cancellationToken, int page = 1);


    }
}
