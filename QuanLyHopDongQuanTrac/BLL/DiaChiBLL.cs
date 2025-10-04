using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class DiaChiBLL
    {
        private readonly DiaChiLuuTru repo;

        public DiaChiBLL()
        {
            repo = new DiaChiLuuTru();
        }

        public List<DiaChi> LayTinhThanh()
        {
            return repo.LayTinhThanh();
        }

        public List<DiaChi> LayQuanHuyen(string maTinh)
        {
            return repo.LayQuanHuyen(maTinh);
        }

        public List<DiaChi> LayXaPhuong(string maHuyen)
        {
            return repo.LayXaPhuong(maHuyen);
        }
    }
}
