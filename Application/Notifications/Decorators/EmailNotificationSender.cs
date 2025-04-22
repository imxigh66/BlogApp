using Application.Abstractions;
using Domain.Models.Notifications;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Application.Notifications.Decorators
{
    public class EmailNotificationSender : NotificationSenderDecorator
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationSender> _logger;

        public EmailNotificationSender(
            INotificationSender wrappedSender,
            IAuthRepository authRepository,
            IConfiguration configuration,
            ILogger<EmailNotificationSender> logger)
            : base(wrappedSender)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override async Task SendAsync(Notification notification)
        {
            // Сначала вызываем базовую функциональность
            await base.SendAsync(notification);

            try
            {
                // Получаем пользователя для отправки email
                var user = await _authRepository.GetUserByIdAsync(notification.UserId);
                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    _logger.LogWarning($"Не удалось отправить email-уведомление: пользователь {notification.UserId} не найден или email не указан");
                    return;
                }

                // Получаем настройки SMTP из конфигурации
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:Username"];
                var smtpPassword = _configuration["EmailSettings:Password"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];

                if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername) ||
                    string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(senderEmail))
                {
                    _logger.LogError("Настройки SMTP не сконфигурированы");
                    return;
                }

                // Создаем клиента SMTP
                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                // Создаем сообщение
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Новое уведомление с блога",
                    Body = FormatEmailBody(notification),
                    IsBodyHtml = true
                };
                mailMessage.To.Add(user.Email);

                // Отправляем email
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email-уведомление отправлено пользователю {user.Username} ({user.Email})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке email-уведомления: {ex.Message}");
            }
        }

        private string FormatEmailBody(Notification notification)
        {
            // Форматируем красивое HTML-сообщение для email
            var notificationType = notification.GetType().Name.Replace("Notification", "");
            var formattedMessage = notification.GetFormattedNotification();

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Уведомление с блога</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4a76a8; color: white; padding: 15px; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f9f9f9; padding: 20px; border-radius: 0 0 5px 5px; border: 1px solid #ddd; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
        .button {{ display: inline-block; background-color: #4a76a8; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin-top: 15px; }}
    </style>
</head>
<body>
    <div class='header'>
        <h2>Новое уведомление</h2>
    </div>
    <div class='content'>
        <p>Здравствуйте!</p>
        <p>{formattedMessage}</p>
        <p>Тип уведомления: {notificationType}</p>
        <a href='http://yourblog.com/notifications' class='button'>Перейти к уведомлениям</a>
    </div>
    <div class='footer'>
        <p>Это автоматическое сообщение, пожалуйста, не отвечайте на него. Если вы хотите отключить email-уведомления, перейдите в <a href='http://yourblog.com/settings'>настройки профиля</a>.</p>
    </div>
</body>
</html>";
        }
    }
}