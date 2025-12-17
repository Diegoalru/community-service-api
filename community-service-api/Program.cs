using System.Text;
using community_service_api.DbContext;
using community_service_api.HostedServices;
using community_service_api.Middleware;
using community_service_api.Models;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;
using community_service_api.Services;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Oracle connection with a wallet location
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
var walletPath = Path.Combine(AppContext.BaseDirectory, "Wallet");

// Configure Oracle client to use the wallet
OracleConfiguration.WalletLocation = walletPath;
OracleConfiguration.TnsAdmin = walletPath;
DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddDbContext<NewApplicationDbContext>(options =>
    options.UseOracle(connectionString));

// Configure settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<FrontEndSettings>(builder.Configuration.GetSection("FrontEndSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configure JWT Authentication
// No detener la app por falta de appsettings; permitir que variables de entorno entren.
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
    throw new InvalidOperationException(
        "JwtSettings.SecretKey no está configurado o es muy corto. " +
        "Configure 'JwtSettings:SecretKey' en appsettings.json o variables de entorno 'JwtSettings__SecretKey'.");

var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization (global) with a configurable enable flag
var authEnabled = builder.Configuration.GetSection("AuthSettings").GetValue("Enabled", true);

if (authEnabled)
{
    builder.Services.AddAuthorization(options =>
    {
        // Require authenticated user by default across the app
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
}
else
{
    // Authorization disabled: still add authorization services to avoid runtime issues
    builder.Services.AddAuthorization();
}

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoIdentificadorService, TipoIdentificadorService>();
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IOrganizacionService, OrganizacionService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IRolUsuarioOrganizacionService, RolUsuarioOrganizacionService>();
builder.Services.AddScoped<ICategoriaActividadService, CategoriaActividadService>();
builder.Services.AddScoped<IActividadService, ActividadService>();
builder.Services.AddScoped<ICoordinadorActividadService, CoordinadorActividadService>();
builder.Services.AddScoped<IHorarioActividadService, HorarioActividadService>();
builder.Services.AddScoped<IParticipanteActividadService, ParticipanteActividadService>();
builder.Services.AddScoped<ICertificacionParticipacionService, CertificacionParticipacionService>();
builder.Services.AddScoped<IUniversidadService, UniversidadService>();
builder.Services.AddScoped<IIntegracionService, IntegracionService>();
builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
builder.Services.AddScoped<IAsistenciaActividadService, AsistenciaActividadService>();
builder.Services.AddScoped<IMailerService, MailerService>();
builder.Services.AddScoped<IEmailQueueService, EmailQueueService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRepository<CorreoPendiente>, GenericRepository<CorreoPendiente>>();
builder.Services.AddHostedService<CertificationGenService>();
builder.Services.AddHostedService<EmailSendService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Community Service API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese el token JWT en formato: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new List<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Register the custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();