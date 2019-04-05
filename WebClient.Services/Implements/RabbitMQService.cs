using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    /// <summary>
    /// RabbitMQ Service
    /// </summary>
    public class RabbitMQService : IRabbitMQService
    {
        /// <summary>
        /// Connection
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// Channel
        /// </summary>
        private IModel channel;

        /// <summary>
        /// Publish a message
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        /// <param name="name"></param>
        public void Publish(object content, string sender, string action, string name)
        {
            if (this.IsDisconnected())
            {
                this.Connect();
            }

            this.channel.ExchangeDeclare(WebConfig.RabbitMQ.ExchangeName, ExchangeType.Fanout);
            var rabbitMessage = new RabbitMessage
            {
                Content = content,
                Sender = sender,
                ActionType = action
            };

            var props = this.channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("ObjectType", name);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rabbitMessage));

            if (WebConfig.Applications != null)
            {
                var apps = WebConfig.Applications;
                foreach (var app in apps)
                {
                    this.channel.QueueDeclare(queue: app.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    this.channel.QueueBind(queue: app.QueueName, exchange: WebConfig.RabbitMQ.ExchangeName, routingKey: "");
                }
            }

            this.channel.BasicPublish(
                exchange: WebConfig.RabbitMQ.ExchangeName,
                routingKey: "",
                basicProperties: props,
                body: body);
        }

        private bool IsDisconnected()
        {
            return this.connection == null || !this.connection.IsOpen;
        }

        private void Connect()
        {
            var factory = new ConnectionFactory();
            factory.UserName = WebConfig.RabbitMQ.Username;
            factory.Password = WebConfig.RabbitMQ.Password;
            // factory.VirtualHost = WebConfig.RabbitMQ.VirtualHost;
            factory.HostName = WebConfig.RabbitMQ.HostName;
            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();
        }
    }
}
