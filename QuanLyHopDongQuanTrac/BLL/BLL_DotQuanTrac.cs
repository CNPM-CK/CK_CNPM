using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace BLL
{
    public class BLL_DotQuanTrac
    {
        private DatabaseAccess dal;

        public BLL_DotQuanTrac()
        {
            dal = new DatabaseAccess();
        }


        public List<HopDong> LayDanhSachHopDong()
        {
            try
            {
                return dal.LayDanhSachHopDong();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL - Lỗi lấy danh sách hợp đồng: " + ex.Message);
            }
        }


        public List<DTO_DotQuanTrac> LayDanhSachDotQuanTrac()
        {
            return dal.LayDanhSachQuanTrac();
        }


        public bool KiemTraHopDongTonTai(string maHD)
        {
            try
            {
                var dsHopDong = dal.LayDanhSachHopDong();
                return dsHopDong.Exists(hd => hd.MaHD == maHD);
            }
            catch
            {
                return false;
            }
        }


        public string TaoKeHoachNhap()
        {
            return dal.TaoDotQuanTracDraft();
        }


        public DTO_DotNen ThemNenMauVaoDot(string maDot, string maNen)
        {
            if (string.IsNullOrWhiteSpace(maDot))
                throw new ArgumentException("Mã đợt không hợp lệ!");

            if (string.IsNullOrWhiteSpace(maNen))
                throw new ArgumentException("Mã nền mẫu không hợp lệ!");

            return dal.ThemNenMauVaoDot(maDot, maNen);
        }


        public DataTable LayDanhSachTrangThai()
        {
            return dal.LayDanhSachTrangThai();
        }


        public bool LuuChiTietNenMau(
            string maDN,
            string tenViTri,
            string toaDo,
            string ghiChu,
            List<ChiTietQuanTracView> danhSachThongSo)
        {
            if (string.IsNullOrWhiteSpace(maDN))
                throw new Exception("Mã đợt nền không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tenViTri))
                throw new Exception("Vui lòng nhập tên vị trí!");

            if (danhSachThongSo == null || danhSachThongSo.Count == 0)
                throw new Exception("Phải có ít nhất một thông số!");

            foreach (var ts in danhSachThongSo)
            {
                if (string.IsNullOrWhiteSpace(ts.MaTS))
                    throw new Exception("Mã thông số không được để trống!");

                if (string.IsNullOrWhiteSpace(ts.DonVi))
                    throw new Exception($"Thông số '{ts.TenTS}' chưa có đơn vị!");

                if (ts.GiaTriToiThieu.HasValue && ts.GiaTriToiDa.HasValue)
                {
                    if (ts.GiaTriToiThieu.Value > ts.GiaTriToiDa.Value)
                        throw new Exception($"Thông số '{ts.TenTS}': Giá trị tối thiểu không được lớn hơn tối đa!");
                }
            }

            return dal.LuuChiTietNenMau(maDN, tenViTri, toaDo, ghiChu, danhSachThongSo);
        }


        //public DTO_DotNen LayThongTinDotNen(string maDN)
        //{
        //    return dal.LayThongTinDotNen(maDN);
        //}

        public (bool Success, string Message) XoaDotQuanTrac(string maDot)
        {
            if (string.IsNullOrWhiteSpace(maDot))
                return (false, "Mã đợt quan trắc không hợp lệ!");

            return dal.XoaDotQuanTrac(maDot);
        }


        public bool HoanTatKeHoachQuanTrac(DTO_DotQuanTrac dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaDot))
                throw new ArgumentException("Mã đợt không được để trống!");

            if (string.IsNullOrWhiteSpace(dto.MaHD))
                throw new ArgumentException("Phải chọn hợp đồng!");

            if (string.IsNullOrWhiteSpace(dto.DotQuanTrac))
                throw new ArgumentException("Tên đợt quan trắc không được để trống!");

            if (dto.NgayDuKien < dto.NgayBatDau)
                throw new ArgumentException("Ngày dự kiến phải >= ngày bắt đầu!");

            if (dto.NgayTraKQ.HasValue && dto.NgayTraKQ.Value < dto.NgayBatDau)
                throw new ArgumentException("Ngày trả kết quả phải >= ngày bắt đầu!");

            return dal.HoanTatKeHoachQuanTrac(dto);
        }
    }
}