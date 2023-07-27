using MobilityWays.Application.Entities;

namespace MobilityWays.Application.Persistence;
public class InMemoryUserStore : IUserStore
{
    //Make this thread safe as will be singleton
    private readonly SemaphoreSlim _lockObj = new SemaphoreSlim(1, 1);
    private readonly List<User> _users = new();

    public ICollection<User> Users
    {
        get
        {
            try
            {
                _lockObj.Wait();
                return _users;
            }
            finally
            {
                _lockObj.Release();
            }
        }
    }

    public async Task SaveChangesAsync(CancellationToken? cancellationToken)
    {
        //Do nothing as in memory
    }
}
