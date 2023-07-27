namespace MobilityWays.Application.Exceptions;

//Custom exceptions help to better manage and identify problems
public sealed class InvalidUserException : Exception
{
    public InvalidUserException(string errorMessage) : base(errorMessage)
    {

    }
}
