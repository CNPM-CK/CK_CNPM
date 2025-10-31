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
            return dal.LayDanhSachHD();
        }
        public void ThemHopDong(HopDong hd)
        {
            dal.ThemHopDong(hd);
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
