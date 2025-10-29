using DAL;
using DTO;
using static BLL.ThongSoBLL;

namespace BLL
{
    public class ThongSoBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public bool ThemThongSo(ThongSo ts)
        {
            return dal.ThemThongSoMoiTruong(ts);
        }

        public List<ThongSo> LayDanhSachThongSo()
        {
            return dal.GetDanhSachThongSo();
        }


        public bool SuaThongSoMoiTruong(ThongSo ts)
        {
            return dal.SuaThongSoMoiTruong(ts);
        }


        public bool XoaThongSoMoiTruong(string maTS, out string ketQua)
        {
            return dal.XoaThongSoMoiTruong(maTS, out ketQua);
        }
    }
}
