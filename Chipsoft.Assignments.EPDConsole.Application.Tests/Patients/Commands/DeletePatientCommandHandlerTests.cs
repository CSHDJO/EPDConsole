using Chipsoft.Assignments.EPDConsole.Application.Patients.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Patients.Commands
{
    public class DeletePatientCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        
        public DeletePatientCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Handle_Should_Remove_Patient_When_Found()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "Test", LastName = "Patient" };
            var mockDbSet = new Mock<DbSet<Patient>>();
            mockDbSet.Setup(m => m.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _contextMock.Setup(c => c.Patients).Returns(mockDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(new List<Appointment>());
            
            var handler = new DeletePatientCommandHandler(_contextMock.Object);
            var command = new DeletePatientCommand { Id = 1 };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockDbSet.Verify(x => x.Remove(patient), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Patient_Not_Found()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Patient>>();
            mockDbSet.Setup(m => m.FindAsync(new object[] { 99 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null);

            _contextMock.Setup(c => c.Patients).Returns(mockDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(new List<Appointment>());

            var handler = new DeletePatientCommandHandler(_contextMock.Object);
            var command = new DeletePatientCommand { Id = 99 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Patient with id 99 not found");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Patient_Has_Appointments()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "Test", LastName = "Patient" };
            var appointments = new List<Appointment> { new Appointment { PatientId = 1 } };
            
            var mockPatientDbSet = new Mock<DbSet<Patient>>();
            mockPatientDbSet.Setup(m => m.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _contextMock.Setup(c => c.Patients).Returns(mockPatientDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(appointments);

            var handler = new DeletePatientCommandHandler(_contextMock.Object);
            var command = new DeletePatientCommand { Id = 1 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Cannot delete patient with active appointments.");
        }
    }
} 