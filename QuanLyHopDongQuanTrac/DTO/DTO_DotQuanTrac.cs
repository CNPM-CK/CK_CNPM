using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DotQuanTrac
    {
        public string MaDot { get; set; }
        public string MaHD { get; set; }
        public string TenKhachHang { get; set; }
        public string NoiDung { get; set; }
        public string DotQuanTrac { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayDuKien { get; set; }
        public DateTime? NgayTraKQ { get; set; } 
        public string TrangThai { get; set; }
    }
}
