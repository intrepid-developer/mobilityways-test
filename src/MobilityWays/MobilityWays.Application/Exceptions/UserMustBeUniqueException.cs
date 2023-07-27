namespace MobilityWays.Application.Exceptions;

//Custom exceptions help to better manage and identify problems
public sealed class UserMustBeUniqueException : Exception
{
    public UserMustBeUniqueException(string errorMessage) : base(errorMessage)
    {

    }
}
