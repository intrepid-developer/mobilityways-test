using MobilityWays.Application.Commands;
using MobilityWays.Application.Entities;
using MobilityWays.Application.Exceptions;
using MobilityWays.Application.Persistence;
using Moq;

namespace MobilityWays.Application.UnitTests.Commands;

//Some simple tests to cater for Password Logic and happy path

[TestClass]
public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserStore> _userStoreMock = new Mock<IUserStore>();

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenValidCommand_ShouldCreateUser()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "Pa$$w0rd");
        _userStoreMock.Setup(x => x.Users).Returns(new List<User> {});
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.AreEqual(1, _userStoreMock.Object.Users.Count);
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenNameIsNull_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand(null, "user@email.com", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenNameIsEmpty_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("", "user@email.com", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenNameLengthIsGreaterThan100_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand(new string('a', 101), "user@email.com", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenEmailIsNull_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", null, "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenEmailIsEmpty_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenNameLengthIsLessThan5_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "u@u", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenEmailLengthIsGreaterThan255_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", $"user@{new string('a', 255)}.com", "Pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordIsNull_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", null);
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordIsEmpty_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordLengthIsLessThan7_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "Pa$$w");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordDoesNotContainUppercase_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "pa$$w0rd");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordDoesNotContainLowercase_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "PA$$W0RD");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordDoesNotContainNumeric_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "Pa$$word");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenPasswordDoesNotContainSymbol_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "Password123");
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task CreateUserCommandHandler_WhenEmailIsNotUnique_ShouldThrowException()
    {
        //Arrange
        var command = new CreateUserCommand("userName", "user@email.com", "Pa$$w0rd");
        _userStoreMock.Setup(x => x.Users).Returns(new List<User> { new User("userName", "user@email.com", "Pa$$w0rd") });
        var handler = new CreateUserCommandHandler(_userStoreMock.Object);

        //Act and Assert
        await Assert.ThrowsExceptionAsync<UserMustBeUniqueException>(() => handler.Handle(command, CancellationToken.None));
    }
}