using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class KhachHangBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public List<KhachHang> LayDanhSachKH()
        {
            return dal.LayDanhSachKH();
        }
        

        public void ThemKhachHang(KhachHang kh)
        {
            dal.ThemKhachHang(kh);
        }

        public void XoaKhachHang(string maKH) 
        {
            try
            {
                dal.XoaKhachHang(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa khách hàng: " + ex.Message);
            }
        }

        public void SuaKhachHang(KhachHang kh) 
        {
            dal.SuaKhachHang(kh);
        }


        public (bool daThayDoi, string logThayDoi) KiemTraThayDoi(KhachHang khMoi,KhachHang khCu)
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

            if (khMoi.tenDoanhNghiep?.Trim() != khCu.tenDoanhNghiep?.Trim())
                log.AppendLine($"Tên doanh nghiệp khác: '{khCu.tenDoanhNghiep}' → '{khMoi.tenDoanhNghiep}'");

            if (khMoi.kyHieuDN?.Trim() != khCu.kyHieuDN?.Trim())
                log.AppendLine($"kyHieuDN khác: '{khCu.kyHieuDN}' → '{khMoi.kyHieuDN}'");

            if (khMoi.soDienThoaiKH?.Trim() != khCu.soDienThoaiKH?.Trim())
                log.AppendLine($"SĐT khác: '{khCu.soDienThoaiKH}' → '{khMoi.soDienThoaiKH}'");

            if ((khMoi.nguoiDaiDien?.Trim() ?? "") != (khCu.nguoiDaiDien?.Trim() ?? ""))
                log.AppendLine($"Người đại diện khác: '{khCu.nguoiDaiDien}' → '{khMoi.nguoiDaiDien}'");

            string dcCu = ChuanHoaDiaChi(khCu.diaChi);
            string dcMoi = ChuanHoaDiaChi(khMoi.diaChi);
            if (dcMoi != dcCu)
                log.AppendLine($"Địa chỉ khác: '{dcCu}' → '{dcMoi}'");

            bool daThayDoi = log.Length > 0;
            return (daThayDoi, log.ToString());
        }

    }
}
