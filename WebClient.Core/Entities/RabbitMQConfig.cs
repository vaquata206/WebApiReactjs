using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class RabbitMQConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
    }

    public class RabbitEntities
    {
        public const string Employee = "nhan_vien";
        public const string Department = "don_vi";
        public const string Account = "nguoi_dung";
    }

    public class RabbitActionTypes
    {
        public const string Update = "update";
        public const string Create = "create";
    }
}
