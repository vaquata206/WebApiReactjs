using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Responses
{
    public class FeatureResponse
    {
        public int Id_ChucNang { get; set; }
        public string Ma_ChucNang { get; set; }
        public string Ten_ChucNang { get; set; }
        public string MoTa_ChucNang { get; set; }
        public string Tooltip { get; set; }
        public int Id_ChucNang_Cha { get; set; }
        public string Controller_Name { get; set; }
        public string Action_Name { get; set; }
        public int Thu_Tu { get; set; }
        public int HienThi_Menu { get; set; }
    }
}
