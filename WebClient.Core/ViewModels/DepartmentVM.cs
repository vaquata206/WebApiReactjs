using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebClient.Core.ViewModels
{
    public class DepartmentVM
    {
        public int Id_DonVi { get; set; }
        [Required]
        [MaxLength(60)]
        public string Ma_DonVi { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ten_DonVi { get; set; }

        [Required]
        [MaxLength(200)]
        public string Dia_Chi { get; set; }

        [Required]
        [MaxLength(40)]
        public string MaSoThue { get; set; }
        [Required]
        [MaxLength(40)]
        public string Dien_Thoai { get; set; }

        [MaxLength(200)]
        public string Website { get; set; }

        [Required]
        [MaxLength(50)]
        public string TenNguoi_DaiDien { get; set; }

        [Required]
        public int Loai_DonVi { get; set; }

        public int Id_NV_CapNhat { get; set; }

        public int Id_DV_Cha { get; set; }

        [Required]
        public int Cap_DonVi { get; set; }

        [MaxLength(200)]
        public string Ghi_Chu { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string SMTP_Email { get; set; }

        public int? Port_Email { get; set; }

        [MaxLength(32)]
        public string Account_Email { get; set; }

        [MaxLength(32)]
        public string Pass_Email { get; set; }
    }
}
