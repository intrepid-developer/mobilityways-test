using MediatR;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Persistence;

namespace MobilityWays.Application.Queries;
public sealed class GetUserQuery : IRequest<GetUserQuery.Result>
{
    public required string Email { get; set; }
    public required string Password { get; set; }

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

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQuery.Result>
{
    private readonly IUserStore _userStore;

    public GetUserQueryHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<GetUserQuery.Result> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = _userStore.Users.Where(_ => _.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        //Verify the Users Password against the hash
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            //Throw User Not Found so that we don't let a potential attacker know the password is incorrect;
            throw new UserNotFoundException();
        }

        return new GetUserQuery.Result(user.Name, user.Email);
    }
}