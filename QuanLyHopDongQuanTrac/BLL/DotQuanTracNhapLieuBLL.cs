using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DotQuanTracNhapLieuBLL
    {
        private DatabaseAccess dal;
        public DotQuanTracNhapLieuBLL()
        {
            dal = new DatabaseAccess();
        }

        public List<DanhSachDotNhapLieuDTO> layDanhSachDotQuanTracNhapLieu_PhanTrang(int pageNumber, int pageSize, string maPhong)
        {
            return dal.layDanhSachDotQuanTracNhapLieu_PhanTrang(pageNumber, pageSize, maPhong);
        }
        public int demTongKHQT()
        {
            return dal.demTongKHQT();
        }
        public DotNenThongSoNhapLieuDTO LayDotNenTheoMaDotNen(string maDN)
        {
            return dal.LayDotNenTheoMaDotNen(maDN);
        }

        public List<ThongSoNhapLieuDTO> LayDanhSachThongSoTheoDotNenVaPhong(string maDN, string maPhong)
        {
            return dal.LayDanhSachThongSoTheoDotNenVaPhong(maDN, maPhong);
        }
        public DTO_DotNenTs LayThongSoTheoMaDotNenTS(string maDNTS)
        {
            return dal.LayThongSoTheoMaDotNenTS(maDNTS);
        }
        public NhanVien LayNhanVienTheoTenDN(string userName)
        {
            return dal.LayNhanVienTheoTenDN(userName);
        }
        public void ThemKetQua(KetQua kq)
        {
            dal.ThemKetQua(kq);
        }

    }
}
