using Chipsoft.Assignments.EPDConsole.Application.Patients.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Patients.Validators
{
    public class AddPatientCommandValidatorTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly AddPatientCommandValidator _validator;

        public AddPatientCommandValidatorTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            // Setup the Patients DbSet mock
            _contextMock.Setup(x => x.Patients).ReturnsDbSet(new List<Patient>());
            _validator = new AddPatientCommandValidator(_contextMock.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_FirstName_Is_Empty()
        {
            var command = new AddPatientCommand { FirstName = string.Empty };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public async Task Should_Have_Error_When_LastName_Is_Empty()
        {
            var command = new AddPatientCommand { LastName = string.Empty };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
        
        [Fact]
        public async Task Should_Have_Error_When_BSN_Is_Invalid()
        {
            var command = new AddPatientCommand { BSN = "123" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.BSN);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new AddPatientCommand
            {
                FirstName = "John",
                LastName = "Doe",
                BSN = "123456789",
                Email = "john.doe@test.com",
                DateOfBirth = new System.DateTime(1990, 1, 1),
                PhoneNumber = "0612345678"
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
} 