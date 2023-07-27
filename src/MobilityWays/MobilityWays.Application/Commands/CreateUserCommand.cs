using FluentValidation;
using MediatR;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Persistence;

namespace MobilityWays.Application.Commands;

//Using Mediator to do some basic CQS
public sealed class CreateUserCommand : IRequest<Unit>
{
    public CreateUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IUserStore _userStore;
    public CreateUserCommandHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //Validate the Command
        ValidateCommand(request);

        //Check if the User Exists, this could also be part of Validate
        CheckIfUserExists(request.Email.Trim());

        //User is valid so create it
        _userStore.Users.Add(new Entities.User(request.Name, request.Email, request.Password));

        //Save Changes
        await _userStore.SaveChangesAsync(cancellationToken);

        //Mediator has to return something so we use the Unit.Value as this is essentially a void return.
        return Unit.Value;
    }

    private void CheckIfUserExists(string email)
    {
        //Check if a user already exists using the Any check, the string comparison will ignore case, so we don't have to call .ToLower
        if(_userStore.Users.Any(_ => _.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))){
            throw new UserMustBeUniqueException("This email is already in use");
        }
    }

    private void ValidateCommand(CreateUserCommand request)
    {
        //We could trigger validation automatically through Mediator Behaviours, but wanted to also show how we can manually run a validation to better control what happens
        var validator = new CreateUserCommandValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            string errorMessage = string.Join(Environment.NewLine, validationResult.Errors.Select(_ => _.ErrorMessage));
            throw new InvalidUserException(errorMessage);
        }
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    // I know there was no requirement for Password Length or Validation Rules I thought it would be good to show how it could be done
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name is too long");


        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(255)
            .MinimumLength(5)
            .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
            .WithMessage("Invalid Email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(7)
            .WithMessage("Password must be at least 7 characters");

        RuleFor(x => x.Password)
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase character");

        RuleFor(x => x.Password)
           .Matches("[a-z]")
           .WithMessage("Password must contain at least one lowercase character");

        RuleFor(x => x.Password)
           .Matches("[0-9]")
           .WithMessage("Password must contain at least one numeric character");

        RuleFor(x => x.Password)
           .Matches("[^A-Za-z0-9]")
           .WithMessage("Password must contain at least one symbol character");
    }
}
