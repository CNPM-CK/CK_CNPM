using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChatMessageDbDTO
    {
        public int MaTinNhan { get; set; }
        public int MaPhien { get; set; }
        public int ThuTu { get; set; }
        public string VaiTro { get; set; }   // VaiTroGui
        public string TenNguoiGui { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGianTao { get; set; }
    }

}
