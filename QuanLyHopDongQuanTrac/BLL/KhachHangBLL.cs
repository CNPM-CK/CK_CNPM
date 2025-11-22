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

        public List<KhachHang> layDanhSachKH()
        {
            return dal.layDanhSachKH();
        }
        

        public void themKhachHang(KhachHang kh)
        {
            dal.themKhachHang(kh);
        }

        public void xoaKhachHang(string maKH) 
        {
            try
            {
                dal.xoaKhachHang(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa khách hàng: " + ex.Message);
            }
        }

        public List<TrangThaiKhachHang> layTrangThaiKhachHang()
        {
            return dal.layTrangThaiKhachHang();
        }

        public void suaKhachHang(KhachHang kh) 
        {
            dal.suaKhachHang(kh);
        }


        public (bool daThayDoi, string logThayDoi) kiemTraThayDoi(KhachHang khMoi,KhachHang khCu)
        {
            StringBuilder log = new StringBuilder();

            string chuanHoaDiaChi(string dc)
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

            string dcCu = chuanHoaDiaChi(khCu.diaChi);
            string dcMoi = chuanHoaDiaChi(khMoi.diaChi);
            if (dcMoi != dcCu)
                log.AppendLine($"Địa chỉ khác: '{dcCu}' → '{dcMoi}'");

            bool daThayDoi = log.Length > 0;
            return (daThayDoi, log.ToString());
        }


        public List<KhachHang> layDanhSachKH_PhanTrang(int pageNumber, int pageSize)
        {
            return dal.layDanhSachKH_PhanTrang(pageNumber, pageSize);
        }

        public int demTongSoKhachHang()
        {
            return dal.demTongSoKhachHang();
        }
        /// Lấy email khách hàng theo tên doanh nghiệp
        public string layEmailKhachHang(string tenDoanhNghiep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenDoanhNghiep))
                    return null;

                var danhSach = dal.layDanhSachKH();
                var khachHang = danhSach.FirstOrDefault(kh =>
                    kh.tenDoanhNghiep != null &&
                    kh.tenDoanhNghiep.Trim().Equals(tenDoanhNghiep.Trim(), StringComparison.OrdinalIgnoreCase));

                return khachHang?.emailDoanhNghiep;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy email khách hàng: " + ex.Message);
            }
        }


    }
}
