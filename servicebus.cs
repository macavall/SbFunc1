using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace SbFunc1
{
    public class servicebus
    {
        private readonly ILogger<servicebus> _logger;

        public servicebus(ILogger<servicebus> logger)
        {
            _logger = logger;
        }

        [Function(nameof(servicebus))]
        public void Run([ServiceBusTrigger("queue", Connection = "sbconnstring")] ServiceBusReceivedMessage message)
        {

            JToken.Parse(Encoding.UTF8.GetString(message.Body));

            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
