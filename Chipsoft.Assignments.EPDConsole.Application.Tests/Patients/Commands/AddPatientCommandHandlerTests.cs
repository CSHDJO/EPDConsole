using Chipsoft.Assignments.EPDConsole.Application.Patients.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Moq;
using Moq.EntityFrameworkCore;
namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Patients.Commands;

public class AddPatientCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly AddPatientCommandHandler _handler;

    public AddPatientCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new AddPatientCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Add_Patient_And_Save_Changes()
    {
        // Arrange
        var patients = new List<Patient>();
        _contextMock.Setup(c => c.Patients).ReturnsDbSet(patients);

        var command = new AddPatientCommand
        {
            FirstName = "John",
            LastName = "Doe",
            BSN = "123456789",
            Address = "123 Main St",
            PhoneNumber = "555-1234",
            Email = "john.doe@test.com",
            DateOfBirth = new System.DateTime(1990, 1, 1)
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contextMock.Verify(
            x => x.Patients.Add(It.Is<Patient>(p =>
                p.FirstName == command.FirstName &&
                p.LastName == command.LastName &&
                p.BSN == command.BSN &&
                p.Address == command.Address &&
                p.PhoneNumber == command.PhoneNumber &&
                p.Email == command.Email &&
                p.DateOfBirth == command.DateOfBirth
            )), 
            Times.Once);

        _contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
