using System;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Services.TvShowService;

namespace TrackerApi.Transaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void RollBack()
        {

        }
    }
}
