using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.Services.Interfaces
{
    /// <summary>
    /// RabbitMQ service
    /// </summary>
    public interface IRabbitMQService
    {
        /// <summary>
        /// Publish a message
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="name"></param>
        void Publish(object content, string action, string name);
    }
}
