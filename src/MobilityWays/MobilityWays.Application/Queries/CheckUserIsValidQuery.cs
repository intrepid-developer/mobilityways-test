using MediatR;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Persistence;

namespace MobilityWays.Application.Queries;
public sealed class CheckUserIsValidQuery : IRequest<bool>
{
    public required string Email { get; set; }
    public required string Name { get; set; }
}

public sealed class GetUserFromClaimsQueryHandler : IRequestHandler<CheckUserIsValidQuery, bool>
{
    private readonly IUserStore _userStore;

    public GetUserFromClaimsQueryHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<bool> Handle(CheckUserIsValidQuery request, CancellationToken cancellationToken)
    {
        var user = _userStore.Users.Where(_ => _.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase)
                                            && _.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return true;
    }
}
