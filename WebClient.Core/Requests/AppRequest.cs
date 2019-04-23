using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebClient.Core.Requests
{
    public class AppRequest
    {
        public int Id_ChuongTrinh { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ten_ChuongTrinh { get; set; }

        [MaxLength(200)]
        public string Mo_Ta { get; set; }

        [Required]
        [MaxLength(200)]
        public string URL { get; set; }

        [MaxLength(200)]
        public string Ghi_Chu { get; set; }
    }
}
