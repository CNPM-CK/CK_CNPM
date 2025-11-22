using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class HopDongBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public List<HopDongDTO> LayDanhSachHD()
        {
            return dal.layDanhSachHD();
        }

        /// <summary>
        /// Lấy danh sách hợp đồng dạng DTO_HopDong cho form QuanLyHopDongChuKy
        /// </summary>
        public List<DTO_HopDong> LayDanhSachHopDong()
        {
            try
            {
                // Lấy danh sách HopDongDTO và chuyển đổi sang DTO_HopDong
                var danhSachDTO = dal.layDanhSachHD();

                if (danhSachDTO == null)
                    return new List<DTO_HopDong>();

                return danhSachDTO.Select(hd => new DTO_HopDong
                {
                    MaHD = hd.maHD,
                    MaKH = hd.maKH,
                    TenKhachHang = hd.TenKhachHang,
                    DiaChiKhachHang = hd.DiaChiKhachHang,
                    NgayKy = hd.ngayKy,
                    NgayKetThuc = hd.ngayKetThucHD,
                    TrangThai = hd.trangThai,
                    TanSuatQuanTrac = hd.tanSuatQuanTrac,
                    SoHD = hd.soHD
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL.LayDanhSachHopDong: " + ex.Message);
            }
        }

        public void ThemHopDong(HopDongDTO hd)
        {
            dal.ThemHopDong(hd);
        }

        public void suaHopDong(HopDongDTO hd)
        {
            dal.suaHopDong(hd);
        }

        public void XoaNhanVien(string maNV)
        {
            try
            {
                dal.xoaNhanVien(maNV);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message);
            }
        }

        public (bool daThayDoi, string logThayDoi) KiemTraThayDoi(HopDongDTO hdMoi, HopDongDTO hdCu)
        {
            StringBuilder log = new StringBuilder();

            if (hdMoi.maKH?.Trim() != hdCu.maKH?.Trim())
                log.AppendLine($"Khách hàng khác: '{hdCu.maKH}' → '{hdMoi.maKH}'");

            if (hdMoi.ngayKy.Date != hdCu.ngayKy.Date)
                log.AppendLine($"Ngày ký khác: '{hdCu.ngayKy:dd/MM/yyyy}' → '{hdMoi.ngayKy:dd/MM/yyyy}'");

            if (hdMoi.ngayKetThucHD.Date != hdCu.ngayKetThucHD.Date)
                log.AppendLine($"Ngày kết thúc hợp đồng khác: '{hdCu.ngayKetThucHD:dd/MM/yyyy}' → '{hdMoi.ngayKetThucHD:dd/MM/yyyy}'");

            if (hdMoi.trangThai?.Trim() != hdCu.trangThai?.Trim())
                log.AppendLine($"Trạng thái khác: '{hdCu.trangThai}' → '{hdMoi.trangThai}'");

            if (hdMoi.tanSuatQuanTrac?.Trim() != hdCu.tanSuatQuanTrac?.Trim())
                log.AppendLine($"Tần suất quan trắc khác: '{hdCu.tanSuatQuanTrac}' → '{hdMoi.tanSuatQuanTrac}'");

            if (hdMoi.soHD?.Trim() != hdCu.soHD?.Trim())
                log.AppendLine($"Số hợp đồng khác: '{hdCu.soHD}' → '{hdMoi.soHD}'");

            bool daThayDoi = log.Length > 0;
            return (daThayDoi, log.ToString());
        }

        public DataTable layTrangThaiHopDong()
        {
            try
            {
                return dal.layTrangThaiHopDong();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL.layTrangThaiHopDong: " + ex.Message);
            }
        }

        public List<HopDongDTO> layDanhSachHopDong_PhanTrang(int pageNumber, int pageSize)
        {
            return dal.layDanhSachHopDong_PhanTrang(pageNumber, pageSize);
        }

        public int demSoLuongHopDong()
        {
            return dal.demSoLuongHopDong();
        }
    }
}