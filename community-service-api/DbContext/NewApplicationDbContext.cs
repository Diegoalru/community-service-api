using Microsoft.EntityFrameworkCore;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.DbContext;

public partial class NewApplicationDbContext(DbContextOptions<NewApplicationDbContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public virtual DbSet<Actividad> Actividad { get; set; }
    public virtual DbSet<AsistenciaActividad> AsistenciaActividad { get; set; }
    public virtual DbSet<Canton> Canton { get; set; }

    public virtual DbSet<CategoriaActividad> CategoriaActividad { get; set; }

    public virtual DbSet<CertificadoParticipacion> CertificadoParticipacion { get; set; }

    public virtual DbSet<ControlProcesoGeneracionCer> ControlProcesoGeneracionCer { get; set; }

    public virtual DbSet<CoordinadorActividad> CoordinadorActividad { get; set; }

    public virtual DbSet<CorreoPendiente> CorreoPendiente { get; set; }

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

    public virtual DbSet<Universidad> Universidad { get; set; }

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

            entity.ToTable("ACTIVIDAD", tb => tb.HasComment("Tabla que almacena las actividades registradas en el sistema"));

            entity.HasIndex(e => new { e.IdOrganizacion, e.IdActividad }, "UQ_ACT_ORG").IsUnique();

            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ACTIVIDAD\".\"NEXTVAL\"")
                .HasComment("Identificador único de la actividad")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.Cupos)
                .HasPrecision(10)
                .HasComment("Número de cupos disponibles para la actividad")
                .HasColumnName("CUPOS");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("Descripción de la actividad")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                   ")
                .IsFixedLength()
                .HasComment("Estado de la actividad: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP        ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaFin)
                .HasPrecision(6)
                .HasComment("Fecha de finalización de la actividad")
                .HasColumnName("FECHA_FIN");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaInicio)
                .HasPrecision(6)
                .HasComment("Fecha de inicio de la actividad")
                .HasColumnName("FECHA_INICIO");
            entity.Property(e => e.Horas)
                .HasPrecision(10)
                .HasComment("Número de horas de la actividad")
                .HasColumnName("HORAS");
            entity.Property(e => e.IdCategoria)
                .HasPrecision(10)
                .HasComment("Identificador de la categoría de la actividad")
                .HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización que crea la actividad")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasComment("Identificador de la ubicación de la actividad")
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUsuarioCreador)
                .HasPrecision(10)
                .HasComment("Identificador del usuario que crea la actividad")
                .HasColumnName("ID_USUARIO_CREADOR");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre de la actividad")
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Situación de la actividad: Iniciada (I), Publicada (P), Cancelada (C), Finalizada (F), Anulada (A)")
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

            entity.ToTable("CANTON", tb => tb.HasComment("Tabla que almacena los cantones de cada provincia"));

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.Codigo }, "UQ_CANTON_PROV_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.Nombre }, "UQ_CANTON_PROV_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasComment("Identificador del país al que pertenece el cantón")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasComment("Identificador de la provincia a la que pertenece el cantón")
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CANTON\".\"NEXTVAL\"")
                .HasComment("Identificador único del cantón dentro de la provincia")
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("Código único del cantón dentro de la provincia")
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                ")
                .IsFixedLength()
                .HasComment("Estado del cantón: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP     ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del cantón")
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.Provincia).WithMany(p => p.Canton)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CANTON_PROVINCIA");
        });

        modelBuilder.Entity<CategoriaActividad>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaActividad).HasName("PK_CAT_ACT");

            entity.ToTable("CATEGORIA_ACTIVIDAD", tb => tb.HasComment("Tabla que almacena las categorías de las actividades"));

            entity.HasIndex(e => e.Nombre, "UQ_CAT_ACT_NOMBRE").IsUnique();

            entity.Property(e => e.IdCategoriaActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CATEGORIA_ACTIVIDAD\".\"NEXTVAL\"")
                .HasColumnName("ID_CATEGORIA_ACTIVIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                             ")
                .IsFixedLength()
                .HasComment("Estado de la categoría de actividad: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                  ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre de la categoría de actividad")
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<CertificadoParticipacion>(entity =>
        {
            entity.HasKey(e => e.IdCertificacion).HasName("PK_CERT_PART");

            entity.ToTable("CERTIFICADO_PARTICIPACION", tb => tb.HasComment("Tabla que almacena los certificados de participación de los voluntarios"));

            entity.HasIndex(e => e.IdParticipanteActividad, "UQ_CERT_PART").IsUnique();

            entity.Property(e => e.IdCertificacion)
                .HasDefaultValueSql("SYS_GUID()     ")
                .HasComment("Identificador único del certificado de participación")
                .HasColumnName("ID_CERTIFICACION");
            entity.Property(e => e.DiasTotales)
                .HasPrecision(10)
                .HasComment("Número total de días certificados")
                .HasColumnName("DIAS_TOTALES");
            entity.Property(e => e.Documento)
                .HasComment("Documento del certificado en formato BLOB")
                .HasColumnType("BLOB")
                .HasColumnName("DOCUMENTO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'            ")
                .IsFixedLength()
                .HasComment("Estado del certificado: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaEmision)
                .HasPrecision(6)
                .HasComment("Fecha de emisión del certificado")
                .HasColumnName("FECHA_EMISION");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaPrimeraAsistencia)
                .HasPrecision(6)
                .HasComment("Fecha de la primera asistencia del voluntario")
                .HasColumnName("FECHA_PRIMERA_ASISTENCIA");
            entity.Property(e => e.FechaUltimaAsistencia)
                .HasPrecision(6)
                .HasComment("Fecha de la última asistencia del voluntario")
                .HasColumnName("FECHA_ULTIMA_ASISTENCIA");
            entity.Property(e => e.HorasTotales)
                .HasPrecision(10)
                .HasComment("Número total de horas certificadas")
                .HasColumnName("HORAS_TOTALES");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasComment("Identificador de la actividad")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización que emite el certificado")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdParticipanteActividad)
                .HasPrecision(10)
                .HasComment("Identificador del participante en la actividad")
                .HasColumnName("ID_PARTICIPANTE_ACTIVIDAD");
            entity.Property(e => e.IdUsuarioVoluntario)
                .HasPrecision(10)
                .HasComment("Identificador del usuario voluntario al que se le emite el certificado")
                .HasColumnName("ID_USUARIO_VOLUNTARIO");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Observaciones adicionales sobre el certificado")
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Situación del certificado: Pendiente (P), Generado (G), Emitido (E), Completado (C), Anulado (A)")
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.CertificadoParticipacion)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_ACT");

            entity.HasOne(d => d.IdParticipanteActividadNavigation).WithOne(p => p.CertificadoParticipacion)
                .HasForeignKey<CertificadoParticipacion>(d => d.IdParticipanteActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_PART");

            entity.HasOne(d => d.IdUsuarioVoluntarioNavigation).WithMany(p => p.CertificadoParticipacion)
                .HasForeignKey(d => d.IdUsuarioVoluntario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERT_PART_USU");
        });

        modelBuilder.Entity<ControlProcesoGeneracionCer>(entity =>
        {
            entity.HasKey(e => e.IdControl).HasName("PK_CONTROL");

            entity.ToTable("CONTROL_PROCESO_GENERACION_CER", tb => tb.HasComment("Tabla que almacena el control de los procesos de generación de certificados y envío de correos"));

            entity.Property(e => e.IdControl)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CONTROL_PROCESO\".\"NEXTVAL\"")
                .HasComment("Identificador único del control de proceso")
                .HasColumnName("ID_CONTROL");
            entity.Property(e => e.ActividadesProcesadas)
                .HasPrecision(10)
                .HasDefaultValueSql("0                           ")
                .HasComment("Número de actividades procesadas en el proceso")
                .HasColumnName("ACTIVIDADES_PROCESADAS");
            entity.Property(e => e.CertificadosGenerados)
                .HasPrecision(10)
                .HasDefaultValueSql("0                           ")
                .HasComment("Número de certificados generados en el proceso")
                .HasColumnName("CERTIFICADOS_GENERADOS");
            entity.Property(e => e.CorreosEnviados)
                .HasPrecision(10)
                .HasDefaultValueSql("0                           ")
                .HasComment("Número de correos enviados en el proceso")
                .HasColumnName("CORREOS_ENVIADOS");
            entity.Property(e => e.ErrorMensaje)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Mensaje de error en caso de que el proceso falle")
                .HasColumnName("ERROR_MENSAJE");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'S'                         ")
                .IsFixedLength()
                .HasComment("Estado del proceso: Espera (S), Completado (C), Error (E), Procesando (P)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FinEjecucion)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP              ")
                .HasComment("Fecha y hora de finalización de la ejecución del proceso")
                .HasColumnName("FIN_EJECUCION");
            entity.Property(e => e.InicioEjecucion)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP              ")
                .HasComment("Fecha y hora de inicio de la ejecución del proceso")
                .HasColumnName("INICIO_EJECUCION");
            entity.Property(e => e.TipoControl)
                .HasPrecision(10)
                .HasComment("Identificador del tipo de control de proceso")
                .HasColumnName("TIPO_CONTROL");

            entity.HasOne(d => d.TipoControlNavigation).WithMany(p => p.ControlProcesoGeneracionCer)
                .HasForeignKey(d => d.TipoControl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONTROL_TIPO_CONTROL");
        });

        modelBuilder.Entity<CoordinadorActividad>(entity =>
        {
            entity.HasKey(e => e.IdCoordinadorActividad).HasName("PK_COORD_ACT");

            entity.ToTable("COORDINADOR_ACTIVIDAD", tb => tb.HasComment("Tabla que relaciona los coordinadores con las actividades"));

            entity.HasIndex(e => new { e.IdActividad, e.IdUsuarioCoordinador }, "UQ_COORD_ACT_LOGICA").IsUnique();

            entity.Property(e => e.IdCoordinadorActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_COORDINADOR_ACTIVIDAD\".\"NEXTVAL\"")
                .HasComment("Identificador del registro")
                .HasColumnName("ID_COORDINADOR_ACTIVIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                               ")
                .IsFixedLength()
                .HasComment("Estado del registro: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                    ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasComment("Identificador de la actividad")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUsuarioCoordinador)
                .HasPrecision(10)
                .HasComment("Identificador del usuario coordinador")
                .HasColumnName("ID_USUARIO_COORDINADOR");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.CoordinadorActividad)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_ACT");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.CoordinadorActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioCoordinadorNavigation).WithMany(p => p.CoordinadorActividad)
                .HasForeignKey(d => d.IdUsuarioCoordinador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COORD_ACT_USU");
        });

        modelBuilder.Entity<CorreoPendiente>(entity =>
        {
            entity.HasKey(e => e.IdCorreoPendiente);

            entity.ToTable("CORREO_PENDIENTE", tb => tb.HasComment("Tabla para encolar correos electrónicos a ser enviados por un proceso en segundo plano."));

            entity.Property(e => e.IdCorreoPendiente)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CORREO_PENDIENTE\".\"NEXTVAL\"")
                .HasComment("Identificador único del correo pendiente.")
                .HasColumnName("ID_CORREO_PENDIENTE");
            entity.Property(e => e.Adjunto)
                .HasComment("Contenido binario del archivo adjunto (e.g., PDF del certificado).")
                .HasColumnType("BLOB")
                .HasColumnName("ADJUNTO");
            entity.Property(e => e.Asunto)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Asunto del correo electrónico.")
                .HasColumnName("ASUNTO");
            entity.Property(e => e.Cuerpo)
                .HasComment("Cuerpo del correo electrónico, puede contener HTML.")
                .HasColumnType("CLOB")
                .HasColumnName("CUERPO");
            entity.Property(e => e.Destinatario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Dirección de correo electrónico del destinatario.")
                .HasColumnName("DESTINATARIO");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'P'                      ")
                .HasComment("Estado del envío del correo: Pendiente, Enviado, Fallido.")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("SYSDATE                  ")
                .HasComment("Fecha en que se encoló el correo.")
                .HasColumnType("DATE")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.FechaUltimoIntento)
                .HasComment("Fecha del último intento de envío.")
                .HasColumnType("DATE")
                .HasColumnName("FECHA_ULTIMO_INTENTO");
            entity.Property(e => e.IdCertificacion)
                .HasComment("ID del certificado que originó este correo, si aplica.")
                .HasColumnName("ID_CERTIFICACION");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasComment("ID del usuario al que se le envía este correo.")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Intentos)
                .HasPrecision(10)
                .HasDefaultValueSql("0                        ")
                .HasComment("Número de intentos de envío.")
                .HasColumnName("INTENTOS");
            entity.Property(e => e.MensajeError)
                .IsUnicode(false)
                .HasComment("Mensaje de error si el último intento de envío falló.")
                .HasColumnName("MENSAJE_ERROR");
            entity.Property(e => e.NombreAdjunto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Nombre del archivo adjunto.")
                .HasColumnName("NOMBRE_ADJUNTO");

            entity.HasOne(d => d.IdCertificacionNavigation).WithMany(p => p.CorreoPendiente)
                .HasForeignKey(d => d.IdCertificacion)
                .HasConstraintName("FK_CORREO_CERTIFICADO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CorreoPendiente)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORREO_USUARIO");
        });

        modelBuilder.Entity<Correspondencia>(entity =>
        {
            entity.HasKey(e => e.IdCorrespondencia);

            entity.ToTable("CORRESPONDENCIA", tb => tb.HasComment("Tabla de correspondencia de datos para usuarios y organizaciones"));

            entity.Property(e => e.IdCorrespondencia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_CORRESPONDENCIA\".\"NEXTVAL\"")
                .HasColumnName("ID_CORRESPONDENCIA");
            entity.Property(e => e.Consentimiento)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Consentimiento del usuario")
                .HasColumnName("CONSENTIMIENTO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                         ")
                .IsFixedLength()
                .HasComment("Estado de la correspondencia: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP              ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización (si la correspondencia es para una organización)")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdTipoCorrespondencia)
                .HasPrecision(10)
                .HasComment("Tipo de correspondencia")
                .HasColumnName("ID_TIPO_CORRESPONDENCIA");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasComment("Identificador del usuario (si la correspondencia es para un usuario)")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Valor de la correspondencia")
                .HasColumnName("VALOR");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.Correspondencia)
                .HasForeignKey(d => d.IdOrganizacion)
                .HasConstraintName("FK_CORR_ORGANIZACION");

            entity.HasOne(d => d.IdTipoCorrespondenciaNavigation).WithMany(p => p.Correspondencia)
                .HasForeignKey(d => d.IdTipoCorrespondencia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORRESP_TIPO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Correspondencia)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_CORR_USUARIO");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.IdDistrito });

            entity.ToTable("DISTRITO", tb => tb.HasComment("Tabla que almacena los distritos de cada cantón"));

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.Codigo }, "UQ_DISTRITO_CANTON_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.IdProvincia, e.IdCanton, e.Nombre }, "UQ_DISTRITO_CANTON_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasComment("Identificador del país al que pertenece el distrito")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasComment("Identificador de la provincia a la que pertenece el distrito")
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasComment("Identificador del cantón al que pertenece el distrito")
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.IdDistrito)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_DISTRITO\".\"NEXTVAL\"")
                .HasComment("Identificador único del distrito dentro del cantón")
                .HasColumnName("ID_DISTRITO");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("Código único del distrito dentro del cantón")
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                  ")
                .IsFixedLength()
                .HasComment("Estado del distrito: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP       ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del distrito")
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.Canton).WithMany(p => p.Distrito)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia, d.IdCanton })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DISTRITO_CANTON");
        });

        modelBuilder.Entity<HorarioActividad>(entity =>
        {
            entity.HasKey(e => e.IdHorarioActividad).HasName("PK_HOR_ACT");

            entity.ToTable("HORARIO_ACTIVIDAD", tb => tb.HasComment("Tabla que almacena los horarios de las actividades"));

            entity.HasIndex(e => new { e.IdActividad, e.Fecha, e.HoraInicio }, "UQ_HOR_ACT_LOGICA").IsUnique();

            entity.Property(e => e.IdHorarioActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_HORARIO_ACTIVIDAD\".\"NEXTVAL\"")
                .HasComment("Identificador único del horario de la actividad")
                .HasColumnName("ID_HORARIO_ACTIVIDAD");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Descripción del horario de la actividad")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                           ")
                .IsFixedLength()
                .HasComment("Estado del registro: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.Fecha)
                .HasComment("Fecha del horario de la actividad")
                .HasColumnType("DATE")
                .HasColumnName("FECHA");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.HoraFin)
                .HasPrecision(6)
                .HasComment("Hora de fin del horario de la actividad")
                .HasColumnName("HORA_FIN");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(6)
                .HasComment("Hora de inicio del horario de la actividad")
                .HasColumnName("HORA_INICIO");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasComment("Identificador de la actividad")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización que crea el horario")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasComment("Identificador del usuario que crea el horario")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Situación del horario de la actividad: Iniciada (I), Publicada (P), Cancelada (C), Finalizada (F), Anulada (A)")
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.HorarioActividad)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_ACT");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.HorarioActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HorarioActividad)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOR_ACT_USU");
        });

        modelBuilder.Entity<LogError>(entity =>
        {
            entity.HasKey(e => e.IdLogError);

            entity.ToTable("LOG_ERROR", tb => tb.HasComment("Tabla que almacena los errores producidos en el sistema"));

            entity.Property(e => e.IdLogError)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_LOG_ERROR\".\"NEXTVAL\"")
                .HasComment("Identificador único del error registrado")
                .HasColumnName("ID_LOG_ERROR");
            entity.Property(e => e.Backtrace)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Rastreo de llamadas del error formateada")
                .HasColumnName("BACKTRACE");
            entity.Property(e => e.Clase)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Clase del error: NEGOCIO, TECNICO, VALIDACION")
                .HasColumnName("CLASE");
            entity.Property(e => e.Codigo)
                .HasComment("Código del error: SQLCODE o código de negocio")
                .HasColumnType("NUMBER")
                .HasColumnName("CODIGO");
            entity.Property(e => e.Fecha)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP        ")
                .HasComment("Fecha y hora en que ocurrió el error")
                .HasColumnName("FECHA");
            entity.Property(e => e.Linea)
                .HasPrecision(10)
                .HasComment("Número de línea del código donde ocurrió el error")
                .HasColumnName("LINEA");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Mensaje descriptivo del error")
                .HasColumnName("MENSAJE");
            entity.Property(e => e.Programa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del programa o procedimiento donde ocurrió el error")
                .HasColumnName("PROGRAMA");
            entity.Property(e => e.Stack)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Pila de llamadas del error formateada")
                .HasColumnName("STACK");
            entity.Property(e => e.Usuario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("USER                  ")
                .HasComment("Usuario que experimentó el error")
                .HasColumnName("USUARIO");
        });

        modelBuilder.Entity<Organizacion>(entity =>
        {
            entity.HasKey(e => e.IdOrganizacion);

            entity.ToTable("ORGANIZACION", tb => tb.HasComment("Tabla que almacena las organizaciones registradas en el sistema"));

            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ORGANIZACION\".\"NEXTVAL\"")
                .HasComment("Identificador único de la organización")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Descripción de la organización")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                      ")
                .IsFixedLength()
                .HasComment("Estado de la organización: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP           ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasComment("Identificador de la ubicación de la organización")
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUniversidad)
                .HasPrecision(10)
                .HasColumnName("ID_UNIVERSIDAD");
            entity.Property(e => e.IdUsuarioCreador)
                .HasPrecision(10)
                .HasComment("Identificador del usuario que creó la organización")
                .HasColumnName("ID_USUARIO_CREADOR");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Nombre de la organización")
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Organizacion)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORG_UBIC");

            entity.HasOne(d => d.IdUniversidadNavigation).WithMany(p => p.Organizacion)
                .HasForeignKey(d => d.IdUniversidad)
                .HasConstraintName("FK_ORG_UNIVERSIDAD");

            entity.HasOne(d => d.IdUsuarioCreadorNavigation).WithMany(p => p.Organizacion)
                .HasForeignKey(d => d.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORG_USUARIO");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais);

            entity.ToTable("PAIS", tb => tb.HasComment("Tabla que almacena los países"));

            entity.HasIndex(e => e.Nombre, "UQ_PAIS_NOMBRE").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PAIS\".\"NEXTVAL\"")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'              ")
                .IsFixedLength()
                .HasComment("Estado de la correspondencia: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP   ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del país")
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<ParticipanteActividad>(entity =>
        {
            entity.HasKey(e => e.IdParticipanteActividad).HasName("PK_PART_ACT");

            entity.ToTable("PARTICIPANTE_ACTIVIDAD", tb => tb.HasComment("Tabla que relaciona los participantes con las actividades"));

            entity.HasIndex(e => new { e.IdActividad, e.IdUsuarioVoluntario }, "UQ_PART_ACT_LOGICA").IsUnique();

            entity.Property(e => e.IdParticipanteActividad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PARTICIPANTE_ACTIVIDAD\".\"NEXTVAL\"")
                .HasComment("Identificador del registro")
                .HasColumnName("ID_PARTICIPANTE_ACTIVIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                                ")
                .IsFixedLength()
                .HasComment("Estado del registro: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                     ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaInscripcion)
                .HasPrecision(6)
                .HasComment("Fecha de inscripción del participante en la actividad")
                .HasColumnName("FECHA_INSCRIPCION");
            entity.Property(e => e.FechaRetiro)
                .HasPrecision(6)
                .HasComment("Fecha de retiro del participante de la actividad")
                .HasColumnName("FECHA_RETIRO");
            entity.Property(e => e.IdActividad)
                .HasPrecision(10)
                .HasComment("Identificador de la actividad")
                .HasColumnName("ID_ACTIVIDAD");
            entity.Property(e => e.IdHorarioActividad)
                .HasPrecision(10)
                .HasComment("Identificador del horario de la actividad")
                .HasColumnName("ID_HORARIO_ACTIVIDAD");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdUsuarioVoluntario)
                .HasPrecision(10)
                .HasComment("Identificador del usuario voluntario participante")
                .HasColumnName("ID_USUARIO_VOLUNTARIO");
            entity.Property(e => e.Situacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Situación del participante en la actividad: Inicial (I), Activo (A), Retirado (R), Cancelado (C), Finalizado (F)")
                .HasColumnName("SITUACION");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_ACT");

            entity.HasOne(d => d.IdHorarioActividadNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdHorarioActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_HOR");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_ORG");

            entity.HasOne(d => d.IdUsuarioVoluntarioNavigation).WithMany(p => p.ParticipanteActividad)
                .HasForeignKey(d => d.IdUsuarioVoluntario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_ACT_USU");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil);

            entity.ToTable("PERFIL", tb => tb.HasComment("Tabla que almacena los perfiles de los usuarios"));

            entity.HasIndex(e => new { e.IdUsuario, e.IdIdentificador }, "UQ_PERFIL_USU_IDENT").IsUnique();

            entity.Property(e => e.IdPerfil)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PERFIL\".\"NEXTVAL\"")
                .HasComment("Identificador único del perfil")
                .HasColumnName("ID_PERFIL");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Segundo apellido de la persona")
                .HasColumnName("APELLIDO_M");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Primer apellido de la persona")
                .HasColumnName("APELLIDO_P");
            entity.Property(e => e.Bibliografia)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Biografía o descripción del usuario")
                .HasColumnName("BIBLIOGRAFIA");
            entity.Property(e => e.Carrera)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Carrera de estudio de la persona")
                .HasColumnName("CARRERA");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                ")
                .IsFixedLength()
                .HasComment("Estado del perfil: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP     ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.FechaNacimiento)
                .HasComment("Fecha de nacimiento de la persona")
                .HasColumnType("DATE")
                .HasColumnName("FECHA_NACIMIENTO");
            entity.Property(e => e.IdIdentificador)
                .HasPrecision(10)
                .HasComment("Identificador del tipo de identificador asociado al perfil")
                .HasColumnName("ID_IDENTIFICADOR");
            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasComment("Identificador de la ubicación asociada al perfil")
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.IdUniversidad)
                .HasPrecision(10)
                .HasComment("Identificador de la universidad asociada al perfil")
                .HasColumnName("ID_UNIVERSIDAD");
            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasComment("Identificador del usuario asociado al perfil")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Valor del identificador del usuario")
                .HasColumnName("IDENTIFICACION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre de la persona")
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.IdIdentificadorNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdIdentificador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_IDENT");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_UBIC");

            entity.HasOne(d => d.IdUniversidadNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdUniversidad)
                .HasConstraintName("FK_PERFIL_UNIVERSIDAD");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Perfil)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERFIL_USUARIO");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => new { e.IdPais, e.IdProvincia });

            entity.ToTable("PROVINCIA", tb => tb.HasComment("Tabla que almacena las provincias de cada país"));

            entity.HasIndex(e => new { e.IdPais, e.Codigo }, "UQ_PROVINCIA_PAIS_COD").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UQ_PROVINCIA_PAIS_NOM").IsUnique();

            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasComment("Identificador del país al que pertenece la provincia")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_PROVINCIA\".\"NEXTVAL\"")
                .HasComment("Identificador único de la provincia dentro del país")
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("Código único de la provincia dentro del país")
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                   ")
                .IsFixedLength()
                .HasComment("Estado de la provincia: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP        ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre de la provincia")
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PROVINCIA_PAIS");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("ROL", tb => tb.HasComment("Roles de los usuarios"));

            entity.HasIndex(e => e.Nombre, "UQ_ROL_NOMBRE").IsUnique();

            entity.Property(e => e.IdRol)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ROL\".\"NEXTVAL\"")
                .HasComment("Identificador del rol")
                .HasColumnName("ID_ROL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'             ")
                .IsFixedLength()
                .HasComment("Estado del rol: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP  ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del rol (ADMINISTRADOR, COORDINADOR, ASISTENTE)")
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<RolUsuarioOrganizacion>(entity =>
        {
            entity.HasKey(e => e.IdRolUsuarioOrganizacion).HasName("PK_RUO");

            entity.ToTable("ROL_USUARIO_ORGANIZACION", tb => tb.HasComment("Tabla que relaciona los usuarios con sus organizaciones y sus roles"));

            entity.HasIndex(e => new { e.IdOrganizacion, e.IdUsuarioAsignado, e.IdRol }, "UQ_RUO_COMPUESTA").IsUnique();

            entity.Property(e => e.IdRolUsuarioOrganizacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_ROL_USUARIO_ORGANIZACION\".\"NEXTVAL\"")
                .HasComment("Identificador del registro")
                .HasColumnName("ID_ROL_USUARIO_ORGANIZACION");
            entity.Property(e => e.EsActivo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A' ")
                .IsFixedLength()
                .HasComment("Indica si la asignación del rol está activa (A) o inactiva (I).")
                .HasColumnName("ES_ACTIVO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                                  ")
                .IsFixedLength()
                .HasComment("Estado del registro: Activo (A) o Inactivo (I) para borrado lógico.")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                       ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdOrganizacion)
                .HasPrecision(10)
                .HasComment("Identificador de la organización")
                .HasColumnName("ID_ORGANIZACION");
            entity.Property(e => e.IdRol)
                .HasPrecision(10)
                .HasComment("Identificador del rol")
                .HasColumnName("ID_ROL");
            entity.Property(e => e.IdUsuarioAdministrador)
                .HasPrecision(10)
                .HasComment("Identificador del usuario administrador")
                .HasColumnName("ID_USUARIO_ADMINISTRADOR");
            entity.Property(e => e.IdUsuarioAsignado)
                .HasPrecision(10)
                .HasComment("Identificador del usuario asignado")
                .HasColumnName("ID_USUARIO_ASIGNADO");

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

            entity.ToTable("TIPO_CONTROL", tb => tb.HasComment("Tabla que almacena los tipos de control de procesos"));

            entity.Property(e => e.IdTipoControl)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_CONTROL_PROCESO\".\"NEXTVAL\"")
                .HasComment("Identificador único del tipo de control de proceso")
                .HasColumnName("ID_TIPO_CONTROL");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                              ")
                .IsFixedLength()
                .HasComment("Estado del tipo de control de proceso: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del tipo de control de proceso (e.g., Generación de Certificados, envío de correos)")
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<TipoCorrespondencia>(entity =>
        {
            entity.HasKey(e => e.IdTipoCorrespondencia).HasName("PK_TIPO_CORRESP");

            entity.ToTable("TIPO_CORRESPONDENCIA", tb => tb.HasComment("Tipos de corresponsalidad"));

            entity.Property(e => e.IdTipoCorrespondencia)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_CORRESPONDENCIA\".\"NEXTVAL\"")
                .HasColumnName("ID_TIPO_CORRESPONDENCIA");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Descripción del tipo de corresponsalidad (e.g., Email, Celular, Oficina)")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                              ")
                .IsFixedLength()
                .HasComment("Estado del tipo de corresponsalidad: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                   ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
        });

        modelBuilder.Entity<TipoIdentificador>(entity =>
        {
            entity.HasKey(e => e.IdIdentificador).HasName("PK_TIPO_IDENT");

            entity.ToTable("TIPO_IDENTIFICADOR", tb => tb.HasComment("Tipos de identificadores de usuarios"));

            entity.Property(e => e.IdIdentificador)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_TIPO_IDENTIFICADOR\".\"NEXTVAL\"")
                .HasColumnName("ID_IDENTIFICADOR");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Descripción del tipo de identificador (e.g., Cédula Física, Cédula Jurídica, DIMEX, Pasaporte)")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                            ")
                .IsFixedLength()
                .HasComment("Estado de la correspondencia: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP                 ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.IdUbicacion);

            entity.ToTable("UBICACION", tb => tb.HasComment("Tabla que almacena las ubicaciones de los usuarios"));

            entity.Property(e => e.IdUbicacion)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_UBICACION\".\"NEXTVAL\"")
                .HasComment("Identificador único de la ubicación")
                .HasColumnName("ID_UBICACION");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Código postal de la ubicación")
                .HasColumnName("CODIGO_POSTAL");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Dirección detallada de la ubicación")
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                   ")
                .IsFixedLength()
                .HasComment("Estado de la ubicación: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP        ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IdCanton)
                .HasPrecision(10)
                .HasComment("Identificador del cantón de la ubicación")
                .HasColumnName("ID_CANTON");
            entity.Property(e => e.IdDistrito)
                .HasPrecision(10)
                .HasComment("Identificador del distrito de la ubicación")
                .HasColumnName("ID_DISTRITO");
            entity.Property(e => e.IdPais)
                .HasPrecision(10)
                .HasComment("Identificador del país de la ubicación")
                .HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia)
                .HasPrecision(10)
                .HasComment("Identificador de la provincia de la ubicación")
                .HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.Latitud)
                .HasComment("Latitud geográfica de la ubicación")
                .HasColumnType("NUMBER(9,6)")
                .HasColumnName("LATITUD");
            entity.Property(e => e.Longitud)
                .HasComment("Longitud geográfica de la ubicación")
                .HasColumnType("NUMBER(9,6)")
                .HasColumnName("LONGITUD");

            entity.HasOne(d => d.Distrito).WithMany(p => p.Ubicacion)
                .HasForeignKey(d => new { d.IdPais, d.IdProvincia, d.IdCanton, d.IdDistrito })
                .HasConstraintName("FK_UBIC_DISTRITO");
        });

        modelBuilder.Entity<Universidad>(entity =>
        {
            entity.HasKey(e => e.IdUniversidad);

            entity.ToTable("UNIVERSIDAD", tb => tb.HasComment("Tabla que almacena las universidades"));

            entity.Property(e => e.IdUniversidad)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_UNIVERSIDAD\".\"NEXTVAL\"")
                .HasComment("Identificador único de la universidad")
                .HasColumnName("ID_UNIVERSIDAD");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'A'                     ")
                .IsFixedLength()
                .HasComment("Estado de la universidad: Activa (A) o Inactiva (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP          ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Nombre de la universidad")
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Siglas)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SIGLAS");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIO", tb => tb.HasComment("Tabla que almacena los usuarios del sistema"));

            entity.HasIndex(e => e.Username, "UQ_USUARIO_USUARIO").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasPrecision(10)
                .HasDefaultValueSql("\"COMMUNITY\".\"SEQ_USUARIO\".\"NEXTVAL\"")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'I'")
                .IsFixedLength()
                .HasComment("Estado del usuario: Activo (A) o Inactivo (I)")
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaDesbloqueo)
                .HasPrecision(6)
                .HasComment("Fecha en la que el usuario fue desbloqueado")
                .HasColumnName("FECHA_DESBLOQUEO");
            entity.Property(e => e.FechaDesde)
                .HasPrecision(6)
                .HasDefaultValueSql("LOCALTIMESTAMP      ")
                .HasComment("Fecha desde la cual el registro es accesible")
                .HasColumnName("FECHA_DESDE");
            entity.Property(e => e.FechaHasta)
                .HasPrecision(6)
                .HasComment("Fecha hasta la cual el registro es accesible")
                .HasColumnName("FECHA_HASTA");
            entity.Property(e => e.IntentosFallidos)
                .HasPrecision(10)
                .HasDefaultValueSql("0 ")
                .HasColumnName("INTENTOS_FALLIDOS");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Almacena el hash de la contraseña del usuario")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Restablecer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'N'                 ")
                .IsFixedLength()
                .HasComment("Indica si el usuario debe restablecer su contraseña en el próximo inicio de sesión (S/N)")
                .HasColumnName("RESTABLECER");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Token de autenticación para el usuario")
                .HasColumnName("TOKEN");
            entity.Property(e => e.TokenEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'V' ")
                .IsFixedLength()
                .HasComment("Estado del token de autenticación (P: Pendiente, V: Válido, E: Expirado, I: Inválido)")
                .HasColumnName("TOKEN_ESTADO");
            entity.Property(e => e.TokenExpiracion)
                .HasPrecision(6)
                .HasComment("Fecha de expiración del token de autenticación")
                .HasColumnName("TOKEN_EXPIRACION");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Nombre de usuario único para autenticación")
                .HasColumnName("USERNAME");
        });
        modelBuilder.HasSequence("DBTOOLS$EXECUTION_HISTORY_SEQ");
        modelBuilder.HasSequence("SEQ_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CANTON");
        modelBuilder.HasSequence("SEQ_CATEGORIA_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CONTROL_PROCESO");
        modelBuilder.HasSequence("SEQ_COORDINADOR_ACTIVIDAD");
        modelBuilder.HasSequence("SEQ_CORREO_PENDIENTE");
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
        modelBuilder.HasSequence("SEQ_UNIVERSIDAD");
        modelBuilder.HasSequence("SEQ_USUARIO");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
