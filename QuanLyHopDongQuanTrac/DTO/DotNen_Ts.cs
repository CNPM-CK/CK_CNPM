using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DotNenTs
    {
        public string MaDNTS { get; set; }
        public string MaDN { get; set; }
        public string MaTS { get; set; }
        public string TenTS { get; set; }          // ⭐ Trường mới
        public string DonVi { get; set; }          // ⭐ Trường mới
        public double? GiaTriToiThieu { get; set; } // ⭐ Trường mới
        public double? GiaTriToiDa { get; set; }    // ⭐ Trường mới
        public string PhuongPhap { get; set; }     // ⭐ Trường mới
        public string MaPhong { get; set; }
    }
}
