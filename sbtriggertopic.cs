using System;
using System.IO;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SbFunc1
{
    public class sbtriggertopic
    {
        private readonly ILogger<sbtriggertopic> _logger;

        public sbtriggertopic(ILogger<sbtriggertopic> logger)
        {
            _logger = logger;
        }

        [Function(nameof(sbtriggertopic))]
        public void Run([ServiceBusTrigger("topic1", "sub1", Connection = "sbconnstring")] ServiceBusReceivedMessage message)
        {
            JToken.Parse(Encoding.UTF8.GetString(message.Body));

            string json = "{ \"invalid_json\": }"; // This JSON is intentionally invalid

            try
            {
                using (JsonReader reader = new JsonTextReader(new StringReader(json)))
                {
                    JToken token = JToken.Load(reader);

                    // Attempt to access a property in the JSON
                    Console.WriteLine(token["invalid_property"]);
                }
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Error reading JSON: " + ex.Message);
            }

            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
