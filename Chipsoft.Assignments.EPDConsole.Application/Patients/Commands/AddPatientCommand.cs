using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using MediatR;

namespace Chipsoft.Assignments.EPDConsole.Application.Patients.Commands;

public class AddPatientCommand : IRequest<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string BSN { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddPatientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddPatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            BSN = request.BSN,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken);
        return patient.Id;
    }
}
