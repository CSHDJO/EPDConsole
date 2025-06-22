using Chipsoft.Assignments.EPDConsole.Application.Appointments.Queries;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Appointments.Queries
{
    public class GetAppointmentsByPhysicianQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly GetAppointmentsByPhysicianQueryHandler _handler;

        public GetAppointmentsByPhysicianQueryHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new GetAppointmentsByPhysicianQueryHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Only_Appointments_For_Given_Physician()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };
            var physician1 = new Physician { Id = 1, FirstName = "Dr.", LastName = "Smith" };
            var physician2 = new Physician { Id = 2, FirstName = "Dr.", LastName = "Who" };


            var appointments = new List<Appointment>
            {
                new Appointment { PatientId = 1, PhysicianId = 1, Patient = patient, Physician = physician1, AppointmentDateTime = DateTime.Now.AddDays(1) },
                new Appointment { PatientId = 1, PhysicianId = 2, Patient = patient, Physician = physician2, AppointmentDateTime = DateTime.Now.AddDays(2) },
                new Appointment { PatientId = 1, PhysicianId = 1, Patient = patient, Physician = physician1, AppointmentDateTime = DateTime.Now.AddDays(3) }
            };

            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(appointments);

            var query = new GetAppointmentsByPhysicianQuery { PhysicianId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(a => a.PhysicianName == "Dr. Smith").Should().BeTrue();
        }
    }
} 