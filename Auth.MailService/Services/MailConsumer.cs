using Auth.MailService.Contracts.ConsumerContracts;
using Auth.MailService.Controllers;
using Confluent.Kafka;
using System.Text.Json;
using System.Threading;

namespace Auth.MailService.Services
{
    public class MailConsumer:BackgroundService
    {
        private readonly ILogger<MailConsumer> _logger;
        private readonly IMailService mailService;

        public MailConsumer(ILogger<MailConsumer> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "email-confirmation-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumerConfirmation = new ConsumerBuilder<Ignore, string>(config).Build();
            consumerConfirmation.Subscribe("confirm-email");

            var configReset = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "password-reset-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumerReset = new ConsumerBuilder<Ignore, string>(configReset).Build();
            consumerReset.Subscribe("reset-password-email");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumerConfirmation.Consume(TimeSpan.FromSeconds(5));
                    var consumeResultReset = consumerReset.Consume(TimeSpan.FromSeconds(5));

                    if(consumeResult != null)
                    {
                        _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.Offset}'");

                        MailConfirmationRequest? request = JsonSerializer.Deserialize<MailConfirmationRequest>(consumeResult.Message.Value);

                        await mailService.SendEmailAsync(request?.mail ?? "", request?.link ?? "", "src/mailConfirmation.html", "Please Confirm Your Email Address");
                    }
                    if(consumeResultReset != null)
                    {
                        _logger.LogInformation($"Consumed message '{consumeResultReset.Message.Value}' at: '{consumeResultReset.Offset}'");

                        ResetPasswordRequest? request = JsonSerializer.Deserialize<ResetPasswordRequest>(consumeResultReset.Message.Value);

                        await mailService.SendEmailAsync(request?.mail ?? "", request?.link ?? "", "src/mailPasswordReset.html", "Reset your password");
                    }
                    
                }
                catch (Exception e)
                {
                    _logger.LogError("Consumer confirmation email error: " + e);
                }
            }
        }
    }
}
