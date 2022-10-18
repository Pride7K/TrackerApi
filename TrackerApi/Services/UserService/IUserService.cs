using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Services.UserService
{
    public interface IUserService
    {
        Task<GetUsersViewModel> GetAll(int skip, int take,CancellationToken token);
        Task<User> GetById(int id,CancellationToken token);
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email,CancellationToken token);
        Task<User> GetByEmail(string email);
        Task<User> Create(CreateUserViewModel model);
        Task Favorite(string userEmail, FavoriteTvShowViewModel model);
    }
}
