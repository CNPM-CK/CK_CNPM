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
        public List<HopDong> LayDanhSachHD()
        {
            return dal.layDanhSachHD();
        }
        public void ThemHopDong(HopDong hd)
        {
            dal.ThemHopDong(hd);
        }


        public void SuaNhanVien(NhanVien nv, bool truongPhong)
        {
            dal.suaNhanVien(nv, truongPhong);
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
    }
}
