using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebClient.Core.ViewModels
{
    public class ChangePasswordVM
    {
        /// <summary>
        /// The current pasword
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Mật khẩu phải 5 ký tự trở lên!", MinimumLength = 5)]
        public string MatKhauCu { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Mật khẩu phải 5 ký tự trở lên!", MinimumLength = 5)]
        public string MatKhauMoi { get; set; }
    }
}
