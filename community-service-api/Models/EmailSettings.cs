namespace community_service_api.Models
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public int Port { get; set; }
        public string? GmailUser { get; set; }
        public string? GmailAppPassword { get; set; }
    }
}
