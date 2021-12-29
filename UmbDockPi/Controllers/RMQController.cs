using RabbitMQ.Client;
using System.Text;
using Umbraco.Cms.Web.Common.Controllers;
using Microsoft.Extensions.Logging;
using System;

namespace UmbDockPi.Controllers
{
    public class RMQController : UmbracoApiController
    {
        private readonly ILogger<RMQController> _logger;
        public RMQController(ILogger<RMQController> logger)
        {
            _logger = logger;
        }

        public Microsoft.AspNetCore.Mvc.RedirectResult SendMessage(string name)
        {
            _logger.LogInformation("SendMessage : " + name);
            try
            {
                // TODO : Put the hostname in Config
                // var factory = new ConnectionFactory() { HostName = "192.168.0.144" };
                //var factory = new ConnectionFactory() { HostName = "localhost" };
                var factory = new ConnectionFactory() { HostName = "rabbit_mq" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = $"Hey {name}! Happy 24 Days of Umbraco. #h5yr!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                }
                _logger.LogInformation("SendMessage Completed");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessage Error. {0}", ex.Message);
            }

            // Being Lazy :-)
            return Redirect("/thank-you/");
        }

    }
}
