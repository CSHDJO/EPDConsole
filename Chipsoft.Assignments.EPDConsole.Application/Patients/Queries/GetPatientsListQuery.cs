using MediatR;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Patients.Queries;

public class PatientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BSN { get; set; } = string.Empty;
}

public class GetPatientsListQuery : IRequest<List<PatientDto>>
{
}

public class GetPatientsListQueryHandler(IApplicationDbContext context) : IRequestHandler<GetPatientsListQuery, List<PatientDto>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<List<PatientDto>> Handle(GetPatientsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .Select(p => new PatientDto
            {
                Id = p.Id,
                Name = $"{p.FirstName} {p.LastName}",
                BSN = p.BSN
            })
            .ToListAsync(cancellationToken);
    }
}
