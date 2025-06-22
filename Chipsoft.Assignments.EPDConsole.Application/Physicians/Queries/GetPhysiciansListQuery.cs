using MediatR;
using System.Collections.Generic;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chipsoft.Assignments.EPDConsole.Application.Physicians.Queries
{
    public class PhysicianDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetPhysiciansListQuery : IRequest<List<PhysicianDto>>
    {
    }

    public class GetPhysiciansListQueryHandler : IRequestHandler<GetPhysiciansListQuery, List<PhysicianDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetPhysiciansListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

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
} 