using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Infrastructure.Messaging;
using MedClaim.Claims.Infrastructure.Persistence;
using MedClaim.Claims.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedClaim.Claims.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddClaimsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ClaimsDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("ClaimsDb")));

        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUnitOfWork, ClaimsUnitOfWork>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], "/", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
            });
        });

        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}