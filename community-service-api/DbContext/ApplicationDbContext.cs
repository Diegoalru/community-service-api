using Microsoft.EntityFrameworkCore;
using community_service_api.Models.Entities;

namespace community_service_api.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<TipoIdentificador> TipoIdentificadores => Set<TipoIdentificador>();
    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Perfil> Perfiles => Set<Perfil>();
    public DbSet<Organizacion> Organizaciones => Set<Organizacion>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<RolUsuarioOrganizacion> RolesUsuarioOrganizacion => Set<RolUsuarioOrganizacion>();
    public DbSet<CategoriaActividad> CategoriasActividad => Set<CategoriaActividad>();
    public DbSet<Actividad> Actividades => Set<Actividad>();
    public DbSet<CoordinadorActividad> CoordinadoresActividad => Set<CoordinadorActividad>();
    public DbSet<HorarioActividad> HorariosActividad => Set<HorarioActividad>();
    public DbSet<ParticipanteActividad> ParticipantesActividad => Set<ParticipanteActividad>();
    public DbSet<CertificacionParticipacion> CertificacionesParticipacion => Set<CertificacionParticipacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<TipoIdentificador>()
            .Property(t => t.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Pais>()
            .Property(p => p.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Perfil>()
            .Property(p => p.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Organizacion>()
            .Property(o => o.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Rol>()
            .Property(r => r.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<RolUsuarioOrganizacion>()
            .Property(r => r.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<CategoriaActividad>()
            .Property(c => c.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Actividad>()
            .Property(a => a.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<Actividad>()
            .Property(a => a.Situacion)
            .HasMaxLength(1);

        modelBuilder.Entity<CoordinadorActividad>()
            .Property(c => c.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<HorarioActividad>()
            .Property(h => h.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<ParticipanteActividad>()
            .Property(p => p.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<ParticipanteActividad>()
            .Property(p => p.Situacion)
            .HasMaxLength(1);

        modelBuilder.Entity<CertificacionParticipacion>()
            .Property(c => c.Estado)
            .HasMaxLength(1);

        modelBuilder.Entity<CertificacionParticipacion>()
            .Property(c => c.Situacion)
            .HasMaxLength(1);
    }
}
