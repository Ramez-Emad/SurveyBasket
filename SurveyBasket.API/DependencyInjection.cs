using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories;
using Presentation.Filters.Authentication;
using Service;
using Service.Authentication;
using Service.Email;
using Service.Mapping;
using ServiceAbstraction;
using Shared;
using SurveyBasket.Web.Health;
using System.Text;

namespace SurveyBasket.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddHangfire(configuration);

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );

        services.AddOpenApi();

        services.AddAuthConfig(configuration);

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddMapsterConf();

        services.AddValidatorsFromAssemblyContaining<FluentValidationAssemblyReference>();
        //   .AddFluentValidationAutoValidation();

        services.Configure<MailSettings>(
                        configuration.GetSection(MailSettings.SectionName));

       
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPollService , PollService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>("Database")
                .AddHangfire(options =>
                {
                    options.MinimumAvailableServers = 1;
                })
                .AddCheck<MailProviderHealthCheck>(name: "mail service");


        return services;
    }


    private static IServiceCollection AddMapsterConf(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(MappingConfigurations).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }


    private static IServiceCollection AddAuthConfig(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings?.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings?.Audience,

                    ValidateLifetime = true,
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }


    private static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration confg)
    {
        services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(confg.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();


        return services;
    }
}