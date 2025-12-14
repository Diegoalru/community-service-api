namespace community_service_api.MailTemplates;

public static class ActivationMailTemplate
{
    public static string GetBody(string activationLink)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Activación de Cuenta</title>
</head>
<body>
    <h1>¡Bienvenido a Community Service App!</h1>
    <p>Gracias por registrarte. Por favor, activa tu cuenta haciendo clic en el siguiente enlace:</p>
    <a href='{activationLink}'>Activar Cuenta</a>
    <p>Si no te registraste en nuestra aplicación, por favor ignora este correo.</p>
</body>
</html>
";
    }
}