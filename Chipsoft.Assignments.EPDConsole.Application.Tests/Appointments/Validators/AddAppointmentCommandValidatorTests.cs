using Chipsoft.Assignments.EPDConsole.Application.Appointments.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Appointments.Validators
{
    public class AddAppointmentCommandValidatorTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;

        public AddAppointmentCommandValidatorTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Appointment_Is_Valid()
        {
            // Arrange
            _contextMock.Setup(x => x.Patients).ReturnsDbSet(new List<Patient> { new Patient { Id = 1 } });
            _contextMock.Setup(x => x.Physicians).ReturnsDbSet(new List<Physician> { new Physician { Id = 1 } });
            _contextMock.Setup(x => x.Appointments).ReturnsDbSet(new List<Appointment>());
            
            var validator = new AddAppointmentCommandValidator(_contextMock.Object);
            var command = new AddAppointmentCommand { PatientId = 1, PhysicianId = 1, AppointmentDateTime = DateTime.Now.AddDays(1) };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Should_Have_Error_When_Physician_Has_Conflict()
        {
            // Arrange
            var appointmentTime = DateTime.Now.AddDays(1);
            var existingAppointments = new List<Appointment>
            {
                new Appointment { PhysicianId = 1, AppointmentDateTime = appointmentTime }
            };
            
            _contextMock.Setup(x => x.Patients).ReturnsDbSet(new List<Patient> { new Patient { Id = 1 } });
            _contextMock.Setup(x => x.Physicians).ReturnsDbSet(new List<Physician> { new Physician { Id = 1 } });
            _contextMock.Setup(x => x.Appointments).ReturnsDbSet(existingAppointments);

            var validator = new AddAppointmentCommandValidator(_contextMock.Object);
            var command = new AddAppointmentCommand { PatientId = 1, PhysicianId = 1, AppointmentDateTime = appointmentTime };
            
            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.Errors.Should().Contain(e => e.ErrorMessage == "De arts of patiënt heeft op dit tijdstip al een andere afspraak.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Patient_Has_Conflict()
        {
            // Arrange
            var appointmentTime = DateTime.Now.AddDays(1);
            var existingAppointments = new List<Appointment>
            {
                new Appointment { PatientId = 1, AppointmentDateTime = appointmentTime }
            };
            
            _contextMock.Setup(x => x.Patients).ReturnsDbSet(new List<Patient> { new Patient { Id = 1 } });
            _contextMock.Setup(x => x.Physicians).ReturnsDbSet(new List<Physician> { new Physician { Id = 1 } });
            _contextMock.Setup(x => x.Appointments).ReturnsDbSet(existingAppointments);

            var validator = new AddAppointmentCommandValidator(_contextMock.Object);
            var command = new AddAppointmentCommand { PatientId = 1, PhysicianId = 1, AppointmentDateTime = appointmentTime };
            
            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.Errors.Should().Contain(e => e.ErrorMessage == "De arts of patiënt heeft op dit tijdstip al een andere afspraak.");
        }
    }
} 