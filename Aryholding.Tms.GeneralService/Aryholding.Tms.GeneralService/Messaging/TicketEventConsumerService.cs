using System.Diagnostics;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aryholding.Tms.GeneralService.Messaging
{
    public class TicketEventConsumerService : BackgroundService
    {
        public const string TopicName = "tms.ticket.events.v1";

        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TicketEventConsumerService> _logger;

        public TicketEventConsumerService(IConsumer<string, string> consumer, ILogger<TicketEventConsumerService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
        }

        private void ConsumeLoop(CancellationToken stoppingToken)
        {
            bool subscribed = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!subscribed)
                {
                    try
                    {
                        _consumer.Subscribe(TopicName);
                        subscribed = true;
                        _logger.LogInformation("Successfully subscribed to Kafka topic {Topic}", TopicName);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Kafka broker not yet ready or topic {Topic} unreachable. Retrying in 5 seconds...", TopicName);
                        try
                        {
                            Task.Delay(5000, stoppingToken).Wait(stoppingToken);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        continue;
                    }
                }

                ConsumeResult<string, string>? result;
                try
                {
                    result = _consumer.Consume(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException ex)
                {
                    _logger.LogWarning(ex, "Kafka consume warning on {Topic}", TopicName);
                    continue;
                }
                catch (KafkaException ex)
                {
                    _logger.LogWarning(ex, "Kafka broker connection temporarily unavailable on {Topic}. Reconnecting in 5 seconds...", TopicName);
                    try
                    {
                        Task.Delay(5000, stoppingToken).Wait(stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in Kafka consume loop on {Topic}. Retrying in 5 seconds...", TopicName);
                    try
                    {
                        Task.Delay(5000, stoppingToken).Wait(stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    continue;
                }

                if (result?.Message is null)
                    continue;

                try
                {
                    ProcessMessage(result);
                    _consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process or commit Kafka message {Key} on {Topic}", result.Message.Key, TopicName);
                }
            }
        }

        private void ProcessMessage(ConsumeResult<string, string> result)
        {
            var parentContext = default(ActivityContext);
            var traceparentHeader = result.Message.Headers?.FirstOrDefault(h => h.Key == "traceparent");
            if (traceparentHeader is not null)
            {
                var traceparent = Encoding.UTF8.GetString(traceparentHeader.GetValueBytes());
                ActivityContext.TryParse(traceparent, null, out parentContext);
            }

            using var activity = Telemetry.MessagingSource.StartActivity(
                $"{TopicName} process", ActivityKind.Consumer, parentContext);
            activity?.SetTag("messaging.system", "kafka");
            activity?.SetTag("messaging.destination.name", TopicName);
            activity?.SetTag("tms.ticket_key", result.Message.Key);

            _logger.LogInformation(
                "Dashboard projector received ticket event {Key} from partition {Partition}",
                result.Message.Key, result.Partition.Value);
        }
    }
}
