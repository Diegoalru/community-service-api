namespace community_service_api.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string? GmailUser { get; set; }
        public string? GmailAppPassword { get; set; }
    }
}
