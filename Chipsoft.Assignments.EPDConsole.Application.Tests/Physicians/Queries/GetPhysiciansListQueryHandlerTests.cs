using Chipsoft.Assignments.EPDConsole.Application.Physicians.Queries;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Physicians.Queries;

public class GetPhysiciansListQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly GetPhysiciansListQueryHandler _handler;

    public GetPhysiciansListQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetPhysiciansListQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Physicians_As_Dtos()
    {
        // Arrange
        var physicians = new List<Physician>
        {
            new Physician { Id = 1, FirstName = "Dr.", LastName = "Who" },
            new Physician { Id = 2, FirstName = "Dr.", LastName = "Strange" }
        };

        _contextMock.Setup(c => c.Physicians).ReturnsDbSet(physicians);

        var query = new GetPhysiciansListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Dr. Who");
        result[1].Name.Should().Be("Dr. Strange");
    }
}
