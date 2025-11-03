using community_service_api.DbContext;
using community_service_api.Repositories;
using community_service_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var env = app.Environment;

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
