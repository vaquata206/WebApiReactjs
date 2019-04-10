using System;
using WebClient.Core.Entities;

namespace WebClient.Core.Messages
{
    public class AccountMessage : BaseEntity
    {
        public string Ma_NguoiDung { get; set; }
        public string UserName { get; set; }
        public string MatKhau { get; set; }
        public int Id_VaiTro { get; set; }
        public int Quan_Tri { get; set; }
        public string Ma_NhanVien { get; set; }
        public int SoLan_LoginSai { get; set; }
        public DateTime? Ngay_Login { get; set; }
        public DateTime? Ngay_DoiMatKhau { get; set; }
        public int Trang_Thai { get; set; }

        public string Ma_NguoiDung_Cu { get; set; }
    }
}
