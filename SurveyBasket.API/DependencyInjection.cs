using Domain.RepositoriesContracts;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using Service.Mapping;
using ServiceAbstraction;

namespace SurveyBasket.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddOpenApi();


        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddMapsterConf();

        services.AddValidatorsFromAssemblyContaining<ServiceAbtractionReference>();


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServiceManager, ServiceManager>();
        return services;
    }

    public static IServiceCollection AddMapsterConf(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(MappingConfigurations).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }

    
}