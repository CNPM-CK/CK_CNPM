using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhanVien
    {
        public string maNV { get; set; }
        public string maPhong { get; set; }
        public string hoTen { get; set; }
        public DateTime ngaySinh { get; set; }
        public string gioiTinh { get; set; } 
        public string diaChi { get; set; }
        public string soDienThoai { get; set; }
        public string email { get; set; }
        public string tenPhong { get; set; }
        public int trangThai { get; set; }
        public string anhDaiDien { get; set; }
        public bool isTruongPhong { get; set; }
        public string tenTrangThai
        {
            get
            {
                return trangThai == 1 ? "Đang hoạt động"
                     : trangThai == 2 ? "Nghỉ phép"
                     : trangThai == 4 ? "Nghỉ thai sản"
                     : trangThai == 5 ? "Công tác"
                     : trangThai == 6 ? "Ngưng hoạt động"
                      : "Không xác định";
            }
        }

    }
}
