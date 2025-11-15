using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NenMauBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public string themNenMau(string tenNenMau, string moTa)
        {
            if (string.IsNullOrWhiteSpace(tenNenMau))
                throw new Exception("Tên nền mẫu không được để trống!");

            if (string.IsNullOrWhiteSpace(moTa))
                throw new Exception("Mô tả không được để trống!");

            return dal.themNenMau(tenNenMau, moTa);
        }



        public List<NenMau> layDSNenMau()
        {
            return dal.layDanhSachNenMau();
        }

        public List<NenMau> layDanhSachNenMau_PhanTrang(int pageNumber, int pageSize, string keyword = "")
        {
            return dal.layDanhSachNenMau_PhanTrang(pageNumber, pageSize, keyword);
        }

        public bool suaNenMau(string maNen, string moTa)
        {
            return dal.suaNenMau(maNen, moTa);
        }

        public void xoaNenMau(string maNen)
        {
            try
            {
                dal.xoaNenMau(maNen);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message);
            }
        }

        public int demSoLuongNenMau()
        {
            return dal.demSoLuongNenMau();
        }
    }
}
