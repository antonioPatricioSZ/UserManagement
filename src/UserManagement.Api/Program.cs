using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Api.Filters;
using UserManagement.Application;
using UserManagement.Application.Services.AutoMapper;
using UserManagement.Communication.Response;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;
using UserManagement.Infrastructure.RepositoryAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Configuration.AddSecretsManager(configurator: options => {
    options.SecretFilter = entry =>
        entry.Name.StartsWith($"{env}_{appName}_") ||
        entry.Name.StartsWith($"{appName}_") ||
        entry.Name.StartsWith($"{appName}:") ||
        entry.Name.Contains($"_{appName}_");
    options.KeyGenerator = (_, s) => s.Replace($"{env}_{appName}_", string.Empty)
        .Replace($"{appName}_", string.Empty)
        .Replace("__", ":");
    options.PollingInterval = TimeSpan.FromHours(10);
});

var jwtSecret = builder.Configuration["JwtConfig:Secret"];
var apiKey = builder.Configuration["ApyKeys"];
var dbConnection = builder.Configuration.GetConnectionString("Conexao");

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionsFilter)));

builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc(
        "v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "UserManagement",
            Version = "1.0"
        }
    );
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe o Token"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement { {
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options => {
    options.AddPolicy(name: "PermitirApiRequest", build => build.WithOrigins("")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserManagementContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt => {
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecret!)),
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = false,
        ValidateAudience = false,
    };

    jwt.Events = new JwtBearerEvents
    {
        OnChallenge = async context => {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = System.Text.Json.JsonSerializer.Serialize(new ErrorResponse("Token ausente ou inválido."));
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(result);
                context.Response.CompleteAsync().Wait();
                await Task.CompletedTask;
            }
        },
        OnForbidden = async context => {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var result = System.Text.Json.JsonSerializer.Serialize(new ErrorResponse("Permissões insuficientes"));
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(result);
                context.Response.CompleteAsync().Wait();
                await Task.CompletedTask;
            }
        }
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("PermitirApiRequest");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
