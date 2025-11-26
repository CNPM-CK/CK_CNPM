using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ThongSoNhapLieuDTO
    {
        public string MaDNTS { get; set; }
        public string MaTS { get; set; }
        public string TrangThai { get; set; }
        public string TenTS { get; set; }
        public string DonVi { get; set; }
        public string GiaTriToiThieu { get; set; }
        public string GiaTriToiDa { get; set; }
        public string PhuongPhap { get; set; }
        public string GiaTriDoDuoc { get; set; }
        public DateTime NgayDo { get; set; }
    }
}
