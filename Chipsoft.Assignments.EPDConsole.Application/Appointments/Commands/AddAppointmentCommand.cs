using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;

namespace Chipsoft.Assignments.EPDConsole.Application.Appointments.Commands
{
    public class AddAppointmentCommand : IRequest<int>
    {
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }

    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddAppointmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                PhysicianId = request.PhysicianId,
                AppointmentDateTime = request.AppointmentDateTime
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment.Id;
        }
    }
} 