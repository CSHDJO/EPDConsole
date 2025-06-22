using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Chipsoft.Assignments.EPDConsole.Application.Patients.Commands;

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

        RuleFor(v => v.Address)
            .NotEmpty().WithMessage("Adres is een verplicht veld.");

        RuleFor(v => v.BSN)
            .NotEmpty().WithMessage("BSN is een verplicht veld.")
            .Length(9).WithMessage("BSN moet exact 9 cijfers bevatten.")
            .Matches("^[0-9]*$").WithMessage("BSN mag alleen cijfers bevatten.")
            .MustAsync(BeUniqueBsn).WithMessage("Een patiënt met dit BSN bestaat al.");
        
        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage("Telefoonnummer is een verplicht veld.")
            .MinimumLength(10).WithMessage("Telefoonnummer moet minimaal 10 tekens lang zijn.")
            .Matches(new Regex(@"^[\d\s\(\)\+\-]+$")).WithMessage("Telefoonnummer mag alleen nummers en gebruikelijke tekens (+, -, (, )) bevatten.");

        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(v => v.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now).WithMessage("Geboortedatum kan niet in de toekomst liggen.");
    }

    private async Task<bool> BeUniqueBsn(string bsn, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .AllAsync(p => p.BSN != bsn, cancellationToken);
    }
}
