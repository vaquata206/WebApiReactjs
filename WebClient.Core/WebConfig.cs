using System;
using System.Collections.Generic;
using System.Text;
using WebClient.Core.Entities;

namespace WebClient.Core
{
    /// <summary>
    /// Web config
    /// </summary>
    public static class WebConfig
    {
        /// <summary>
        /// Url api system
        /// </summary>
        public static string ApiSystemUrl;

        /// <summary>
        /// Connection string
        /// </summary>
        public static string ConnectionString;

        /// <summary>
        /// WebRoot path
        /// </summary>
        public static string WebRootPath;

        /// <summary>
        /// Jwt key
        /// </summary>
        public static string JWTKey;

        /// <summary>
        /// Applications
        /// </summary>
        public static IEnumerable<Application> Applications;

        /// <summary>
        /// RabbitMQ config
        /// </summary>
        public static RabbitMQConfig RabbitMQ;
    }
}
