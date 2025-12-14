namespace community_service_api.MailTemplates;

public static class PasswordRecoveryMailTemplate
{
    public static string GetBody(string recoveryLink)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Recuperación de Contraseña</title>
</head>
<body>
    <h1>Recuperación de Contraseña</h1>
    <p>Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace para continuar:</p>
    <a href='{recoveryLink}'>Restablecer Contraseña</a>
    <p>Si no solicitaste un restablecimiento de contraseña, por favor ignora este correo.</p>
</body>
</html>
";
    }
}
