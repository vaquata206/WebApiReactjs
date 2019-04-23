using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Request
{
    public class UserAppsRequest
    {
        public int Id_NguoiDung { get; set; }
        public int[] Id_ChuongTrinhs { get; set; }
    }
}
