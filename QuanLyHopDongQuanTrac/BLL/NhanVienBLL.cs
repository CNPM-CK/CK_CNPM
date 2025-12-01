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

        public List<NhanVien> layDanhSachNhanVien()
        {
            return dal.layDanhSachNhanVien();
        }

        public List<NhanVienSearch> layDanhSachNhanVien_TimKiem()
        {
            return dal.layDanhSachNhanVien_TimKiem();
        }

        public void themNhanVien(NhanVien nv,bool truongPhong)
        {
            dal.themNhanVien(nv, truongPhong);
        }


        public void suaNhanVien(NhanVien nv, bool truongPhong)
        {
            dal.suaNhanVien(nv, truongPhong);
        }


        public void xoaNhanVien(string maNV)
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


        public (bool daThayDoi, string logThayDoi) kiemTraThayDoi(NhanVien nvMoi, NhanVien nvCu, bool isTruongPhongMoi)
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

            bool laTruongPhongCu = kiemTraTruongPhong(nvCu.maNV, nvCu.maPhong);
            if (isTruongPhongMoi != laTruongPhongCu)
                log.AppendLine($"Trạng thái trưởng phòng khác: '{laTruongPhongCu}' → '{isTruongPhongMoi}'");

            bool daThayDoi = log.Length > 0;
            return (daThayDoi, log.ToString());
        }

        private bool kiemTraTruongPhong(string maNV, string maPhong)
        {
            if (string.IsNullOrEmpty(maPhong)) return false;

            var phongBanBLL = new PhongBanBLL();
            var phongBan = phongBanBLL.layPhongBanTheoMa(maPhong);

            return phongBan != null && phongBan.truongPhong == maNV;
        }


        public string layPhongBanTheoTaiKhoan(string tenTK)
        {
            return dal.layPhongBanTheoTaiKhoan(tenTK);
        }

        public List<TrangThaiNhanVien> layDanhSachTrangThai()
        {
            return dal.layTrangThaiNhanVien();
        }

        public List<NhanVien> layDanhSachNhanVien_PhanTrang(int pageNumber, int pageSize)
        {
            return dal.layDanhSachNhanVien_PhanTrang(pageNumber, pageSize);
        }

        public int demSoLuongNhanVien()
        {
            return dal.demTongDSNV();
        }

        public NhanVien layThongTinCaNhan(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email không được để trống.");

            return dal.layThongTinCaNhan(email);
        }


        public void capNhatThongTinCaNhan(NhanVien nv)
        {
            if (nv == null) throw new ArgumentNullException(nameof(nv));
            if (string.IsNullOrEmpty(nv.maNV))
                throw new ArgumentException("Ma nhan vien khong duoc de trong");

            dal.capNhatThongTinCaNhan(nv);
        }


        public NhanVien layNhanVienTheoMa(string maNV)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống!");
            }

            try
            {
                NhanVien nv = dal.layNhanVienTheoMa(maNV);

                if (nv == null)
                {
                    throw new Exception($"Không tìm thấy nhân viên có mã: {maNV}");
                }

                return nv;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL - Lấy nhân viên theo mã: " + ex.Message);
            }
        }
    }
}
