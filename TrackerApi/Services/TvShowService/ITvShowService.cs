using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.TvShowService
{
    public interface ITvShowService
    {
        Task<GetTvShowViewModel> GetAll(int skip, int take);
        Task<TvShow> GetById(int id);
        Task<TvShow> GetByTitle(string title);
        Task<TvShow> GetTvShowWithEpisode(int tvShowId);
        Task<TvShow> Update(int tvShowId,PutTvShowViewModel model);

        Task<TvShow> Create(CreateTvShowViewModel model);
        Task Delete(int id);
        Task Load();


    }
}
