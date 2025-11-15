using DAL;
using DTO;
using static BLL.ThongSoBLL;

namespace BLL
{
    public class ThongSoBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public bool themThongSo(ThongSo ts)
        {
            return dal.themThongSoMoiTruong(ts);
        }

        public List<ThongSo> layDanhSachThongSo()
        {
            return dal.layDanhSachThongSo();
        }

        public List<ThongSo> layDanhSachThongSo_PhanTrang(int pageNumber, int pageSize)
        {
            return dal.layDanhSachThongSo_PhanTrang(pageNumber, pageSize);
        }


        public bool suaThongSoMoiTruong(ThongSo ts)
        {
            return dal.suaThongSoMoiTruong(ts);
        }


        public bool xoaThongSoMoiTruong(string maTS, out string ketQua)
        {
            return dal.xoaThongSoMoiTruong(maTS, out ketQua);
        }

        public int demSoLuongThongSo()
        {
            return dal.demSoLuongThongSo();
        }
    }
}
