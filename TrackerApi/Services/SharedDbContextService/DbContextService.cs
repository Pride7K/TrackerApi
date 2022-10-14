using TrackerApi.Data;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.UserService;
using TrackerApi.Transaction;

namespace TrackerApi.Services.SharedServices
{
    public class DbContextService
    {
        protected readonly AppDbContext _context;
        protected readonly IUnitOfWork _uow;
        public DbContextService(AppDbContext context,IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }
    }
}
