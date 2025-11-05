using System;
using System.Collections.Generic;

namespace DTO
{
    // DTO cho Header của kết quả (hiển thị trong danh sách)
    public class DTO_KetQuaHeader
    {
        public string MaKQ { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayTraKQ { get; set; }
        public string TenNhanVien { get; set; }
        public bool TrangThaiXacNhan { get; set; }
        public string TrangThai
        {
            get
            {
                return TrangThaiXacNhan ? "Đã xác nhận" : "Chưa xác nhận";
            }
            set { }
        }
        public string GhiChu { get; set; }
        public string DotQuanTrac { get; set; }
        public string MaDot { get; set; }
        public int SoNenMau { get; set; }

        public DTO_KetQuaHeader()
        {
            MaKQ = string.Empty;
            NgayTao = DateTime.Now;
            TenNhanVien = string.Empty;
            TrangThaiXacNhan = false;
            GhiChu = string.Empty;
            DotQuanTrac = string.Empty;
            MaDot = string.Empty;
            SoNenMau = 0;
        }
    }

    // DTO cho nền mẫu trong kết quả
    public class DTO_KetQuaNenMau
    {
        public string MaKQNen { get; set; }
        public string MaKQ { get; set; }
        public string MaNen { get; set; }
        public string TenNenMau { get; set; }
        public string ViTri { get; set; }
        public string ToaDo { get; set; }
        public List<DTO_KetQuaChiTiet> DanhSachThongSo { get; set; }

        public DTO_KetQuaNenMau()
        {
            MaKQNen = string.Empty;
            MaKQ = string.Empty;
            MaNen = string.Empty;
            TenNenMau = string.Empty;
            ViTri = string.Empty;
            ToaDo = string.Empty;
            DanhSachThongSo = new List<DTO_KetQuaChiTiet>();
        }
    }

    // DTO cho chi tiết thông số trong kết quả
    public class DTO_KetQuaChiTiet
    {
        public string MaKQCT { get; set; }
        public string MaKQNen { get; set; }
        public string MaTS { get; set; }
        public string TenTS { get; set; }
        public string DonVi { get; set; }
        public string PhuongPhapPhanTich { get; set; }
        public double KetQua { get; set; }
        public string GioiHanPhatHien { get; set; }
        public string QCVN { get; set; }
        public string TinhTrang { get; set; }

        public DTO_KetQuaChiTiet()
        {
            MaKQCT = string.Empty;
            MaKQNen = string.Empty;
            MaTS = string.Empty;
            TenTS = string.Empty;
            DonVi = string.Empty;
            PhuongPhapPhanTich = string.Empty;
            KetQua = 0;
            GioiHanPhatHien = string.Empty;
            QCVN = string.Empty;
            TinhTrang = string.Empty;
        }
    }

    // DTO tổng hợp đầy đủ thông tin kết quả
    public class DTO_KetQuaFull
    {
        public DTO_KetQuaHeader Header { get; set; }
        public List<DTO_KetQuaNenMau> DanhSachNenMau { get; set; }

        public DTO_KetQuaFull()
        {
            Header = new DTO_KetQuaHeader();
            DanhSachNenMau = new List<DTO_KetQuaNenMau>();
        }
    }

    // DTO cho báo cáo (giữ lại để tương thích)
    public class DTO_BaoCao
    {
        public string MaBC { get; set; }
        public string MaDot { get; set; }
        public string TenNguoiXuat { get; set; }
        public DateTime NgayXuat { get; set; }
        public int SoNenMau { get; set; }
        public int TongSoThongSo { get; set; }
        public string TrangThai { get; set; }

        public DTO_BaoCao()
        {
            MaBC = string.Empty;
            MaDot = string.Empty;
            TenNguoiXuat = string.Empty;
            NgayXuat = DateTime.Now;
            SoNenMau = 0;
            TongSoThongSo = 0;
            TrangThai = string.Empty;
        }
    }
}