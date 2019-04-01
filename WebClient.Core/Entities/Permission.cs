using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebClient.Core.Entities
{
    [Table("DM_Quyen")]
    public class Permission
    {
        public int Id_Quyen { get; set; }
        public string Ma_Quyen { get; set; }
        public string Ten_Quyen { get; set; }
        public bool Tinh_Trang { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
