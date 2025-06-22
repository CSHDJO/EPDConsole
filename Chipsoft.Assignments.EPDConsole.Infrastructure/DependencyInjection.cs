using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<EPDDbContext>(options =>
            options.UseSqlite("Data Source=epd.db"));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<EPDDbContext>());

        return services;
    }
}
