using MobilityWays.Application.Entities;

namespace MobilityWays.Application.Persistence;

//User Store/Persistence abstraction so we can use whatever storage medium in the future
public interface IUserStore
{
    ICollection<User> Users { get; }

    //Although in this example the implementation is in memory, it could be swapped out for a more permanent store. Most use some form of saving, mimicking EF Core.
    //Async because most permanent stores will support async.
    Task SaveChangesAsync(CancellationToken? cancellationToken);
}
