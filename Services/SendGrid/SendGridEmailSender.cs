using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Services {
  public class SendGridEmailSender : IEmailSender {

    public SendGridEmailSenderOptions Options { get; set; }

    public SendGridEmailSender(IOptions<SendGridEmailSenderOptions> options) {
      this.Options = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
      await Execute(Options.ApiKey, subject, htmlMessage, email);
    }

    public async Task<Response> Execute(string apiKey, string subject, string message, string email) {
      var client = new SendGridClient(apiKey);
      var msg = new SendGridMessage()
      {
        From = new EmailAddress(Options.SenderEmail, Options.SenderName),
        Subject = subject,
        PlainTextContent = message,
        HtmlContent = message
      };
      msg.AddTo(new EmailAddress(email));

      // disable tracking settings
      msg.SetClickTracking(false, false);
      msg.SetOpenTracking(false);
      msg.SetGoogleAnalytics(false);
      msg.SetSubscriptionTracking(false);

      return await client.SendEmailAsync(msg);
    }
  }
}
