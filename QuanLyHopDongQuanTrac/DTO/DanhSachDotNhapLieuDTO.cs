using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DanhSachDotNhapLieuDTO
    {
        public string maDot { get; set; }
        public string maHD { get; set; }
        public DateTime ngayBatDau { get; set; }
        public DateTime ngayDuKien { get; set; }
        public string ngayConLai { get; set; }
        public string trangThai { get; set; }
    }
}
