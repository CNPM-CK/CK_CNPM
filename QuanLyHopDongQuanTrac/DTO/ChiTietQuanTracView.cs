
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
        public class ChiTietQuanTracView
        {
            public string MaNen { get; set; }      
            public string MaTS { get; set; }       
            public string TenTS { get; set; }      
            public string DonVi { get; set; }    
            public double? GiaTriToiThieu { get; set; }
            public double? GiaTriToiDa { get; set; }
            public string MaPhong { get; set; }    
            public string TenPhong { get; set; }
            public string PhuongPhap { get; set; }

    }

}