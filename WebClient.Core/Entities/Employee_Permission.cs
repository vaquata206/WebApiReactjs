using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class Employee_Permission : BaseEntity
    {
        public int Id_NhanVien { get; set; }
        public int Id_Quyen { get; set; }
        public int Id_ChucNang { get; set; }
    }
}
