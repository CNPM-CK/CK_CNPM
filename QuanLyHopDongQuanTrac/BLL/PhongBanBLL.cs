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

        public List<PhongBan> layDSPhongBan()
        {
            return dal.layDSPhongBan();
        }


        public List<PhongBan> layPTNvaPHT()
        {
            return dal.layPTNvaPHT();
        }
        public PhongBan layPhongBanTheoMa(string maPhong)
        {
            return dal.layPhongBanTheoMa(maPhong);
        }
    }
}
