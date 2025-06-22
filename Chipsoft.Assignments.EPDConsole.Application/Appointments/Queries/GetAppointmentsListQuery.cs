using Chipsoft.Assignments.EPDConsole.Application.Appointments.Dtos;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Appointments.Queries;

public class GetAppointmentsListQuery : IRequest<List<AppointmentDto>>
{
}

public class GetAppointmentsListQueryHandler : IRequestHandler<GetAppointmentsListQuery, List<AppointmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAppointmentsListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
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
