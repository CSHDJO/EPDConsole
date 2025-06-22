using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chipsoft.Assignments.EPDConsole.Application.Appointments.Commands
{
    public class AddAppointmentCommandValidator : AbstractValidator<AddAppointmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddAppointmentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.AppointmentDateTime)
                .GreaterThan(DateTime.Now).WithMessage("Een afspraak moet in de toekomst worden gepland.");

            RuleFor(v => v.PatientId)
                .NotEmpty().WithMessage("Patiënt ID is verplicht.")
                .MustAsync(PatientExists).WithMessage("De geselecteerde patiënt bestaat niet.");

            RuleFor(v => v.PhysicianId)
                .NotEmpty().WithMessage("Arts ID is verplicht.")
                .MustAsync(PhysicianExists).WithMessage("De geselecteerde arts bestaat niet.");

            RuleFor(v => v)
                .MustAsync(BeAvailable).WithMessage("De arts of patiënt heeft op dit tijdstip al een andere afspraak.");
        }

        private async Task<bool> PatientExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Patients.AnyAsync(p => p.Id == id, cancellationToken);
        }
        
        private async Task<bool> PhysicianExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Physicians.AnyAsync(p => p.Id == id, cancellationToken);
        }

        private async Task<bool> BeAvailable(AddAppointmentCommand command, CancellationToken cancellationToken)
        {
            var appointmentTime = command.AppointmentDateTime;
            var appointmentEndTime = appointmentTime.AddMinutes(30); // Assuming 30 minute slots

            var physicianHasConflict = await _context.Appointments
                .AnyAsync(a => a.PhysicianId == command.PhysicianId &&
                               appointmentTime < a.AppointmentDateTime.AddMinutes(30) &&
                               appointmentEndTime > a.AppointmentDateTime, cancellationToken);

            if (physicianHasConflict) return false;

            var patientHasConflict = await _context.Appointments
                .AnyAsync(a => a.PatientId == command.PatientId &&
                               appointmentTime < a.AppointmentDateTime.AddMinutes(30) &&
                               appointmentEndTime > a.AppointmentDateTime, cancellationToken);

            return !patientHasConflict;
        }
    }
} 