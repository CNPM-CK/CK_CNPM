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
    }
}
