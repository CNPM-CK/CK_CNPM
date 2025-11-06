using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class KhachHang
    {
        public string maKH { get; set; }
        public string tenDoanhNghiep{ get; set; }

        public string kyHieuDN { get; set; }
        public string diaChi { get; set; }

        public string nguoiDaiDien { get; set; }

        public string soDienThoaiKH { get; set; }

        public string maSoThue { get; set; }
        
        public string emailNguoiDaiDien { get; set; }
        
        public string emailDoanhNghiep { get; set; }
        
        public int trangThai { get; set; }
        public string tenTrangThai
        {
            get
            {
                return trangThai == 1 ? "Đang hợp tác"
                     : trangThai == 2 ? "Ngừng hợp tác"
                     : "Không xác định";
            }
        }

    }
}
