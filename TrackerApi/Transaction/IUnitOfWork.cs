using System.Threading.Tasks;

namespace TrackerApi.Transaction
{
    public interface IUnitOfWork
    {
        Task Commit();

        void RollBack();
    }
}
