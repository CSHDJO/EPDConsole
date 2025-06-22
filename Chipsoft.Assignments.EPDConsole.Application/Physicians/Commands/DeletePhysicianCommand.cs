using MediatR;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Physicians.Commands;

public class DeletePhysicianCommand : IRequest
{
    public int Id { get; set; }
}

public class DeletePhysicianCommandHandler : IRequestHandler<DeletePhysicianCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePhysicianCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeletePhysicianCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Physicians
            .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new Exception($"Physician with id {request.Id} not found");
        var hasAppointments = await _context.Appointments.AnyAsync(a => a.PhysicianId == request.Id, cancellationToken);
        if(hasAppointments)
        {
            throw new Exception("Cannot delete physician with active appointments.");
        }

        _context.Physicians.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
