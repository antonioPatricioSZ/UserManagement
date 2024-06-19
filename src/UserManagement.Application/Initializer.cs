using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Application.Services.Token;
using UserManagement.Application.UseCases.Auth.AddClaimsToUser;
using UserManagement.Application.UseCases.Auth.AddRoleToUser;
using UserManagement.Application.UseCases.Auth.CreateRole;
using UserManagement.Application.UseCases.Auth.GetAllClaimsUser;
using UserManagement.Application.UseCases.Auth.GetAllRolesUser;
using UserManagement.Application.UseCases.Auth.RefreshToken;
using UserManagement.Application.UseCases.User.ChangePassword;
using UserManagement.Application.UseCases.User.GetAllUsers;
using UserManagement.Application.UseCases.User.GetById;
using UserManagement.Application.UseCases.User.Login;
using UserManagement.Application.UseCases.User.Register;
using UserManagement.Application.UseCases.User.ResetPassword;
using UserManagement.Application.UseCases.User.SendEmail;
using UserManagement.Application.UseCases.User.VerifyEmail;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.RefreshTokenRepository;

namespace UserManagement.Application;

public static class Initializer {

    public static void AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    ){
        AdicionarTokenJWT(services, configuration);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services) {
        services
            .AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>()
            .AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>()
            .AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>()
            .AddScoped<IRegisterUserUseCase, RegisterUserUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IAddClaimsToUserUseCase, AddClaimsToUserUseCase>()
            .AddScoped<IGetAllClaimsUserUseCase, GetAllClaimsUserUseCase>()
            .AddScoped<ICreateRoleUseCase, CreateRoleUseCase>()
            .AddScoped<IAddRoleToUserUseCase, AddRoleToUserUseCase>()
            .AddScoped<IGetAllRolesUserUseCase, GetAllRolesUserUseCase>()
            .AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>()
            .AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>()
            .AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>()
            .AddScoped<IVerifyEmailUseCase, VerifyEmailUseCase>();
    }

    private static void AdicionarTokenJWT(
        IServiceCollection services,
        IConfiguration configuration
    )
    {

        services.AddScoped(provider =>
        {

            var chaveDeSeguranca = configuration.GetSection("JwtConfig:Secret").Value;
            var refreshTokenWriteOnlyRepository = provider.GetService<IRefreshTokenRepository>();
            var unitOfWork = provider.GetService<IUnitOfWork>();

            var tokenValidationParameters = new TokenValidationParameters(); // Instanciando seus TokenValidationParameters conforme necessário

            var userManager = provider.GetService<UserManager<User>>();
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();

            var mapper = provider.GetService<IMapper>();
            var httpContextAccessor = provider.GetService<IHttpContextAccessor>(); // Obtendo IHttpContextAccessor

            return new TokenService(chaveDeSeguranca, refreshTokenWriteOnlyRepository, unitOfWork, userManager, roleManager, mapper);
        });
    }
}