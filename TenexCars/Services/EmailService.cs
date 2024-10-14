using MimeKit;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TenexCars.DataAccess.DTOs;
using TenexCars.Interfaces;
using MailKit.Net.Smtp;

namespace TenexCars.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

		public async Task SendOperatorSetPasswordEmailAsync(EmailDto emailDto)
		{
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("Tenex Cars", _config["EmailSettings:Username"]));
				message.To.Add(new MailboxAddress("", emailDto.To));
				message.Subject = emailDto.Subject;
                message.Body = new TextPart("html") 
                {
                    Text = emailDto.Body
                };
                /*message.Body = new TextPart("plain")
				{
					Text = emailDto.Body
				};*/

                using (var client = new SmtpClient())
				{
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;
					await client.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
					await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while sending email: {ex.Message}");
			}
		}

        public async Task ContactApplicantEmailAsync(EmailDto emailDto)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Tenex Cars", _config["EmailSettings:Username"]));
                message.To.Add(new MailboxAddress("", emailDto.To));
                message.Subject = emailDto.Subject;
                message.Body = new TextPart("html")
                {
                    Text = emailDto.Body
                };
                /*message.Body = new TextPart("plain")
				{
					Text = emailDto.Body
				};*/

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
                    await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            }
        }
        public async Task ApproveSubscriptionEmail(EmailDto emailDto)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Tenex Cars", _config["EmailSettings:Username"]));
                message.To.Add(new MailboxAddress("", emailDto.To));
                message.Subject = emailDto.Subject;
                message.Body = new TextPart("html")
                {
                    Text = emailDto.Body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
                    await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            }
        }
        //Optional Email Implementation template

        /*public async Task SendEmailAsync(EmailDto emailDto)
        {
            try
            {
                string body = PopulateRegisterEmail(emailDto.UserName, emailDto.Otp.ToString());

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Tenex Cars", _config["EmailSettings:Username"]));
                message.To.Add(new MailboxAddress("", emailDto.To));
                message.Subject = emailDto.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
                    await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you need to implement a logger, e.g., using ILogger)
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                throw; // Optionally rethrow or handle the exception as needed
            }
        }

        private string PopulateRegisterEmail(string userName, string otp)
        {
            string body = string.Empty;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "SignUpEmail.html");

            using (var reader = new StreamReader(filePath))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Otp}", otp);

            return body;
        }*/
    }
}
