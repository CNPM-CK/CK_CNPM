using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TanSuatQTBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        public List<TanSuatQTDTO> LayDanhSachTSQT()
        {
            return dal.LayDanhSachTSQT();
        }
    }
}
