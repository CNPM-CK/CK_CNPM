using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class PhongBanBLL
    {
        public readonly DatabaseAccess dal = new DatabaseAccess();

        public List<PhongBan> LayDSPhongBan()
        {
            return dal.LayDSPhongBan();
        }


        public List<PhongBan> LayPTNvaPHT()
        {
            return dal.layPTNvaPHT();
        }
        public PhongBan LayPhongBanTheoMa(string maPhong)
        {
            return dal.LayPhongBanTheoMa(maPhong);
        }
    }
}
