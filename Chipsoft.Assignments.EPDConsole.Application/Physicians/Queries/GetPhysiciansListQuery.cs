using MediatR;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Physicians.Queries;

public class PhysicianDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class GetPhysiciansListQuery : IRequest<List<PhysicianDto>>
{
}

public class GetPhysiciansListQueryHandler(IApplicationDbContext context) : IRequestHandler<GetPhysiciansListQuery, List<PhysicianDto>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<List<PhysicianDto>> Handle(GetPhysiciansListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Physicians
            .Select(p => new PhysicianDto
            {
                Id = p.Id,
                Name = $"{p.FirstName} {p.LastName}"
            })
            .ToListAsync(cancellationToken);
    }
}
