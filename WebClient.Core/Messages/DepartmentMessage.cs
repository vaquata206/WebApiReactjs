using WebClient.Core.Entities;

namespace WebClient.Core.Messages
{
    public class DepartmentMessage : BaseEntity
    {
        public string Ma_DonVi { get; set; }
        public string Ten_DonVi { get; set; }
        public string Dia_Chi { get; set; }
        public string MaSoThue { get; set; }
        public string Dien_Thoai { get; set; }
        public string Website { get; set; }
        public string TenNguoi_DaiDien { get; set; }
        public int Cap_DonVi { get; set; }
        public int Loai_DonVi { get; set; }
        public string Ma_DV_Cha { get; set; }
        public int Trang_Thai { get; set; }
        public string Email { get; set; }
        public string SMTP_Email { get; set; }
        public int? Port_Email { get; set; }
        public string Account_Email { get; set; }
        public string Pass_Email { get; set; }

        public string Ma_DV_Cu { get; set; }
    }
}
