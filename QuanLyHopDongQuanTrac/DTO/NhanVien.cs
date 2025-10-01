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
        public string tenTK { get; set; }
        public string maPhong { get; set; }
        public string hoTen { get; set; }
        public DateTime ngaySinh { get; set; }
        public string gioiTinh { get; set; } //chỉnh lại thành string cho dễ hiển thị
        public string diaChi { get; set; }
        public string soDienThoai { get; set; }
        public string email { get; set; }

        // Thêm thuộc tính tên phòng
        public string tenPhong { get; set; }
    }
}
