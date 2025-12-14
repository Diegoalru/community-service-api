namespace community_service_api.MailTemplates
{
    public static class CertificateMailTemplate
    {
        public static string GetCertificateMailTemplate()
        {
            return """
            <!DOCTYPE html>
            <html lang="es">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Certificado de Participación</title>
            </head>
            <body style="margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4;">
                <table role="presentation" style="width: 100%; border-collapse: collapse;">
                    <tr>
                        <td style="padding: 40px 0;">
                            <table role="presentation" style="max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);">
                                <!-- Header -->
                                <tr>
                                    <td style="background-color: #2c5282; padding: 30px 40px; text-align: center;">
                                        <h1 style="margin: 0; color: #ffffff; font-size: 24px; font-weight: 600;">
                                            Community Service App
                                        </h1>
                                    </td>
                                </tr>
                                <!-- Content -->
                                <tr>
                                    <td style="padding: 40px;">
                                        <h2 style="margin: 0 0 20px 0; color: #2d3748; font-size: 20px; font-weight: 600;">
                                            Certificado de Participación Generado
                                        </h2>
                                        <p style="margin: 0 0 16px 0; color: #4a5568; font-size: 16px; line-height: 1.6;">
                                            Estimado/a participante,
                                        </p>
                                        <p style="margin: 0 0 16px 0; color: #4a5568; font-size: 16px; line-height: 1.6;">
                                            Nos complace informarle que su <strong>Certificado de Participación</strong> ha sido 
                                            generado y procesado exitosamente.
                                        </p>
                                        <p style="margin: 0 0 16px 0; color: #4a5568; font-size: 16px; line-height: 1.6;">
                                            Encontrará el certificado adjunto a este correo electrónico en formato PDF.
                                        </p>
                                        <p style="margin: 0 0 16px 0; color: #4a5568; font-size: 16px; line-height: 1.6;">
                                            Agradecemos su valiosa contribución y participación en nuestras actividades 
                                            de servicio comunitario.
                                        </p>
                                        <!-- Divider -->
                                        <hr style="border: none; border-top: 1px solid #e2e8f0; margin: 30px 0;">
                                        <p style="margin: 0; color: #718096; font-size: 14px; line-height: 1.6;">
                                            Si tiene alguna consulta, no dude en contactarnos.
                                        </p>
                                    </td>
                                </tr>
                                <!-- Footer -->
                                <tr>
                                    <td style="background-color: #f7fafc; padding: 24px 40px; text-align: center; border-top: 1px solid #e2e8f0;">
                                        <p style="margin: 0 0 8px 0; color: #718096; font-size: 14px;">
                                            Atentamente,
                                        </p>
                                        <p style="margin: 0; color: #4a5568; font-size: 14px; font-weight: 600;">
                                            Equipo de Community Service App
                                        </p>
                                        <p style="margin: 16px 0 0 0; color: #a0aec0; font-size: 12px;">
                                            Este es un correo automático, por favor no responda a este mensaje.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;;
        }
    }
}
