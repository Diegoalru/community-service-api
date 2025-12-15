namespace community_service_api.Models;

/// <summary>
/// Códigos de error definidos en PKG_INTEGRACION de Oracle.
/// Mapea las constantes de PL/SQL a valores tipados en C#.
/// </summary>
public enum CodigoErrorIntegracion
{
    /// <summary>
    /// Operación exitosa, sin errores.
    /// </summary>
    Exito = 0,

    /// <summary>
    /// Error base del paquete.
    /// </summary>
    ErrorBase = -20700,

    /// <summary>
    /// C_ERR_NULL_DATOS: El parámetro PV_DATOS es nulo.
    /// </summary>
    DatosNulos = -20701,

    /// <summary>
    /// C_ERR_INVALID_JSON: JSON inválido o incompleto.
    /// </summary>
    JsonInvalido = -20702,

    /// <summary>
    /// C_ERR_NO_DATA_FOUND: Datos no encontrados.
    /// </summary>
    DatosNoEncontrados = -20703,

    /// <summary>
    /// C_ERR_USUARIO_FAILED: Error al crear usuario (ej. duplicado).
    /// </summary>
    ErrorCrearUsuario = -20704,

    /// <summary>
    /// C_ERR_UBICACION_FAILED: Error al crear ubicación.
    /// </summary>
    ErrorCrearUbicacion = -20705,

    /// <summary>
    /// C_ERR_PERFIL_FAILED: Error al crear perfil.
    /// </summary>
    ErrorCrearPerfil = -20706,

    /// <summary>
    /// C_ERR_CORRESPONDENCIA_FAILED: Error al crear correspondencia.
    /// </summary>
    ErrorCrearCorrespondencia = -20707,

    /// <summary>
    /// C_ERR_TOKEN_FAILED: Error al generar token.
    /// </summary>
    ErrorGenerarToken = -20708,

    /// <summary>
    /// C_ERR_ACTIVIDAD_FAILED: Error al crear actividad.
    /// </summary>
    ErrorCrearActividad = -20709,

    /// <summary>
    /// C_ERR_COORDINADOR_FAILED: Error al asignar coordinador.
    /// </summary>
    ErrorAsignarCoordinador = -21710,

    /// <summary>
    /// C_ERR_HORARIO_FAILED: Error al crear horario.
    /// </summary>
    ErrorCrearHorario = -21711,

    /// <summary>
    /// C_ERR_INSCRIPCION_FAILED: Error en inscripción.
    /// </summary>
    ErrorInscripcion = -21712,

    /// <summary>
    /// C_ERR_ASIGNAR_ROL_FAILED: Error al asignar rol.
    /// </summary>
    ErrorAsignarRol = -21713,

    /// <summary>
    /// C_ERR_UPDATE_PERFIL_FAILED: Error al actualizar perfil.
    /// </summary>
    ErrorActualizarPerfil = -21714,

    /// <summary>
    /// C_ERR_UPDATE_ACTIVIDAD_FAILED: Error al actualizar actividad.
    /// </summary>
    ErrorActualizarActividad = -21715,

    /// <summary>
    /// C_ERR_CAMBIO_PASSWORD_FAILED: Error al cambiar contraseña.
    /// </summary>
    ErrorCambiarPassword = -21716,

    /// <summary>
    /// C_ERR_RESTABLECER_PASSWORD_FAILED: Error al restablecer contraseña.
    /// </summary>
    ErrorRestablecerPassword = -21717,

    /// <summary>
    /// C_ERR_ACTIVACION_FAILED: Error en activación de cuenta.
    /// </summary>
    ErrorActivacion = -21718,

    /// <summary>
    /// Error interno del servidor (usado en C#, no en PL/SQL).
    /// </summary>
    ErrorInterno = -1
}

/// <summary>
/// Extensiones para trabajar con códigos de error de integración.
/// </summary>
public static class CodigoErrorIntegracionExtensions
{
    /// <summary>
    /// Determina si el código representa un error interno del servidor.
    /// </summary>
    public static bool EsErrorInterno(this CodigoErrorIntegracion codigo)
    {
        return codigo == CodigoErrorIntegracion.ErrorInterno;
    }

    /// <summary>
    /// Determina si el código representa una operación exitosa.
    /// </summary>
    public static bool EsExitoso(this CodigoErrorIntegracion codigo)
    {
        return codigo == CodigoErrorIntegracion.Exito;
    }

    /// <summary>
    /// Obtiene un mensaje descriptivo para el código de error.
    /// </summary>
    public static string ObtenerDescripcion(this CodigoErrorIntegracion codigo)
    {
        return codigo switch
        {
            CodigoErrorIntegracion.Exito => "Operación exitosa.",
            CodigoErrorIntegracion.DatosNulos => "Los datos proporcionados son nulos.",
            CodigoErrorIntegracion.JsonInvalido => "El formato de los datos es inválido.",
            CodigoErrorIntegracion.DatosNoEncontrados => "No se encontraron los datos solicitados.",
            CodigoErrorIntegracion.ErrorCrearUsuario => "Error al crear el usuario. Posible duplicado.",
            CodigoErrorIntegracion.ErrorCrearUbicacion => "Error al registrar la ubicación.",
            CodigoErrorIntegracion.ErrorCrearPerfil => "Error al crear el perfil del usuario.",
            CodigoErrorIntegracion.ErrorCrearCorrespondencia => "Error al registrar los datos de contacto.",
            CodigoErrorIntegracion.ErrorGenerarToken => "Error al generar el token de verificación.",
            CodigoErrorIntegracion.ErrorCrearActividad => "Error al crear la actividad.",
            CodigoErrorIntegracion.ErrorAsignarCoordinador => "Error al asignar el coordinador.",
            CodigoErrorIntegracion.ErrorCrearHorario => "Error al crear el horario.",
            CodigoErrorIntegracion.ErrorInscripcion => "Error en el proceso de inscripción.",
            CodigoErrorIntegracion.ErrorAsignarRol => "Error al asignar el rol.",
            CodigoErrorIntegracion.ErrorActualizarPerfil => "Error al actualizar el perfil.",
            CodigoErrorIntegracion.ErrorActualizarActividad => "Error al actualizar la actividad.",
            CodigoErrorIntegracion.ErrorCambiarPassword => "Error al cambiar la contraseña.",
            CodigoErrorIntegracion.ErrorRestablecerPassword => "Error al restablecer la contraseña.",
            CodigoErrorIntegracion.ErrorActivacion => "Error en la activación de la cuenta.",
            CodigoErrorIntegracion.ErrorInterno => "Error interno del servidor.",
            _ => "Error desconocido."
        };
    }

    /// <summary>
    /// Convierte un entero a CodigoErrorIntegracion.
    /// </summary>
    public static CodigoErrorIntegracion ToCodigoError(this int codigo)
    {
        return Enum.IsDefined(typeof(CodigoErrorIntegracion), codigo)
            ? (CodigoErrorIntegracion)codigo
            : CodigoErrorIntegracion.ErrorInterno;
    }
}

