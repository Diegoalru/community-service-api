using community_service_api.DbContext;
using community_service_api.HostedServices;
using community_service_api.Middleware;
using community_service_api.Repositories;
using community_service_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NewApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

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

var app = builder.Build();

// Register the custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

var env = app.Environment;

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
