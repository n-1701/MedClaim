using MedClaim.Policy.Application.Abstractions;
using MedClaim.Policy.Infrastructure.Persistence;
using MedClaim.Policy.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedClaim.Policy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PolicyDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("PolicyDb")));

        services.AddScoped<IPolicyRepository, PolicyRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUnitOfWork, PolicyUnitOfWork>();

        return services;
    }
}