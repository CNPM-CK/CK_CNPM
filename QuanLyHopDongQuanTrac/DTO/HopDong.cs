using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDong
    {
        public string maHD { get; set; }
        public string maKH { get; set; }

        public DateTime ngayKy { get; set; }
        public DateTime ngayKetThucHD { get; set; }

        public string trangThai { get; set; }
        public string tanSuatQuanTrac { get; set; }
        public string soHD { get; set; }

    }
}
