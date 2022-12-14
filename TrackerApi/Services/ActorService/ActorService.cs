using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.ActorService.ViewModel;
using TrackerApi.Services.Erros;
using TrackerApi.Services.SharedServices;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Transaction;

namespace TrackerApi.Services.ActorService
{
    public class ActorService : DbContextService, IActorService
    {
        private readonly ITvShowService _tvshowService;

        public ActorService(AppDbContext context, ITvShowService tvshowService, IUnitOfWork uow) : base(context, uow)
        {
            _tvshowService = tvshowService;
        }

        public async Task<Actor> GetByName(string name)
        {
            return await _context.Actors.Include(x => x.ActorTvShow).FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Actor> GetById(int id)
        {
            return await _context.Actors.Include(x => x.ActorTvShow).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Actor> Create(CreateActorViewModel model)
        {
            var actorDb = await GetByName(model.Name);

            if (actorDb != null)
                throw new AlreadyExistsException("The Actor already exists!");

            var actor = new Actor()
            {
                Name = model.Name,
                Description = model.Description
            };


            await _context.Actors.AddAsync(actor);

            await _uow.Commit();

            return actor;
        }

        public async Task<GetActorViewModel> GetAll(int skip, int take, GetActorFiltersViewModel filter,CancellationToken token)
        {
            var totalactors = await _context.Actors.CountAsync();


            var actors = _context.Actors
                .Include(x => x.ActorTvShow)
                .ThenInclude(x => x.TvShow)
                .AsNoTracking()
                .Skip(skip)
                .Take(take).AsQueryable();


            if (!String.IsNullOrEmpty(filter.Name))
            {
                actors = actors.Where(x => x.Name == filter.Name);
            }

            

            return new GetActorViewModel()
            {
                TotalActors = totalactors,
                Actors = await actors.ToListAsync(cancellationToken:token)
            };
        }

        public async Task AddToTvShow(int actorId, int tvShowId)
        {
            var actorDb = await GetById(actorId);

            if (actorDb == null)
                throw new NotFoundException("Actor was not found!");

            var tvshowDb = await _tvshowService.GetById(tvShowId);


            if (tvshowDb == null)
                throw new NotFoundException("Tv Show was not found!");

            if (actorDb.ActorTvShow.Any())
                throw new AlreadyExistsException("The Actor already is part of the tv show!");

            var actorTvShow = new ActorTvShow()
            {
                Actor = actorDb,
                TvShow = tvshowDb,
            };

            actorDb.ActorTvShow.Add(actorTvShow);

            await _uow.Commit();

        }
    }
}
