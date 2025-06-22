using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chipsoft.Assignments.EPDConsole.Application.Patients.Commands
{
    public class AddPatientCommandValidator : AbstractValidator<AddPatientCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddPatientCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("Voornaam is een verplicht veld.")
                .MaximumLength(200);

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Achternaam is een verplicht veld.")
                .MaximumLength(200);

            RuleFor(v => v.BSN)
                .NotEmpty().WithMessage("BSN is een verplicht veld.")
                .Length(9).WithMessage("BSN moet 9 karakters lang zijn.")
                .MustAsync(BeUniqueBsn).WithMessage("Een patiënt met dit BSN bestaat al.");
            
            RuleFor(v => v.PhoneNumber)
                .Matches(new Regex(@"^[\d\s\(\)\+\-]+$")).WithMessage("Telefoonnummer mag alleen nummers en gebruikelijke tekens (+, -, (, )) bevatten.");

            RuleFor(v => v.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(v => v.DateOfBirth)
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("Geboortedatum moet in het verleden liggen.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date < DateTime.Now;
        }

        private async Task<bool> BeUniqueBsn(string bsn, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AllAsync(p => p.BSN != bsn, cancellationToken);
        }
    }
} 