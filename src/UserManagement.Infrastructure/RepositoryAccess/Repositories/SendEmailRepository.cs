using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using UserManagement.Domain.Repositories.EmailRepository;

namespace UserManagement.Infrastructure.RepositoryAccess.Repositories;

public class SendEmailRepository : ISendEmail {

    private readonly IConfiguration _configuration;

    public SendEmailRepository(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void Send(string emailsTo, string subject, string message) {

        var apiKey = _configuration.GetSection("ApyKeys").Value;

        Configuration.Default.ApiKey["api-key"] = apiKey;

        var apiInstance = new TransactionalEmailsApi();

        string SenderName = "";
        string SenderEmail = "";
        var emailSender = new SendSmtpEmailSender(SenderName, SenderEmail);

        var emailReceiver1 = new SendSmtpEmailTo(emailsTo);
        var to = new List<SendSmtpEmailTo> {
            emailReceiver1
        };

        string HtmlContent = message;
        string TextContent = null!;

        try {
            var sendSmtpEmail = new SendSmtpEmail(
                emailSender,
                to,
                null,
                null,
                HtmlContent,
                TextContent,
                subject
            );

            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            Console.WriteLine("Response: \n " + result.ToJson());
        } catch (Exception e) {
            Console.WriteLine("Erro: " + e.Message);
        }

    }

}
