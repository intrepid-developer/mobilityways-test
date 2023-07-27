using MobilityWays.Application.Entities;

namespace MobilityWays.Application.Persistence;
public interface IUserStore
{
    ICollection<User> Users { get; }

    Task SaveChangesAsync(CancellationToken? cancellationToken);
}
