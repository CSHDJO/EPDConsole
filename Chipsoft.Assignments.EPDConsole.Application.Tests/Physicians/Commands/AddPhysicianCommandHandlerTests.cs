using Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Moq;
using Moq.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Physicians.Commands;

public class AddPhysicianCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly AddPhysicianCommandHandler _handler;

    public AddPhysicianCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new AddPhysicianCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Add_Physician_And_Save_Changes()
    {
        // Arrange
        var physicians = new List<Physician>();
        _contextMock.Setup(c => c.Physicians).ReturnsDbSet(physicians);

        var command = new AddPhysicianCommand
        {
            FirstName = "Test",
            LastName = "Physician"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contextMock.Verify(x => x.Physicians.Add(It.IsAny<Physician>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
