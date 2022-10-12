using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ITvShowService _tvshowService;
        public UserService(AppDbContext context, ITvShowService tvshowService)
        {
            _context = context;
            _tvshowService = tvshowService;
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

             await _context.SaveChangesAsync();

            return user;
        }

        public async Task Favorite(string userEmail, FavoriteTvShowViewModel model)
        {
            var userDb = await GetByEmail(userEmail);

            if (userDb == null)
                throw new NotFoundException("User already Exists!");

            var tvshowDb = await _tvshowService.GetById(model.tvshowId);

            var favoriteShow = userDb.UserTvShowFavorite.FirstOrDefault(x => x.TvShowsId == tvshowDb.Id);

            var canFavorite = model.favorite && favoriteShow == null;

            if (canFavorite)
            {

                _context.UserTvShowFavorite.Add(new UserTvShowFavorite()
                {
                    User = userDb,
                    TvShow = tvshowDb
                });
            }
            if(!model.favorite)
            {
                _context.UserTvShowFavorite.Remove(favoriteShow);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<GetUsersViewModel> GetAll(int skip, int take)
        {
            var totalUsers = await _context.Users.CountAsync();


            var users = await _context.Users.AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new GetUsersViewModel()
            {
                TotalUsers = totalUsers,
                Users = users
            };
        }

        public Task<User> GetByEmail(string email)
        {
            return  _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetById(int id)
        {
            return _context.Users.Include(x => x.UserTvShowFavorite).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
