using Chipsoft.Assignments.EPDConsole.Application.Appointments.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Appointments.Commands
{
    public class AddAppointmentCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly AddAppointmentCommandHandler _handler;

        public AddAppointmentCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new AddAppointmentCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Add_Appointment_And_Save_Changes()
        {
            // Arrange
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(new List<Appointment>());

            var command = new AddAppointmentCommand
            {
                PatientId = 1,
                PhysicianId = 1,
                AppointmentDateTime = DateTime.Now.AddHours(1)
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _contextMock.Verify(x => x.Appointments.Add(It.IsAny<Appointment>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
} 