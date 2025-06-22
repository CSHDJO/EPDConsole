using Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands;
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

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Physicians.Commands
{
    public class DeletePhysicianCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        
        public DeletePhysicianCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Handle_Should_Remove_Physician_When_Found()
        {
            // Arrange
            var physician = new Physician { Id = 1, FirstName = "Test", LastName = "Physician" };
            var physicians = new List<Physician> { physician };
            var mockDbSet = new Mock<DbSet<Physician>>();
            mockDbSet.Setup(m => m.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(physician);

            _contextMock.Setup(c => c.Physicians).Returns(mockDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(new List<Appointment>());
            
            var handler = new DeletePhysicianCommandHandler(_contextMock.Object);
            var command = new DeletePhysicianCommand { Id = 1 };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockDbSet.Verify(x => x.Remove(physician), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Physician_Not_Found()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Physician>>();
            mockDbSet.Setup(m => m.FindAsync(new object[] { 99 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Physician)null);

            _contextMock.Setup(c => c.Physicians).Returns(mockDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(new List<Appointment>());

            var handler = new DeletePhysicianCommandHandler(_contextMock.Object);
            var command = new DeletePhysicianCommand { Id = 99 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Physician with id 99 not found");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Physician_Has_Appointments()
        {
            // Arrange
            var physician = new Physician { Id = 1, FirstName = "Test", LastName = "Physician" };
            var appointments = new List<Appointment> { new Appointment { PhysicianId = 1 } };
            
            var mockPhysicianDbSet = new Mock<DbSet<Physician>>();
            mockPhysicianDbSet.Setup(m => m.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(physician);

            _contextMock.Setup(c => c.Physicians).Returns(mockPhysicianDbSet.Object);
            _contextMock.Setup(c => c.Appointments).ReturnsDbSet(appointments);

            var handler = new DeletePhysicianCommandHandler(_contextMock.Object);
            var command = new DeletePhysicianCommand { Id = 1 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Cannot delete physician with active appointments.");
        }
    }
} 