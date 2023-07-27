using MediatR;
using MobilityWays.Application.Persistence;

namespace MobilityWays.Application.Queries;
public class GetUsersQuery : IRequest<IEnumerable<GetUsersQuery.Result>>
{
    public class Result
    {
        public Result(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUsersQuery.Result>>
{
    private readonly IUserStore _userStore;

    public GetUsersQueryHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<IEnumerable<GetUsersQuery.Result>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return _userStore.Users.Select(_ => new GetUsersQuery.Result(_.Name, _.Email));
    }
}
