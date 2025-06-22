using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands
{
    public class AddPhysicianCommand : IRequest<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class AddPhysicianCommandHandler : IRequestHandler<AddPhysicianCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddPhysicianCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddPhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = new Physician
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            _context.Physicians.Add(physician);

            await _context.SaveChangesAsync(cancellationToken);

            return physician.Id;
        }
    }
} 