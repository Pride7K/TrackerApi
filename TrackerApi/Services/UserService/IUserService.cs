using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.UserService
{
    public interface IUserService
    {
        Task<GetUsersViewModel> GetAll(int skip, int take);
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<User> Create(CreateUserViewModel model);
        Task Favorite(string userEmail, FavoriteTvShowViewModel model);
    }
}
