using UserManagement.Infrastructure.RepositoryAccess;
using UserManagement.Infrastructure.RepositoryAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.EmailRepository;
using UserManagement.Domain.Repositories.UserRepository;
using UserManagement.Domain.Repositories.RefreshTokenRepository;

namespace UserManagement.Infrastructure;

public static class Initializer {

    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    ){
        AddContext(services, configuration);

        AddUnitOfWork(services);

        AddRepositories(services);
    }

    private static void AddContext(
        IServiceCollection services,
        IConfiguration configuration
    ){
        var connectionString = configuration.GetConnectionString("Conexao");

        services.AddDbContext<UserManagementContext>(
            dbContextOptions => {
                dbContextOptions.UseSqlServer(connectionString, action => {
                    action.MigrationsAssembly("UserManagement.Infrastructure");
                });
                dbContextOptions.EnableDetailedErrors();
                dbContextOptions.EnableSensitiveDataLogging();                
            }
        );
    }

    private static void AddRepositories(IServiceCollection services) {
        services
            .AddScoped<IUserReadOnlyRepository, UserRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
            .AddScoped<ISendEmail, SendEmailRepository>();
    }

    private static void AddUnitOfWork(IServiceCollection services) {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

}
