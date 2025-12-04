using community_service_api.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace community_service_api.HostedServices;

public class EmailSendService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IOptions<EmailSettings> emailSettings;

    public EmailSendService(IServiceProvider serviceProvider, IOptions<EmailSettings> emailSettings)
    {
        this.serviceProvider = serviceProvider;
        this.emailSettings = emailSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            //var upcomingReservations = await this.GetReservationsToSendNotificationAsync(stoppingToken);

            //foreach (var reservation in upcomingReservations)
            //{
            //    try
            //    {
            //        await SendEmailAsync(reservation.User.Email, ReminderMailTemplate.GetReminderTemplate(reservation), "Recortadorio de su Reserva con Bundalow Paradise!");
            //        await UpdateNotificationStatusAsync(reservation, "Sent");
            //    }
            //    catch (Exception)
            //    {
            //        await UpdateNotificationStatusAsync(reservation, "Not Sent: Error");
            //    }
            //    finally
            //    {
            //        // Avoid Google detecting Spam by waiting 5 seconds each time.
            //        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            //    }

            //}
        }
    }

    public async Task SendEmailAsync(string recipientEmail, string mailTemplate, string subject)
    {
        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress("Hotel Online Services", _emailSettings.GmailUser));
        //message.To.Add(new MailboxAddress("", recipientEmail));
        //message.Subject = subject;
        //message.Body = new TextPart("html") { Text = mailTemplate };

        //using var client = new SmtpClient();
        //await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        //await client.AuthenticateAsync(_emailSettings.GmailUser, _emailSettings.GmailAppPassword);
        //await client.SendAsync(message);
        //await client.DisconnectAsync(true);
    }
}
