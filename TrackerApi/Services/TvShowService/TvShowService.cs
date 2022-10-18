using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.Erros;
using TrackerApi.Services.SharedServices;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService;
using TrackerApi.Transaction;

namespace TrackerApi.Services.TvShowService
{
    public class TvShowService : DbContextService, ITvShowService
    {
        const string GENRE_KEY = "genre";
        const string AVAILABLE_KEY = "available";
        const string STILL_GOING_KEY = "still_going";

        public IHttpClientFactory _clientFactory { get; set; }

        public TvShowService(AppDbContext context, IUnitOfWork uow, IHttpClientFactory clientFactory) : base(context, uow)
        {
            _clientFactory = clientFactory;
        }

        public async Task<TvShow> Create(CreateTvShowViewModel model)
        {
            var tvshowDb = await this.GetByTitle(model.Title);

            if (tvshowDb != null)
                throw new AlreadyExistsException("The Tv Show already exists!");

            var tvshow = new TvShow()
            {
                Title = model.Title,
                Description = model.Description,
                Available = true,
                StillGoing = model.StillGoing.Value
            };


            await _context.TvShows.AddAsync(tvshow);

            await _uow.Commit();

            return tvshow;
        }

        public async Task Delete(int id)
        {
            var tvshow = await this.GetById(id);

            if (tvshow == null)
                throw new NotFoundException("Not found");

            _context.TvShows.Remove(tvshow);

            await _uow.Commit();
        }

        public IEnumerable<TvShow> SeparateSort<T>(IQueryable<TvShow> tvShows, Func<TvShow, T> keySelector, GetTvShowFiltersViewModel filter)
        {
            return filter.Sort == "ASC"
                ? tvShows.OrderBy(keySelector)
                : tvShows.OrderByDescending(keySelector);
        }

        public async Task<GetTvShowViewModel> GetRecomendationsAll(int skip, int take, GetTvShowFiltersViewModel filter,CancellationToken token)
        {
            var data = await GetAll(skip, take, filter,token);

            data.TvShows = GetHalfOfTheList(data.TvShows.OrderBy(x => Guid.NewGuid()).ToList());

            return data;
        }

        public List<T> GetHalfOfTheList<T>(List<T> list)
        {
            return list.Select((x, i) => new { x, i }).Where(t => t.i % 2 == 0).Select(t => t.x).ToList();
        }

        public async ValueTask<GetTvShowViewModel> GetAll(int skip, int take, GetTvShowFiltersViewModel filter,
            CancellationToken token)
        {
            var totalTvShows = await _context.TvShows.CountAsync(cancellationToken:token);


            var tvshows = _context.TvShows
                .Include(x => x.Episodes)
                .AsNoTracking()
                .Skip(skip)
                .Take(take).AsQueryable();

            if (filter != null)
            {
                if (filter.Available.HasValue)
                {
                    tvshows = tvshows.Where(x => x.Available == filter.Available.Value);
                }
                if (filter.StillGoing.HasValue)
                {
                    tvshows = tvshows.Where(x => x.StillGoing == filter.StillGoing.Value);
                }
                if (filter.Genre.HasValue)
                {
                    tvshows = tvshows.Where(x => (int)x.Genre == filter.Genre);
                }

                if (!string.IsNullOrEmpty(filter.Sort))
                {
                    var optionChoosedToSort = filter.TypesSorting.FirstOrDefault(x => x.ToLower() == filter.SortingBy?.ToLower());

                    tvshows = optionChoosedToSort switch
                    {
                        GENRE_KEY => SeparateSort(tvshows, x => x.Genre, filter).AsQueryable(),
                        AVAILABLE_KEY => SeparateSort(tvshows, x => x.Available, filter).AsQueryable(),
                        STILL_GOING_KEY => SeparateSort(tvshows, x => x.StillGoing, filter).AsQueryable(),
                        _ => null
                    };

                }
            }

            return new GetTvShowViewModel()
            {
                TotalTvShows = totalTvShows,
                TvShows = await tvshows.ToListAsync(cancellationToken:token)
            };
        }

        public Task<TvShow> GetById(int id,CancellationToken token)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Id == id,cancellationToken:token);
        }
        
        public Task<TvShow> GetById(int id)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<TvShow> GetByTitle(string title,CancellationToken token)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Title == title,cancellationToken:token);
        }
        
        public Task<TvShow> GetByTitle(string title)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Title == title);
        }

        public Task<TvShow> GetTvShowWithEpisode(int tvShowId,CancellationToken token)
        {
            return _context.TvShows.Include(x => x.Episodes).FirstOrDefaultAsync(x => x.Id == tvShowId,cancellationToken:token);
        }

        public async Task Load(CancellationToken token,int page = 1)
        {
            HttpClient client = _clientFactory.CreateClient("Episodate");

            var result = await client.GetAsync($"most-popular?page={page}",cancellationToken:token);

            if (result.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<TvShowsGetObjectCallViewModel>(await result.Content.ReadAsStringAsync());


                foreach (var tvshow in data.tv_shows)
                {
                    try
                    {
                        var model = new CreateTvShowViewModel() { Title = tvshow.name, Description = tvshow.name, StillGoing = tvshow.status.ToLower().Contains("running") };
                        await Create(model);
                    }
                    catch (AlreadyExistsException e)
                    { continue; }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

            }

        }

        public async Task<GetActorViewModel> GetAllActors(int tvShowId,CancellationToken token)
        {


            var tvshows = await _context.TvShows
                .Where(x => x.Id == tvShowId)
                .Include(x => x.ActorTvShow)
                .ThenInclude(x => x.Actor)
                .AsNoTracking().ToListAsync(cancellationToken:token);

            if (tvshows.Count == 0)
                throw new NotFoundException("Not found");

            return new GetActorViewModel()
            {
                Actors = tvshows.First().ActorTvShow.Select(t => t.Actor).ToList()
            };
        }

        public async Task<TvShow> Update(int tvShowId, PutTvShowViewModel model)
        {

            var tvshow = await this.GetById(tvShowId);

            if (tvshow == null)
                throw new NotFoundException("Not found");


            if (model.Title != null)
            {
                tvshow.Title = model.Title;
            }
            if (model.Description != null)
            {
                tvshow.Description = model.Description;
            }
            if (model.Available.HasValue)
            {
                tvshow.Available = model.Available.Value;
            }

            _context.TvShows.Update(tvshow);

            await _uow.Commit();

            return tvshow;
        }
    }
}
