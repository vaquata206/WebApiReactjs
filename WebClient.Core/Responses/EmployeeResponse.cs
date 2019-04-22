using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Responses
{
    public class EmployeeResponse
    {
        public int Id_NhanVien { get; set; }
        public string Ma_NhanVien { get; set; }
        public string Ho_Ten { get; set; }
        public string Dia_Chi { get; set; }
        public string Dien_Thoai { get; set; }
        public string Email { get; set; }
        public DateTime Nam_Sinh { get; set; }
        public string So_CMND { get; set; }
        public DateTime NgayCap_CMND { get; set; }
        public string NoiCap_CMND { get; set; }
        public int Id_DonVi { get; set; }
        public int Chuc_Vu { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
