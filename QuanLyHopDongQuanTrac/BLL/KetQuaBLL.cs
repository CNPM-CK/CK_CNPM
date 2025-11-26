using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class KetQuaBLL
    {
        private DatabaseAccess db = new DatabaseAccess();

        // =============================================
        // HỆ THỐNG MỚI
        // =============================================

        /// <summary>
        /// Lấy danh sách kết quả (hiển thị trên dgvDanhsachketqua)
        /// </summary>
        public List<DTO_KetQuaHeader> LayDanhSachKetQuaMoi()
        {
            try
            {
                return db.LayDanhSachKetQuaMoi();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách kết quả: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách kết quả có phân trang
        /// </summary>
        public List<DTO_KetQuaHeader> layDanhSachKetQua_PhanTrang(int pageNumber, int pageSize)
        {
            try
            {
                return db.layDanhSachKetQua_PhanTrang(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách kết quả phân trang: " + ex.Message);
            }
        }

        /// <summary>
        /// Đếm tổng số kết quả
        /// </summary>
        public int demTongSoKetQua()
        {
            try
            {
                return db.demTongSoKetQua();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đếm số kết quả: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy chi tiết kết quả theo mã KQ
        /// </summary>
        public DTO_KetQuaFull LayChiTietKetQuaTheoMaKQ(string maKQ)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKQ))
                {
                    throw new Exception("Mã kết quả không được để trống!");
                }

                return db.LayChiTietKetQuaTheoMaKQ(maKQ);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết kết quả: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái xác nhận
        /// </summary>
        public (bool Success, string Message) CapNhatTrangThaiKetQua(string maKQ, bool trangThaiXacNhan)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKQ))
                {
                    return (false, "Mã kết quả không được để trống!");
                }

                return db.CapNhatTrangThaiKetQuaMoi(maKQ, trangThaiXacNhan);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi cập nhật trạng thái: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm mới kết quả
        /// </summary>
        public (bool Success, string Message, string MaKQ) ThemKetQuaMoi(string maDot, string nhanVienNhap, DateTime? ngayTraKQ, string ghiChu)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maDot))
                {
                    return (false, "Mã đợt không được để trống!", "");
                }

                if (string.IsNullOrWhiteSpace(nhanVienNhap))
                {
                    return (false, "Nhân viên nhập không được để trống!", "");
                }

                return db.ThemKetQuaHeader(maDot, nhanVienNhap, ngayTraKQ, ghiChu);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm kết quả: " + ex.Message, "");
            }
        }

        /// <summary>
        /// Thêm nền mẫu vào kết quả
        /// </summary>
        public (bool Success, string Message, string MaKQNen) ThemNenMauVaoKetQua(string maKQ, string maNen, string viTri, string toaDo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKQ))
                {
                    return (false, "Mã kết quả không được để trống!", "");
                }

                if (string.IsNullOrWhiteSpace(maNen))
                {
                    return (false, "Mã nền mẫu không được để trống!", "");
                }

                return db.ThemKetQuaNenMau(maKQ, maNen, viTri, toaDo);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm nền mẫu: " + ex.Message, "");
            }
        }

        /// <summary>
        /// Thêm chi tiết thông số đo
        /// </summary>
        public (bool Success, string Message) ThemChiTietThongSo(DTO_KetQuaChiTiet chiTiet)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(chiTiet.MaKQNen))
                {
                    return (false, "Mã kết quả nền mẫu không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(chiTiet.MaTS))
                {
                    return (false, "Mã thông số không được để trống!");
                }

                return db.ThemKetQuaChiTiet(chiTiet);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm chi tiết: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa kết quả
        /// </summary>
        public (bool Success, string Message) XoaKetQua(string maKQ)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKQ))
                {
                    return (false, "Mã kết quả không được để trống!");
                }

                return db.XoaKetQua(maKQ);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi xóa kết quả: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin tổng quan kết quả
        /// </summary>
        public DTO_KetQuaHeader LayThongTinKetQua(string maKQ)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKQ))
                {
                    throw new Exception("Mã kết quả không được để trống!");
                }

                return db.LayThongTinKetQua(maKQ);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin kết quả: " + ex.Message);
            }
        }

        // =============================================
        // HỆ THỐNG CŨ (Giữ lại để tương thích)
        // =============================================

        public List<DTO_BaoCao> LayDanhSachBaoCao()
        {
            try
            {
                return db.LayDanhSachBaoCao();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách báo cáo: " + ex.Message);
            }
        }

        // Giữ lại API cũ để không phá vỡ mã đang dùng (nếu còn)
        public List<DTO_KetQua> LayChiTietKetQuaTheoBC(string maBC)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maBC))
                {
                    throw new Exception("Mã báo cáo không được để trống!");
                }

                return db.LayChiTietKetQuaTheoBC(maBC);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết kết quả: " + ex.Message);
            }
        }
    }
}