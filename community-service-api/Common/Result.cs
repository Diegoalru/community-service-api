namespace community_service_api.Common;

/// <summary>
/// Representa el resultado de una operación que puede ser exitosa o fallida.
/// Implementa el patrón Result para manejo explícito de errores.
/// </summary>
/// <typeparam name="T">Tipo del valor retornado en caso de éxito.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica si la operación falló.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Valor retornado en caso de éxito.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Mensaje de error en caso de falla.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Código de error (mapea a CodigoErrorIntegracion).
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Indica si es un error interno del servidor (código -1).
    /// </summary>
    public bool IsInternalError => ErrorCode == -1;

    private Result(bool isSuccess, T? value, string errorMessage, int errorCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Crea un resultado exitoso con el valor especificado.
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, string.Empty, 0);

    /// <summary>
    /// Crea un resultado fallido con mensaje y código de error.
    /// </summary>
    public static Result<T> Failure(string errorMessage, int errorCode = 0) =>
        new(false, default, errorMessage, errorCode);

    /// <summary>
    /// Crea un resultado fallido para errores internos del servidor.
    /// </summary>
    public static Result<T> InternalError(string errorMessage) =>
        new(false, default, errorMessage, -1);

    /// <summary>
    /// Ejecuta una acción si el resultado es exitoso.
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Value != null)
            action(Value);
        return this;
    }

    /// <summary>
    /// Ejecuta una acción si el resultado es fallido.
    /// </summary>
    public Result<T> OnFailure(Action<string, int> action)
    {
        if (IsFailure)
            action(ErrorMessage, ErrorCode);
        return this;
    }

    /// <summary>
    /// Transforma el valor si el resultado es exitoso.
    /// </summary>
    public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
    {
        return IsSuccess && Value != null
            ? Result<TNew>.Success(mapper(Value))
            : Result<TNew>.Failure(ErrorMessage, ErrorCode);
    }
}

/// <summary>
/// Representa el resultado de una operación que no retorna valor.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string ErrorMessage { get; }
    public int ErrorCode { get; }
    public bool IsInternalError => ErrorCode == -1;

    private Result(bool isSuccess, string errorMessage, int errorCode)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true, string.Empty, 0);

    public static Result Failure(string errorMessage, int errorCode = 0) =>
        new(false, errorMessage, errorCode);

    public static Result InternalError(string errorMessage) =>
        new(false, errorMessage, -1);
}

/// <summary>
/// Datos de respuesta de autenticación exitosa.
/// </summary>
public class AuthResult
{
    public int IdUsuario { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiration { get; set; }
}

/// <summary>
/// Datos de respuesta de registro exitoso.
/// </summary>
public class RegistroResult
{
    public int IdUsuario { get; set; }
    public string? ActivationToken { get; set; }
}

/// <summary>
/// Datos de respuesta de activación exitosa.
/// </summary>
public class ActivacionResult
{
    public int IdUsuario { get; set; }
}

