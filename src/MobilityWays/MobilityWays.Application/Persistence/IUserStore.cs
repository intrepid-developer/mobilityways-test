using MobilityWays.Application.Entities;

namespace MobilityWays.Application.Persistence;
public interface IUserStore
{
    ICollection<User> Users { get; }

    //Although in this example the implementation is in memory, it could be swapped out for a more permanent store. Most use some form of saving, mimicking EF Core.
    Task SaveChangesAsync(CancellationToken? cancellationToken);
}
