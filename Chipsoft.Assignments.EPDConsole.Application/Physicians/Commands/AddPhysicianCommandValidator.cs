using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands;

public class AddPhysicianCommandValidator : AbstractValidator<AddPhysicianCommand>
{
    private readonly IApplicationDbContext _context;

    public AddPhysicianCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("Voornaam is een verplicht veld.")
            .MaximumLength(200).WithMessage("Voornaam mag niet meer dan 200 karakters bevatten.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Achternaam is een verplicht veld.")
            .MaximumLength(200).WithMessage("Achternaam mag niet meer dan 200 karakters bevatten.");
        
        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Een arts met deze naam bestaat al.");
    }

    private async Task<bool> BeUniqueName(AddPhysicianCommand command, CancellationToken cancellationToken)
    {
        return await _context.Physicians
            .AllAsync(p => p.FirstName != command.FirstName || p.LastName != command.LastName, cancellationToken);
    }
}
