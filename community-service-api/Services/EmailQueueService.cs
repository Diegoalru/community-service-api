using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public class EmailQueueService(IRepository<CorreoPendiente> repository) : IEmailQueueService
{
    public async Task EnqueueEmailAsync(string recipientEmail, string subject, string body, int idUsuario, byte[]? attachment = null, string? attachmentName = null, Guid? idCertificacion = null)
    {
        var email = new CorreoPendiente
        {
            Destinatario = recipientEmail,
            Asunto = subject,
            Cuerpo = body,
            Adjunto = attachment,
            NombreAdjunto = attachmentName,
            IdUsuario = idUsuario,
            IdCertificacion = idCertificacion,
            Estado = "P",
            Intentos = 0,
            FechaCreacion = DateTime.UtcNow
        };
        await repository.AddAsync(email);
    }

    public async Task<IEnumerable<CorreoPendiente>> GetPendingEmailsAsync()
    {
        // Recupera los correos cuyo Estado sea 'P' (Pendiente) o 'F' (Fallido) con menos de 5 intentos de reenvío.
        // Se asume que repository.GetAllAsync() devuelve todos los registros y que el filtrado se realiza en memoria.
        
        var allEmails = await repository.GetAllAsync();
        return allEmails.Where(e => e.Estado == "P" || (e.Estado == "F" && e.Intentos < 5));
    }

    public async Task UpdateEmailStatusAsync(int emailId, bool isSent, string? errorMessage = null)
    {
        var email = await repository.GetByIdAsync(emailId);
        if (email != null)
        {
            email.Estado = isSent ? "E" : "F"; // Usar 'E' para Enviado, 'F' para Fallido
            email.MensajeError = errorMessage;
            email.FechaUltimoIntento = DateTime.UtcNow;

            if (!isSent)
            {
                email.Intentos++;
            }

            await repository.UpdateAsync(email);
        }
    }
}
