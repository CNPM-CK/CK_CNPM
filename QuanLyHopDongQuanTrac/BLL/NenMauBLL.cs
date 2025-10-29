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

        public string ThemNenMau(string tenNenMau, string moTa)
        {
            if (string.IsNullOrWhiteSpace(tenNenMau))
                throw new Exception("Tên nền mẫu không được để trống!");

            if (string.IsNullOrWhiteSpace(moTa))
                throw new Exception("Mô tả không được để trống!");

            return dal.ThemNenMau(tenNenMau, moTa);
        }



        public List<NenMau> LayDSNenMau()
        {
            return dal.GetDanhSachNenMau();
        }

        public void XoaNenMau(string maNen)
        {
            try
            {
                dal.XoaNenMau(maNen);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message);
            }
        }
    }
}
