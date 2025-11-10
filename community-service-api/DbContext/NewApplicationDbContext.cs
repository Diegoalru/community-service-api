using Microsoft.EntityFrameworkCore;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.DbContext;

public partial class NewApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public NewApplicationDbContext()
    {
    }

    public NewApplicationDbContext(DbContextOptions<NewApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actividad> Actividad { get; set; }

    public virtual DbSet<Canton> Canton { get; set; }

    public virtual DbSet<CategoriaActividad> CategoriaActividad { get; set; }

    public virtual DbSet<CertificadoParticipacion> CertificadoParticipacion { get; set; }

    public virtual DbSet<ControlProceso> ControlProceso { get; set; }

    public virtual DbSet<CoordinadorActividad> CoordinadorActividad { get; set; }

    public virtual DbSet<Correspondencia> Correspondencia { get; set; }

    public virtual DbSet<Distrito> Distrito { get; set; }

    public virtual DbSet<HorarioActividad> HorarioActividad { get; set; }

    public virtual DbSet<LogError> LogError { get; set; }

    public virtual DbSet<Organizacion> Organizacion { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<ParticipanteActividad> ParticipanteActividad { get; set; }

    public virtual DbSet<Perfil> Perfil { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<RolUsuarioOrganizacion> RolUsuarioOrganizacion { get; set; }

    public virtual DbSet<TipoControl> TipoControl { get; set; }

    public virtual DbSet<TipoCorrespondencia> TipoCorrespondencia { get; set; }

    public virtual DbSet<TipoIdentificador> TipoIdentificador { get; set; }

    public virtual DbSet<Ubicacion> Ubicacion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("Name=ConnectionStrings:OracleConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("COMMUNITY")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasKey(e => e.IdActividad);

            entity.ToTable("ACTIVIDAD");

            entity.HasIndex(e => new { e.IdOrganizacion, e.IdActividad }, "UQ_ACT_ORG").IsUnique();

            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.Cupos)
                .HasPrecision(10)
                .HasColumnName("CUPOS");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaFin)
                .HasPrecision(6)
                .HasColumnName("FECHA_FIN");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaInicio)
                .HasPrecision(6)
                .HasColumnName("FECHA_INICIO");
            entity.Property(e => e.Horas)
                .HasPrecision(10)
                .HasColumnName("HORAS");
            entity.Property(e => e.IdCategoria)
                .HasPrecision(10)
                .HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUsuarioCreador)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_CREADOR");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Actividad)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACT_CAT");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.Actividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACT_ORG");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Actividad)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACT_UBIC");

            entity.HasOne(d => d.IdUsuarioCreadorNavigation).WithMany(p => p.Actividad)
                .HasForeignKey(d => d.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACT_USUARIO");
        });

        modelBuilder.Entity<Canton>(entity =>
        {
            entity.HasKey(e => new { e.IdPais, e.IdProvincia, e.IdCanton });

            entity.ToTable("CANTON");

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.Codigo }, "UQ_CANTON_PROV_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.Nombre }, "UQ_CANTON_PROV_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CANTON\".\"NEXTVAL\"")
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.Provincia).WithMany(p => p.Canton)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CANTON_PROVINCIA");
        });

        modelBuilder.Entity<CategoriaActividad>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaActividad).HasName("PK_CAT_ACT");

            entity.ToTable("CATEGORIA_ACTIVIDAD");

            entity.Property(e => e.IdCategoriaActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CATEGORIA_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_CATEGORIA_ACTIVIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<CertificadoParticipacion>(entity =>
        {
            entity.HasKey(e => e.IdCertificacion).HasName("PK_CERT_PART");

            entity.ToTable("CERTIFICADO_PARTICIPACION");

            entity.HasIndex(e => new { e.IdParticipanteActividad, e.IdActividad, e.IdUsuarioVoluntario }, "UQ_CERT_PART").IsUnique();

            entity.Property(e => e.IdCertificacion)
                .HasDefaultValueSql("SYS_GUID()")
                .HasColumnName("ID_CERTIFICACION");
            entity.Property(e => e.DiasTotales)
                .HasPrecision(10)
                .HasColumnName("DIAS_TOTALES");
            entity.Property(e => e.Documento)
                .HasColumnType("BLOB")
                .HasColumnName("DOCUMENTO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaEmision)
                .HasPrecision(6)
                .HasColumnName("FECHA_EMISION");
            entity.Property(e => e.FechaEnvio)
                .HasPrecision(6)
                .HasColumnName("FECHA_ENVIO");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaPrimeraAsistencia)
                .HasPrecision(6)
                .HasColumnName("FECHA_PRIMERA_ASISTENCIA");
            entity.Property(e => e.FechaUltimaAsistencia)
                .HasPrecision(6)
                .HasColumnName("FECHA_ULTIMA_ASISTENCIA");
            entity.Property(e => e.HorasTotales)
                .HasPrecision(10)
                .HasColumnName("HORAS_TOTALES");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdParticipanteActividad)
                .HasPrecision(10)
                .HasColumnName("ID_PARTICIPANTE_ACTIVIDAD");
            entity.Property(e => e.IdUsuarioVoluntario)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_VOLUNTARIO");
            entity.Property(e => e.IntentosEnvio)
                .HasPrecision(10)
                .HasColumnName("INTENTOS_ENVIO");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SITUACION");
            entity.Property(e => e.UltimoErrorEnvio)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("ULTIMO_ERROR_ENVIO");
            entity.Property(e => e.UltimoIntentoEnvio)
                .HasPrecision(6)
                .HasColumnName("ULTIMO_INTENTO_ENVIO");

            entity.HasOne(d => d.IdUsuarioVoluntarioNavigation).WithMany(p => p.CertificadoParticipacion)
                .HasForeignKey(d => d.IdUsuarioVoluntario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_USU");

            entity.HasOne(d => d.Actividad).WithMany(p => p.CertificadoParticipacion)
                .HasPrincipalKey(p => new { p.IdOrganizacion, p.IdActividad })
                .HasForeignKey(d => new { d.IdOrganizacion, d.IdActividad })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_ACT");

            entity.HasOne(d => d.ParticipanteActividad).WithOne(p => p.CertificadoParticipacion)
                .HasForeignKey<CertificadoParticipacion>(d => new { d.IdParticipanteActividad, d.IdActividad, d.IdUsuarioVoluntario })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_PART");
        });

        modelBuilder.Entity<ControlProceso>(entity =>
        {
            entity.HasKey(e => e.IdControl).HasName("PK_CONTROL");

            entity.ToTable("CONTROL_PROCESO_GENERACION_CER");

            entity.Property(e => e.IdControl)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CONTROL_PROCESO\".\"NEXTVAL\"")
                .HasColumnName("ID_CONTROL");
            entity.Property(e => e.ActividadesProcesadas)
                .HasPrecision(10)
                .HasDefaultValueSql("0")
                .HasColumnName("ACTIVIDADES_PROCESADAS");
            entity.Property(e => e.CertificadosGenerados)
                .HasPrecision(10)
                .HasDefaultValueSql("0")
                .HasColumnName("CERTIFICADOS_GENERADOS");
            entity.Property(e => e.CorreosEnviados)
                .HasPrecision(10)
                .HasDefaultValueSql("0")
                .HasColumnName("CORREOS_ENVIADOS");
            entity.Property(e => e.ErrorMensaje)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("ERROR_MENSAJE");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'S'")
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FinEjecucion)
                .HasPrecision(6)
                .HasColumnName("FIN_EJECUCION");
            entity.Property(e => e.InicioEjecucion)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("INICIO_EJECUCION");
            entity.Property(e => e.TipoControl)
                .HasPrecision(10)
                .HasColumnName("TIPO_CONTROL");

            entity.HasOne(d => d.TipoControlNavigation).WithMany(p => p.ControlProceso)
                .HasForeignKey(d => d.TipoControl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONTROL_TIPO_CONTROL");
        });

        modelBuilder.Entity<CoordinadorActividad>(entity =>
        {
            entity.HasKey(e => new { e.IdCoordinadorActividad, e.IdOrganizacion, e.IdActividad }).HasName("PK_COORD_ACT");

            entity.ToTable("COORDINADOR_ACTIVIDAD");

            entity.Property(e => e.IdCoordinadorActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_COORDINADOR_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_COORDINADOR_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdUsuarioCoordinador)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_COORDINADOR");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.CoordinadorActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioCoordinadorNavigation).WithMany(p => p.CoordinadorActividad)
                .HasForeignKey(d => d.IdUsuarioCoordinador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_USU");

            entity.HasOne(d => d.Actividad).WithMany(p => p.CoordinadorActividad)
                .HasPrincipalKey(p => new { p.IdOrganizacion, p.IdActividad })
                .HasForeignKey(d => new { d.IdOrganizacion, d.IdActividad })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_ACT");
        });

        modelBuilder.Entity<Correspondencia>(entity =>
        {
            entity.HasKey(e => e.IdCorrespondencia);

            entity.ToTable("CORRESPONDENCIA");

            entity.Property(e => e.IdCorrespondencia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CORRESPONDENCIA\".\"NEXTVAL\"")
                .HasColumnName("ID_CORRESPONDENCIA");
            entity.Property(e => e.Consentimiento)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONSENTIMIENTO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdTipoCorrespondencia)
                .HasPrecision(10)
                .HasColumnName("ID_TIPO_CORRESPONDENCIA");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VALOR");

            entity.HasOne(d => d.IdTipoCorrespondenciaNavigation).WithMany(p => p.Correspondencia)
                .HasForeignKey(d => d.IdTipoCorrespondencia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORRESP_TIPO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Correspondencia)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORRESPONDENCIA_USUARIO");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.IdDistrito });

            entity.ToTable("DISTRITO");

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.Codigo }, "UQ_DISTRITO_CANTON_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.Nombre }, "UQ_DISTRITO_CANTON_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.IdDistrito)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_DISTRITO\".\"NEXTVAL\"")
                .HasColumnName("ID_DISTRITO");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.Canton).WithMany(p => p.Distrito)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia, d.IdCanton })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DISTRITO_CANTON");
        });

        modelBuilder.Entity<HorarioActividad>(entity =>
        {
            entity.HasKey(e => new { e.IdHorarioActividad, e.IdOrganizacion, e.IdActividad }).HasName("PK_HOR_ACT");

            entity.ToTable("HORARIO_ACTIVIDAD");

            entity.Property(e => e.IdHorarioActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_HORARIO_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_HORARIO_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.Fecha)
                .HasColumnType("DATE")
                .HasColumnName("FECHA");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.HoraFin)
                .HasPrecision(6)
                .HasColumnName("HORA_FIN");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(6)
                .HasColumnName("HORA_INICIO");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.HorarioActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HorarioActividad)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_USU");

            entity.HasOne(d => d.Actividad).WithMany(p => p.HorarioActividad)
                .HasPrincipalKey(p => new { p.IdOrganizacion, p.IdActividad })
                .HasForeignKey(d => new { d.IdOrganizacion, d.IdActividad })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_ACT");
        });

        modelBuilder.Entity<LogError>(entity =>
        {
            entity.HasKey(e => e.IdLogError);

            entity.ToTable("LOG_ERROR");

            entity.Property(e => e.IdLogError)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_LOG_ERROR\".\"NEXTVAL\"")
                .HasColumnName("ID_LOG_ERROR");
            entity.Property(e => e.Backtrace)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("BACKTRACE");
            entity.Property(e => e.Clase)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CLASE");
            entity.Property(e => e.Codigo)
                .HasColumnType("NUMBER")
                .HasColumnName("CODIGO");
            entity.Property(e => e.Fecha)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA");
            entity.Property(e => e.Linea)
                .HasPrecision(10)
                .HasColumnName("LINEA");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("MENSAJE");
            entity.Property(e => e.Programa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PROGRAMA");
            entity.Property(e => e.Stack)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("STACK");
            entity.Property(e => e.Usuario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("USER")
                .HasColumnName("USUARIO");
        });

        modelBuilder.Entity<Organizacion>(entity =>
        {
            entity.HasKey(e => e.IdOrganizacion);

            entity.ToTable("ORGANIZACION");

            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ORGANIZACION\".\"NEXTVAL\"")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUsuarioCreador)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_CREADOR");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.SitioWeb)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SITIO_WEB");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Organizacion)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORG_UBIC");

            entity.HasOne(d => d.IdUsuarioCreadorNavigation).WithMany(p => p.Organizacion)
                .HasForeignKey(d => d.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORG_USUARIO");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais);

            entity.ToTable("PAIS");

            entity.HasIndex(e => e.Nombre, "UQ_PAIS_NOMBRE").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PAIS\".\"NEXTVAL\"")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<ParticipanteActividad>(entity =>
        {
            entity.HasKey(e => new { e.IdParticipanteActividad, e.IdActividad, e.IdUsuarioVoluntario }).HasName("PK_PART_ACT");

            entity.ToTable("PARTICIPANTE_ACTIVIDAD");

            entity.Property(e => e.IdParticipanteActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PARTICIPANTE_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_PARTICIPANTE_ACTIVIDAD");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdUsuarioVoluntario)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_VOLUNTARIO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaInscripcion)
                .HasPrecision(6)
                .HasColumnName("FECHA_INSCRIPCION");
            entity.Property(e => e.FechaRetiro)
                .HasPrecision(6)
                .HasColumnName("FECHA_RETIRO");
            entity.Property(e => e.IdHorarioActividad)
                .HasPrecision(10)
                .HasColumnName("ID_HORARIO_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioVoluntarioNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdUsuarioVoluntario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_USU");

            entity.HasOne(d => d.Actividad).WithMany(p => p.ParticipanteActividad)
                .HasPrincipalKey(p => new { p.IdOrganizacion, p.IdActividad })
                .HasForeignKey(d => new { d.IdOrganizacion, d.IdActividad })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_ACT");

            entity.HasOne(d => d.HorarioActividad).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => new { d.IdHorarioActividad, d.IdOrganizacion, d.IdActividad })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_HOR");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil);

            entity.ToTable("PERFIL");

            entity.HasIndex(e => new { e.IdUsuario, e.IdIdentificador }, "UQ_PERFIL_USU_IDENT").IsUnique();

            entity.Property(e => e.IdPerfil)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PERFIL\".\"NEXTVAL\"")
                .HasColumnName("ID_PERFIL");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO_M");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO_P");
            entity.Property(e => e.Bibliografia)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("BIBLIOGRAFIA");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("DATE")
                .HasColumnName("FECHA_NACIMIENTO");
            entity.Property(e => e.IdIdentificador)
                .HasPrecision(10)
                .HasColumnName("ID_IDENTIFICADOR");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IDENTIFICACION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.IdIdentificadorNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdIdentificador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_IDENT");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_UBIC");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_USUARIO");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => new { e.IdPais, e.IdProvincia });

            entity.ToTable("PROVINCIA");

            entity.HasIndex(e => new { e.IdPais, e.Codigo }, "UQ_PROVINCIA_PAIS_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UQ_PROVINCIA_PAIS_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PROVINCIA\".\"NEXTVAL\"")
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PROVINCIA_PAIS");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ROL\".\"NEXTVAL\"")
                .HasColumnName("ID_ROL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<RolUsuarioOrganizacion>(entity =>
        {
            entity.HasKey(e => new { e.IdRolUsuarioOrganizacion, e.IdOrganizacion, e.IdUsuarioAsignado, e.IdRol }).HasName("PK_RUO");

            entity.ToTable("ROL_USUARIO_ORGANIZACION");

            entity.HasIndex(e => new { e.IdOrganizacion, e.IdUsuarioAsignado, e.IdRol }, "UQ_RUO").IsUnique();

            entity.Property(e => e.IdRolUsuarioOrganizacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ROL_USUARIO_ORGANIZACION\".\"NEXTVAL\"")
                .HasColumnName("ID_ROL_USUARIO_ORGANIZACION");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUsuarioAsignado)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_ASIGNADO");
            entity.Property(e => e.IdRol)
                .HasPrecision(10)
                .HasColumnName("ID_ROL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdUsuarioAdministrador)
                .HasPrecision(10)
                .HasColumnName("ID_USUARIO_ADMINISTRADOR");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.RolUsuarioOrganizacion)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUO_ORG");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolUsuarioOrganizacion)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUO_ROL");

            entity.HasOne(d => d.IdUsuarioAdministradorNavigation).WithMany(p => p.RolUsuarioOrganizacionIdUsuarioAdministradorNavigation)
                .HasForeignKey(d => d.IdUsuarioAdministrador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUO_USU_ADMIN");

            entity.HasOne(d => d.IdUsuarioAsignadoNavigation).WithMany(p => p.RolUsuarioOrganizacionIdUsuarioAsignadoNavigation)
                .HasForeignKey(d => d.IdUsuarioAsignado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUO_USU_ASIG");
        });

        modelBuilder.Entity<TipoControl>(entity =>
        {
            entity.HasKey(e => e.IdTipoControl);

            entity.ToTable("TIPO_CONTROL");

            entity.Property(e => e.IdTipoControl)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_CONTROL_PROCESO\".\"NEXTVAL\"")
                .HasColumnName("ID_TIPO_CONTROL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<TipoCorrespondencia>(entity =>
        {
            entity.HasKey(e => e.IdTipoCorrespondencia).HasName("PK_TIPO_CORRESP");

            entity.ToTable("TIPO_CORRESPONDENCIA");

            entity.Property(e => e.IdTipoCorrespondencia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_CORRESPONDENCIA\".\"NEXTVAL\"")
                .HasColumnName("ID_TIPO_CORRESPONDENCIA");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
        });

        modelBuilder.Entity<TipoIdentificador>(entity =>
        {
            entity.HasKey(e => e.IdIdentificador).HasName("PK_TIPO_IDENT");

            entity.ToTable("TIPO_IDENTIFICADOR");

            entity.Property(e => e.IdIdentificador)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_IDENTIFICADOR\".\"NEXTVAL\"")
                .HasColumnName("ID_IDENTIFICADOR");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.IdUbicacion);

            entity.ToTable("UBICACION");

            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_UBICACION\".\"NEXTVAL\"")
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO_POSTAL");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.IdDistrito)
                .HasPrecision(10)
                .HasColumnName("ID_DISTRITO");
            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.Latitud)
                .HasColumnType("NUMBER(9,6)")
                .HasColumnName("LATITUD");
            entity.Property(e => e.Longitud)
                .HasColumnType("NUMBER(9,6)")
                .HasColumnName("LONGITUD");

            entity.HasOne(d => d.Distrito).WithMany(p => p.Ubicacion)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia, d.IdCanton, d.IdDistrito })
                .HasConstraintName("FK_UBIC_DISTRITO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Username, "UQ_USUARIO_USUARIO").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_USUARIO\".\"NEXTVAL\"")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        modelBuilder.HasSequence("SEQ_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CANTON");
        modelBuilder.HasSequence("SEQ_CATEGORIA_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CONTROL_PROCESO");
        modelBuilder.HasSequence("SEQ_COORDINADOR_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CORRESPONDENCIA");
        modelBuilder.HasSequence("SEQ_DISTRITO");
        modelBuilder.HasSequence("SEQ_HORARIO_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_LOG_ERROR");
        modelBuilder.HasSequence("SEQ_ORGANIZACION");
        modelBuilder.HasSequence("SEQ_PAIS");
        modelBuilder.HasSequence("SEQ_PARTICIPANTE_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_PERFIL");
        modelBuilder.HasSequence("SEQ_PROVINCIA");
        modelBuilder.HasSequence("SEQ_ROL");
        modelBuilder.HasSequence("SEQ_ROL_USUARIO_ORGANIZACION");
        modelBuilder.HasSequence("SEQ_TIPO_CONTROL_PROCESO");
        modelBuilder.HasSequence("SEQ_TIPO_CORRESPONDENCIA");
        modelBuilder.HasSequence("SEQ_TIPO_IDENTIFICADOR");
        modelBuilder.HasSequence("SEQ_UBICACION");
        modelBuilder.HasSequence("SEQ_USUARIO");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
