using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class RabbitMessage
    {
        public object Content { get; set; }
        public string ActionType { get; set; }
    }
}
