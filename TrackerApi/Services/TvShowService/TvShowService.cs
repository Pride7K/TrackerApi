using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Services.TvShowService
{
    public class TvShowService : ITvShowService
    {
        private readonly AppDbContext _context;
        public TvShowService(AppDbContext context)
        {
            _context = context;
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

        public async Task<GetTvShowViewModel> GetAll(int skip, int take)
        {
            var totalTvShows = await _context.TvShows.CountAsync();


            var tvshows = await _context.TvShows
                .Where(x => x.Available)
                .Include(x => x.Episodes)
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new GetTvShowViewModel()
            {
                TotalTvShows = totalTvShows,
                TvShows = tvshows
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
