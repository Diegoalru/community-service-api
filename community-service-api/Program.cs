using community_service_api.DbContext;
using community_service_api.HostedServices;
using community_service_api.Middleware;
using community_service_api.Models;
using community_service_api.Repositories;
using community_service_api.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
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

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

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
builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
builder.Services.AddHostedService<CertificationGenService>();
builder.Services.AddHostedService<EmailSendService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var env = app.Environment;

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();