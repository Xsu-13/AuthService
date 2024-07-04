using Auth.AuthService.Contracts.ProducerContracts;
using Confluent.Kafka;
using System.Threading;
using System.Text.Json;

namespace Auth.AuthService.Services
{
    public class MailProducer
    {
        private readonly ILogger<MailProducer> _logger;

        public MailProducer(ILogger<MailProducer> logger)
        {
            _logger = logger;
        }

        public async Task ProduceConfirmEmailAsync(MailConfirmationRequest request, CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(topic: "confirm-email",
                new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(request)
                },
                cancellationToken);

                _logger.LogInformation($"Delivered message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");

            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
            finally
            {
                producer.Flush();
                //producer.Dispose();
            }
        }

        public async Task ProduceResetPasswordEmailAsync(PasswordResetRequest request, CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(topic: "reset-password-email",
                new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(request)
                },
                cancellationToken);

                _logger.LogInformation($"Delivered message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");

            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
            finally
            {
                producer.Flush();
                //producer.Dispose();
            }
        }
    }
}
