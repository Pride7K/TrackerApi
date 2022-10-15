using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Services.Erros;
using TrackerApi.Services.JwtService;
using TrackerApi.Services.Login.ViewModel;

namespace TrackerApi.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        public LoginService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<dynamic> Authenticate(AuthenticateUserViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == x.Password);

            if (user == null)
                throw new  NotFoundException("User Not Found!");

            if (!user.Active)
                throw new UnauthorizedException("User Not Active!");

            var token = TokenService.GenerateToken(user);
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }
    }
}
