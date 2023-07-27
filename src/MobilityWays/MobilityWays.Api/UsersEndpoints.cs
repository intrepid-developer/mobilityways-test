using MediatR;
using MobilityWays.Application.Commands;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Helpers;
using MobilityWays.Application.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MobilityWays.Api;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, string jwtSecret)
    {
        app.MapPost("/api/CreateUser",
            async (HttpContext httpContext,
            IMediator mediator,
            CreateUserCommand command,
            CancellationToken cancellationToken) =>
            {
                try
                {
                    //Send our command via mediator
                    await mediator.Send(command, cancellationToken);
                    return Results.Ok();
                }
                catch (InvalidUserException iue)
                {
                    return Results.BadRequest(iue.Message);
                }
                catch (UserMustBeUniqueException umbue)
                {
                    return Results.BadRequest(umbue.Message);
                }
                catch (Exception)
                {
                    return Results.BadRequest("Sorry something went wrong");
                }
            })
        .WithOpenApi();


        //Posting as we're sending credentials
        app.MapPost("/api/GetJwt",
            async (HttpContext httpContext,
            IMediator mediator,
            GetUserQuery query,
            CancellationToken cancellationToken) =>
            {
                try
                {
                    //Generate our JWT token to use in Auth Header for next call
                    var user = await mediator.Send(query, cancellationToken);
                    var jwt = JwtHelper.GenerateJwt(user.Name, user.Email, jwtSecret);
                    return Results.Ok(jwt);
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound("User Not Found or Password Incorrect");
                }
                catch (Exception)
                {
                    return Results.BadRequest("Sorry something went wrong");
                }
            })
        .WithOpenApi();


        app.MapGet("/api/ListUsers",
            async (HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                try
                {
                    //Get the Claims from the principal
                    ClaimsIdentity? currentIdentity = (ClaimsIdentity?)httpContext?.User?.Identity;
                    ArgumentNullException.ThrowIfNull(currentIdentity);

                    //Extract Claims from principal
                    var name = currentIdentity?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? string.Empty;
                    var email = currentIdentity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;

                    //Check they match a registered user
                    var userIsValid = await mediator.Send(new CheckUserIsValidQuery() { Email = email, Name = name }, cancellationToken);
                    if (!userIsValid)
                    {
                        return Results.Forbid();
                    }

                    //Return the list of users
                    var users = await mediator.Send(new GetUsersQuery(), cancellationToken);
                    return Results.Ok(users.ToList());
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound("Failed to find user for JWT");
                }
                catch (Exception)
                {
                    return Results.BadRequest("Sorry something went wrong");
                }
            })
        .WithOpenApi()
        .RequireAuthorization();
    }
}