using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public  User Create(CreateUserViewModel model)
        {

            var userDb = GetByEmail(model.Email);

            if (userDb != null)
                throw new AlreadyExistsException("User already Exists!");

            var user = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

             _context.Users.AddAsync(user);

             _context.SaveChangesAsync();

            return user;
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
            return  _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetById(int id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
