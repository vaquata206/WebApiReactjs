﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class Permission_Feature
    {
        public int Id_Quyen { get; set; }
        public int Id_ChucNang { get; set; }
        public int Id_NV_KhoiTao { get; set; }
        public int? Id_NV_CapNhap { get; set; }
        public DateTime Ngay_KhoiTao { get; set; }
        public DateTime? Ngay_CapNhap { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
