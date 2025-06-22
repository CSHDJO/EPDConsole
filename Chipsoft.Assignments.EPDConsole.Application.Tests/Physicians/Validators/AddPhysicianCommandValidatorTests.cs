using Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Physicians.Validators
{
    public class AddPhysicianCommandValidatorTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly AddPhysicianCommandValidator _validator;

        public AddPhysicianCommandValidatorTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _contextMock.Setup(x => x.Physicians).ReturnsDbSet(new List<Physician>());
            _validator = new AddPhysicianCommandValidator(_contextMock.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_FirstName_Is_Empty()
        {
            var command = new AddPhysicianCommand { FirstName = string.Empty, LastName = "Test" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public async Task Should_Have_Error_When_LastName_Is_Empty()
        {
            var command = new AddPhysicianCommand { FirstName = "Test", LastName = string.Empty };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public async Task Should_Have_Error_When_FirstName_Exceeds_MaxLength()
        {
            var command = new AddPhysicianCommand { FirstName = new string('a', 201), LastName = "Test" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public async Task Should_Have_Error_When_LastName_Exceeds_MaxLength()
        {
            var command = new AddPhysicianCommand { FirstName = "Test", LastName = new string('a', 201) };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Name_Is_Unique()
        {
            var command = new AddPhysicianCommand { FirstName = "Test", LastName = "Physician" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Not_Unique()
        {
            // Arrange
            var existingPhysicians = new List<Physician>
            {
                new Physician { FirstName = "Test", LastName = "Physician" }
            };
            _contextMock.Setup(x => x.Physicians).ReturnsDbSet(existingPhysicians);
            var validator = new AddPhysicianCommandValidator(_contextMock.Object);
            var command = new AddPhysicianCommand { FirstName = "Test", LastName = "Physician" };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.Errors.Should().Contain(e => e.ErrorMessage == "Een arts met deze naam bestaat al.");
        }
    }
} 