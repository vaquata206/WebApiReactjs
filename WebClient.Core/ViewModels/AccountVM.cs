using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebClient.Core.ViewModels
{
    public class AccountVM
    {
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string MatKhau { get; set; }

        [Required]
        public int Id_NhanVien { get; set; }
    }
}
