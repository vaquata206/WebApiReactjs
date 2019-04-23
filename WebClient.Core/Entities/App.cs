using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{ 
    [Table("chuong_trinh")]
    public class App
    {
        public int Id_ChuongTrinh { get; set; }
        public string Ma_ChuongTrinh { get; set; }
        public string Ten_ChuongTrinh { get; set; }
        public string Mo_Ta { get; set; }
        public int Thu_Tu { get; set; }
        public int Tinh_Trang { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
