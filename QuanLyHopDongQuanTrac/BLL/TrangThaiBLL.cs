using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TrangThaiBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public List<TrangThaiHDDTO> layDanhSachTrangThaiHD()
        {
            return dal.layDanhSachTrangThaiHD();
        }
    }
}
