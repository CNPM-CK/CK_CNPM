using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDongDTO
    {
        // ✅ Thuộc tính chính từ database
        public string maHD { get; set; }
        public string maKH { get; set; }
        public DateTime ngayKy { get; set; }
        public DateTime ngayKetThucHD { get; set; }
        public string trangThai { get; set; }
        public string tanSuatQuanTrac { get; set; }
        public string soHD { get; set; }

        // ✅ Thuộc tính bổ sung (tùy chọn - để hiển thị)
        public string TenKhachHang { get; set; }
        public string DiaChiKhachHang { get; set; }

        // ✅ Thuộc tính tính toán
        public int SoNgayConLai
        {
            get
            {
                TimeSpan diff = ngayKetThucHD - DateTime.Now;
                return diff.Days > 0 ? diff.Days : 0;
            }
        }

        public int TongSoNgay
        {
            get
            {
                return (ngayKetThucHD - ngayKy).Days;
            }
        }

        public double TiLeHoanThanh
        {
            get
            {
                if (TongSoNgay <= 0) return 0;

                int ngayDaQua = (DateTime.Now - ngayKy).Days;
                if (ngayDaQua < 0) return 0;
                if (ngayDaQua > TongSoNgay) return 100;

                return (double)ngayDaQua / TongSoNgay * 100;
            }
        }

        public bool DaQuaHan
        {
            get
            {
                return DateTime.Now > ngayKetThucHD && !IsHoanThanh();
            }
        }

        public bool SapHetHan
        {
            get
            {
                return SoNgayConLai > 0 && SoNgayConLai <= 30 && !IsHoanThanh();
            }
        }

        private bool IsHoanThanh()
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                return false;

            string tt = trangThai.Trim().ToLower();
            return tt.Contains("hoàn thành") || tt == "tt03";
        }

        public HopDongDTO()
        {
            maHD = string.Empty;
            maKH = string.Empty;
            TenKhachHang = string.Empty;
            DiaChiKhachHang = string.Empty;
            ngayKy = DateTime.Now;
            ngayKetThucHD = DateTime.Now;
            trangThai = string.Empty;
            tanSuatQuanTrac = string.Empty;
            soHD = string.Empty;
        }

        public HopDongDTO(string maHD, string maKH, string tenKH, DateTime ngayKy,
            DateTime ngayKetThuc, string trangThai, string tanSuat, string soHD)
        {
            this.maHD = maHD;
            this.maKH = maKH;
            this.TenKhachHang = tenKH;
            this.DiaChiKhachHang = string.Empty;
            this.ngayKy = ngayKy;
            this.ngayKetThucHD = ngayKetThuc;
            this.trangThai = trangThai;
            this.tanSuatQuanTrac = tanSuat;
            this.soHD = soHD;
        }

        // ✅ Constructor với địa chỉ khách hàng
        public HopDongDTO(string maHD, string maKH, string tenKH, string diaChiKH,
            DateTime ngayKy, DateTime ngayKetThuc, string trangThai, string tanSuat, string soHD)
        {
            this.maHD = maHD;
            this.maKH = maKH;
            this.TenKhachHang = tenKH;
            this.DiaChiKhachHang = diaChiKH;
            this.ngayKy = ngayKy;
            this.ngayKetThucHD = ngayKetThuc;
            this.trangThai = trangThai;
            this.tanSuatQuanTrac = tanSuat;
            this.soHD = soHD;
        }

        public override string ToString()
        {
            return $"HĐ: {maHD} - KH: {maKH} - {TenKhachHang} - Từ {ngayKy:dd/MM/yyyy} đến {ngayKetThucHD:dd/MM/yyyy}";
        }
    }
}