using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDong
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public string TenDoanhNghiep { get; set; }
        public string NguoiDaiDien { get; set; }
        public DateTime NgayKy { get; set; }
        public DateTime NgayDuKien { get; set; }
        public bool TrangThai { get; set; }
        public string DisplayText { get; set; }  // Để hiển thị trong ComboBox
    }
}
