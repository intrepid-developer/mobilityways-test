using MediatR;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Persistence;

namespace MobilityWays.Application.Queries;
public class GetUserQuery : IRequest<GetUserQuery.Result>
{
    public string Email { get; set; }
    public string Password { get; set; }

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

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQuery.Result>
{
    private readonly IUserStore _userStore;

    public GetUserQueryHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<GetUserQuery.Result> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = _userStore.Users.Where(_ => _.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase)
                                            && _.Password.Equals(request.Password))
                                    .Select(_ => new GetUserQuery.Result(_.Name, _.Email))
                                    .FirstOrDefault();

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}