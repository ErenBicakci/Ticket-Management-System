using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aryholding.Tms.TicketManagement.Application.Services.Implementationts
{
    public class KafkaTicketEventPublisher : ITicketEventPublisher
    {
        public const string TopicName = "tms.ticket.events.v1";

        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaTicketEventPublisher> _logger;

        public KafkaTicketEventPublisher(IProducer<string, string> producer, ILogger<KafkaTicketEventPublisher> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task PublishAsync(TicketEventMessage message, CancellationToken cancellationToken = default)
        {
            using var activity = Telemetry.MessagingSource.StartActivity($"{TopicName} publish", ActivityKind.Producer);
            activity?.SetTag("messaging.system", "kafka");
            activity?.SetTag("messaging.destination.name", TopicName);
            activity?.SetTag("tms.ticket_id", message.TicketId);
            activity?.SetTag("tms.event_type", message.EventType);

            var headers = new Headers
            {
                { "event-type", Encoding.UTF8.GetBytes(message.EventType) }
            };
            if (activity?.Id is { } traceparent)
            {
                headers.Add("traceparent", Encoding.UTF8.GetBytes(traceparent));
            }

            try
            {
                await _producer.ProduceAsync(TopicName, new Message<string, string>
                {
                    Key = message.TicketId.ToString(),
                    Value = JsonSerializer.Serialize(message),
                    Headers = headers
                }, cancellationToken);
            }
            catch (ProduceException<string, string> ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                _logger.LogError(ex, "Failed to publish {EventType} event for ticket {TicketId}", message.EventType, message.TicketId);
                throw;
            }
        }
    }
}
