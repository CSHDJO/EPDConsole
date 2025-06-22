using Chipsoft.Assignments.EPDConsole.Application.Patients.Queries;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Patients.Queries
{
    public class GetPatientsListQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly GetPatientsListQueryHandler _handler;

        public GetPatientsListQueryHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new GetPatientsListQueryHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_All_Patients_As_Dtos()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "John", LastName = "Doe", BSN = "123456789" },
                new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", BSN = "987654321" }
            };

            _contextMock.Setup(c => c.Patients).ReturnsDbSet(patients);

            var query = new GetPatientsListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("John Doe");
            result[1].BSN.Should().Be("987654321");
        }
    }
} 