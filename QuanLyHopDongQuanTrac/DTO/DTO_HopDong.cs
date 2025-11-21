using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_HopDong
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChiKhachHang { get; set; }
        public DateTime NgayKy { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; }
        public string TanSuatQuanTrac { get; set; }
        public string SoHD { get; set; }

        // Thông tin bổ sung cho phân tích chu kỳ
        public int SoNgayConLai
        {
            get
            {
                TimeSpan diff = NgayKetThuc - DateTime.Now;
                return diff.Days > 0 ? diff.Days : 0;
            }
        }

        public int TongSoNgay
        {
            get
            {
                return (NgayKetThuc - NgayKy).Days;
            }
        }

        public double TiLeHoanThanh
        {
            get
            {
                if (TongSoNgay <= 0) return 0;

                int ngayDaQua = (DateTime.Now - NgayKy).Days;
                if (ngayDaQua < 0) return 0;
                if (ngayDaQua > TongSoNgay) return 100;

                return (double)ngayDaQua / TongSoNgay * 100;
            }
        }

        public bool DaQuaHan
        {
            get
            {
                return DateTime.Now > NgayKetThuc && TrangThai != "Hoàn thành";
            }
        }

        public bool SapHetHan
        {
            get
            {
                return SoNgayConLai > 0 && SoNgayConLai <= 30 && TrangThai != "Hoàn thành";
            }
        }

        // Constructor
        public DTO_HopDong()
        {
            MaHD = string.Empty;
            MaKH = string.Empty;
            TenKhachHang = string.Empty;
            DiaChiKhachHang = string.Empty;
            NgayKy = DateTime.Now;
            NgayKetThuc = DateTime.Now;
            TrangThai = string.Empty;
            TanSuatQuanTrac = string.Empty;
            SoHD = string.Empty;
        }

        public DTO_HopDong(string maHD, string maKH, string tenKH, DateTime ngayKy,
            DateTime ngayKetThuc, string trangThai, string tanSuat, string soHD)
        {
            MaHD = maHD;
            MaKH = maKH;
            TenKhachHang = tenKH;
            DiaChiKhachHang = string.Empty;
            NgayKy = ngayKy;
            NgayKetThuc = ngayKetThuc;
            TrangThai = trangThai;
            TanSuatQuanTrac = tanSuat;
            SoHD = soHD;
        }
    }
}
