using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Chipsoft.Assignments.EPDConsole.Application.Patients.Commands
{
    public class DeletePatientCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePatientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Patients
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new Exception($"Patient with id {request.Id} not found");
            }
            
            var hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientId == request.Id, cancellationToken);
            if(hasAppointments)
            {
                throw new Exception("Cannot delete patient with active appointments.");
            }

            _context.Patients.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 