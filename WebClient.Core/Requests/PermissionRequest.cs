
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebClient.Core.Requests
{
    public class PermissionRequest
    {
        public int? Id_Quyen { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ten_Quyen { get; set; }

        [MaxLength(200)]
        public string Ghi_Chu { get; set; }
    }
}
