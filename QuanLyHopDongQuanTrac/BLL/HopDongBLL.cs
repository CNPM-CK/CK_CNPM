using DAL;
using DTO;
using System;
using System.Collections.Generic;
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
    }
}
