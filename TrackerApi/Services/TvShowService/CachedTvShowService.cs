using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Services.TvShowService
{
    public class CachedTvShowService : ITvShowService
    {
        private const string TvShowListKey = "TvShowList";
        private readonly IMemoryCache _memoryCache;
        private readonly ITvShowService _tvshowService;
        public CachedTvShowService(ITvShowService tvshowService, IMemoryCache  memoryCache)
        {
            _tvshowService = tvshowService;
            _memoryCache = memoryCache;
        }
        public Task<TvShow> Create(CreateTvShowViewModel model)
        {
            return _tvshowService.Create(model);
        }

        public Task Delete(int id)
        {
            return _tvshowService.Delete(id);
        }

        public async Task<GetTvShowViewModel> GetAll(int skip, int take, GetTvShowFiltersViewModel filter)
        {
            var options = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromSeconds(10))
           .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (_memoryCache.TryGetValue(TvShowListKey, out Task<GetTvShowViewModel> result))
                return await result;

            result =  _tvshowService.GetAll(skip, take, filter);

            await _memoryCache.Set(TvShowListKey, result, options);

            return await result;
        }

        public Task<GetActorViewModel> GetAllActors(int tvShowId)
        {
            return _tvshowService.GetAllActors(tvShowId);
        }

        public Task<TvShow> GetById(int id)
        {
            return _tvshowService.GetById(id);
        }

        public Task<TvShow> GetByTitle(string title)
        {
            return _tvshowService.GetByTitle(title);
        }

        public Task<GetTvShowViewModel> GetRecomendationsAll(int skip, int take, GetTvShowFiltersViewModel filter)
        {
            return _tvshowService.GetRecomendationsAll(skip, take, filter);
        }

        public Task<TvShow> GetTvShowWithEpisode(int tvShowId)
        {
            return _tvshowService.GetTvShowWithEpisode(tvShowId);
        }

        public Task Load(int page = 1)
        {
            return _tvshowService.Load(page);
        }

        public Task<TvShow> Update(int tvShowId, PutTvShowViewModel model)
        {
            return _tvshowService.Update(tvShowId,model);
        }
    }
}
