using Application.Abstractions;
using Domain.Models.Notifications;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Application.Notifications.Decorators
{
    public class SmsNotificationSender : NotificationSenderDecorator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsNotificationSender> _logger;
        private readonly HttpClient _httpClient;

        // Репозиторий для получения телефонов пользователей (его нужно создать)
        private readonly IUserPhoneRepository _phoneRepository;

        public SmsNotificationSender(
            INotificationSender wrappedSender,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IUserPhoneRepository phoneRepository,
            ILogger<SmsNotificationSender> logger)
            : base(wrappedSender)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("SmsService");
            _phoneRepository = phoneRepository;
            _logger = logger;
        }

        public override async Task SendAsync(Notification notification)
        {
            await base.SendAsync(notification);

            try
            {
                var phoneNumber = await _phoneRepository.GetUserPhoneAsync(notification.UserId);
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarning($"Не удалось отправить SMS: номер не найден для пользователя {notification.UserId}");
                    return;
                }

                var accountSid = _configuration["SmsSettings:AccountSid"];
                var authToken = _configuration["SmsSettings:AuthToken"];
                var fromNumber = _configuration["SmsSettings:FromNumber"];

                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber))
                {
                    _logger.LogError("Twilio-настройки не заданы");
                    return;
                }

                TwilioClient.Init(accountSid, authToken);

                var smsText = FormatSmsText(notification);

                var message = await MessageResource.CreateAsync(
                    body: smsText,
                    from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                _logger.LogInformation($"SMS отправлено: SID = {message.Sid}, Получатель: {phoneNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке SMS через Twilio: {ex.Message}");
            }
        }

        private string FormatSmsText(Notification notification)
        {
            // SMS должны быть короткими, поэтому берем только основную информацию
            var formattedMessage = notification.GetFormattedNotification();

            // Ограничиваем длину сообщения для SMS (обычно до 160 символов)
            if (formattedMessage.Length > 150)
            {
                formattedMessage = formattedMessage.Substring(0, 147) + "...";
            }

            return formattedMessage;
        }
    }
}