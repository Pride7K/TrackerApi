using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService;

namespace TrackerApi.Services.TvShowService
{
    public class TvShowService : ITvShowService
    {
        const string GENRE_KEY = "genre";
        const string AVAILABLE_KEY = "available";
        const string STILL_GOING_KEY = "still_going";

        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        public TvShowService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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
                StillGoing = model.StillGoing
            };


            await _context.TvShows.AddAsync(tvshow);

            await _context.SaveChangesAsync();

            return tvshow;
        }

        public async Task Delete(int id)
        {
            var tvshow = await this.GetById(id);

            if (tvshow == null)
                throw new NotFoundException("Not found");

            _context.TvShows.Remove(tvshow);

            await _context.SaveChangesAsync();
        }

        public IEnumerable<TvShow> SeparateSort<T>(IQueryable<TvShow> tvShows, Func<TvShow, T> keySelector, GetTvShowFiltersViewModel filter)
        {
            return filter.Sort == "ASC"
                ? tvShows.OrderBy(keySelector)
                : tvShows.OrderByDescending(keySelector);
        }

        public async Task<GetTvShowViewModel> GetRecomendationsAll(int skip, int take, GetTvShowFiltersViewModel filter)
        {
            var data = await GetAll(skip, take, filter);

            data.TvShows = GetHalfOfTheList(data.TvShows.OrderBy(x => Guid.NewGuid()).ToList());

            return data;
        }

        public List<T>  GetHalfOfTheList<T>(List<T> list)
        {
            return list.Select((x, i) => new { x, i }).Where(t => t.i % 2 == 0).Select(t => t.x).ToList();
        }

        public async Task<GetTvShowViewModel> GetAll(int skip, int take, GetTvShowFiltersViewModel filter)
        {
            var totalTvShows = await _context.TvShows.CountAsync();


            var tvshows =  _context.TvShows
                .Include(x => x.Episodes)
                .AsNoTracking()
                .Skip(skip)
                .Take(take).AsQueryable();

            if(filter != null)
            {
                if(filter.Available.HasValue)
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

                if(!string.IsNullOrEmpty(filter.Sort))
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
                TvShows =  tvshows.ToList()
            };
        }

        public Task<TvShow> GetById(int id)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<TvShow> GetByTitle(string title)
        {
            return _context.TvShows.FirstOrDefaultAsync(x => x.Title == title);
        }

        public Task<TvShow> GetTvShowWithEpisode(int tvShowId)
        {
            return _context.TvShows.Include(x => x.Episodes).FirstOrDefaultAsync(x => x.Id == tvShowId);
        }

        public async Task Load()
        {
            HttpClient client = new HttpClient();

            var result = await client.GetAsync("https://www.episodate.com/api/most-popular?page=1");

            if (result.IsSuccessStatusCode)
            {
                 var data =  JsonConvert.DeserializeObject <TvShowsGetObjectCallViewModel>(await result.Content.ReadAsStringAsync());

                Console.WriteLine(data.tv_shows);

                foreach(var tvshow in data.tv_shows)
                {
                    var model = new CreateTvShowViewModel() { Title = tvshow.name, Description = tvshow.name, StillGoing = tvshow.status.ToLower().Contains("running") };
                    await Create(model);
                }

            }

        }

        public async Task<GetActorViewModel> GetAllActors(int tvShowId)
        {


            var tvshows = await _context.TvShows
                .Where(x => x.Id == tvShowId)
                .Include(x => x.ActorTvShow)
                .ThenInclude(x => x.Actor)
                .AsNoTracking().ToListAsync();

            if(tvshows.Count == 0)
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

            await _context.SaveChangesAsync();

            return tvshow;
        }
    }
}
