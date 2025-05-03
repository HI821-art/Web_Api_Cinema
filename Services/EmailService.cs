using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Web_Api_Cinema.Helpers;

public class MailJetEmailSender : IEmailSender
{
    private readonly MailJetSettings _mailJetSettings;

    public MailJetEmailSender(IOptions<MailJetSettings> mailJetSettings)
    {
        _mailJetSettings = mailJetSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);

        var request = new MailjetRequest
        {
            Resource = Send.Resource,
        }
        .Property(Send.FromEmail, _mailJetSettings.SenderEmail)
        .Property(Send.FromName, _mailJetSettings.SenderName)
        .Property(Send.Subject, subject)
        .Property(Send.HtmlPart, htmlMessage)
        .Property(Send.Recipients, new JArray
        {
            new JObject
            {
                { "Email", email }
            }
        });

        var response = await client.PostAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to send email: {response.StatusCode} {response.GetErrorMessage()}");
        }
    }
}
