using Chipsoft.Assignments.EPDConsole.Application.Appointments.Dtos;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chipsoft.Assignments.EPDConsole.Application.Appointments.Queries
{
    public class GetAppointmentsByPhysicianQuery : IRequest<List<AppointmentDto>>
    {
        public int PhysicianId { get; set; }
    }

    public class GetAppointmentsByPhysicianQueryHandler : IRequestHandler<GetAppointmentsByPhysicianQuery, List<AppointmentDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAppointmentsByPhysicianQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByPhysicianQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Where(a => a.PhysicianId == request.PhysicianId)
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .OrderBy(a => a.AppointmentDateTime)
                .Select(a => new AppointmentDto
                {
                    AppointmentDateTime = a.AppointmentDateTime,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    PhysicianName = $"{a.Physician.FirstName} {a.Physician.LastName}"
                })
                .ToListAsync(cancellationToken);
        }
    }
} 