using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.SharedServices;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.UserService.ViewModel;
using TrackerApi.Transaction;

namespace TrackerApi.Services.UserService
{
    public class UserService : DbContextService,IUserService
    {
        private readonly ITvShowService _tvshowService;
        public UserService(AppDbContext context, ITvShowService tvshowService, IUnitOfWork uow) : base(context,uow)
        {
            _tvshowService = tvshowService;
        }

        public Task<User> GetByEmail(string email)
        {
            return  _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Email == email);
        }

        public  async Task<User> Create(CreateUserViewModel model)
        {

            var userDb = await GetByEmail(model.Email);

            if (userDb != null)
                throw new AlreadyExistsException("User already Exists!");

            var user = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

             await _context.Users.AddAsync(user);

            await _uow.Commit();

            return user;
        }

        public async Task Favorite(string userEmail, FavoriteTvShowViewModel model)
        {
            var userDb = await GetByEmail(userEmail);

            if (userDb == null)
                throw new NotFoundException("User Not Found!");

            var tvshowDb = await _tvshowService.GetById(model.tvshowId);

            if (tvshowDb == null)
                throw new NotFoundException("Tv Show Not Found!");

            var favoriteShow = userDb.UserTvShowFavorite.FirstOrDefault(x => x.TvShowsId == tvshowDb.Id);

            var canFavorite = model.favorite && favoriteShow == null;

            var canUnfavorite = model.favorite && favoriteShow != null;

            if (canFavorite)
            {

                _context.UserTvShowFavorite.Add(new UserTvShowFavorite()
                {
                    User = userDb,
                    TvShow = tvshowDb
                });
            }
            if(canUnfavorite)
            {
                _context.UserTvShowFavorite.Remove(favoriteShow);
            }

             await _uow.Commit();
        }

        public async Task<GetUsersViewModel> GetAll(int skip, int take,CancellationToken token)
        {
            var totalUsers = await _context.Users.CountAsync(cancellationToken:token);


            var users = await _context.Users.AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken:token);

            return new GetUsersViewModel()
            {
                TotalUsers = totalUsers,
                Users = users
            };
        }

        public Task<User> GetById(int id)
        {
            return _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<User> GetByEmail(string email,CancellationToken token)
        {
            return  _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Email == email,cancellationToken:token);
        }

        public Task<User> GetById(int id,CancellationToken token)
        {
            return _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Id == id,cancellationToken:token);
        }
    }
}
