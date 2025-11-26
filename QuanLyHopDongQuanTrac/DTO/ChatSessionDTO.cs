using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChatSessionDTO
    {
        public int MaPhien { get; set; }
        public string TenTK { get; set; }
        public string TenPhienChat { get; set; }
        public DateTime ThoiGianTao { get; set; }
        public DateTime ThoiGianSua { get; set; }
    }
}
