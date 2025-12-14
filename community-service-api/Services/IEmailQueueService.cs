using community_service_api.Models.DBTableEntities;

namespace community_service_api.Services;

public interface IEmailQueueService
{
    Task EnqueueEmailAsync(string recipientEmail, string subject, string body, int idUsuario, byte[]? attachment = null, string? attachmentName = null, Guid? idCertificacion = null);
    Task<IEnumerable<CorreoPendiente>> GetPendingEmailsAsync();
    Task UpdateEmailStatusAsync(int emailId, bool isSent, string? errorMessage = null);
}
