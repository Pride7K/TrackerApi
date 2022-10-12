using System.Threading.Tasks;
using TrackerApi.Services.Login.ViewModel;

namespace TrackerApi.Services.LoginService
{
    public interface ILoginService
    {
        Task<dynamic> Authenticate(AuthenticateUserViewModel model);
    }
}
