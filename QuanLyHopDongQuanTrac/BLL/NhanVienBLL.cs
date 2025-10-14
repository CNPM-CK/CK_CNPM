using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class NhanVienBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public List<NhanVien> LayDanhSachNhanVien()
        {
            return dal.LayDanhSachNhanVien();
        }

        public void ThemNhanVien(NhanVien nv,bool truongPhong)
        {
            dal.ThemNhanVien(nv, truongPhong);
        }


        public void SuaNhanVien(NhanVien nv, bool truongPhong)
        {
            dal.SuaNhanVien(nv, truongPhong);
        }


        public void XoaNhanVien(string maNV)
        {
            try
            {
                dal.XoaNhanVien(maNV);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message);
            }
        }


        public (bool daThayDoi, string logThayDoi) KiemTraThayDoi(NhanVien nvMoi, NhanVien nvCu, bool isTruongPhongMoi)
        {
            StringBuilder log = new StringBuilder();

            string ChuanHoaDiaChi(string dc)
            {
                if (string.IsNullOrWhiteSpace(dc)) return "";
                var parts = dc.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(p => p.Trim())
                              .Where(p => !string.IsNullOrEmpty(p));
                return string.Join(", ", parts);
            }

            if (nvMoi.hoTen?.Trim() != nvCu.hoTen?.Trim())
                log.AppendLine($"Họ tên khác: '{nvCu.hoTen}' → '{nvMoi.hoTen}'");

            if (nvMoi.email?.Trim() != nvCu.email?.Trim())
                log.AppendLine($"Email khác: '{nvCu.email}' → '{nvMoi.email}'");

            if (nvMoi.soDienThoai?.Trim() != nvCu.soDienThoai?.Trim())
                log.AppendLine($"SĐT khác: '{nvCu.soDienThoai}' → '{nvMoi.soDienThoai}'");

            if (nvMoi.gioiTinh != nvCu.gioiTinh)
                log.AppendLine($"Giới tính khác: '{nvCu.gioiTinh}' → '{nvMoi.gioiTinh}'");

            if (nvMoi.ngaySinh.Date != nvCu.ngaySinh.Date)
                log.AppendLine($"Ngày sinh khác: '{nvCu.ngaySinh:dd/MM/yyyy}' → '{nvMoi.ngaySinh:dd/MM/yyyy}'");

            if ((nvMoi.maPhong?.Trim() ?? "") != (nvCu.maPhong?.Trim() ?? ""))
                log.AppendLine($"Phòng ban khác: '{nvCu.maPhong}' → '{nvMoi.maPhong}'");

            string dcCu = ChuanHoaDiaChi(nvCu.diaChi);
            string dcMoi = ChuanHoaDiaChi(nvMoi.diaChi);
            if (dcMoi != dcCu)
                log.AppendLine($"Địa chỉ khác: '{dcCu}' → '{dcMoi}'");

            bool laTruongPhongCu = KiemTraTruongPhong(nvCu.maNV, nvCu.maPhong);
            if (isTruongPhongMoi != laTruongPhongCu)
                log.AppendLine($"Trạng thái trưởng phòng khác: '{laTruongPhongCu}' → '{isTruongPhongMoi}'");

            bool daThayDoi = log.Length > 0;
            return (daThayDoi, log.ToString());
        }



        private bool KiemTraTruongPhong(string maNV, string maPhong)
        {
            if (string.IsNullOrEmpty(maPhong)) return false;

            var phongBanBLL = new PhongBanBLL();
            var phongBan = phongBanBLL.LayPhongBanTheoMa(maPhong);

            return phongBan != null && phongBan.truongPhong == maNV;
        }
    }
}
