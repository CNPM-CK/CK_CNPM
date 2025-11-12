using System;
using DAL;
using DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NenMauNhapLieuBLL
    {
        private DatabaseAccess dal;
        public NenMauNhapLieuBLL()
        {
            dal = new DatabaseAccess();
        }

        public List<NenMauNhapLieuDTO> LayDanhSachNenMauNhapLieu(string maPhong, string maDot)
        {
            return dal.LayDanhSachNenMauNhapLieu(maPhong, maDot);
        }
    }
}
