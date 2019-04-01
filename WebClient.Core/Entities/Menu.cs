using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class Menu
    {
        public string M_ID { get; set; }
        public string M_P_ID { get; set; }
        public string M_Name { get; set; }
        public string Controler_Name { get; set; }
        public string Action_Name { get; set; }
        public string Url { get; set; }
        public bool Show { get; set; }
    }
}
