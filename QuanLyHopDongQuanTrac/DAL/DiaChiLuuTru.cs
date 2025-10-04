using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAL
{
    public class DiaChiLuuTru
    {

        private string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        // Load danh sách tỉnh
        public List<DiaChi> LayTinhThanh()
        {
            string path = Path.Combine(basePath, "tinh_tp.json");
            return TaiLen(path);
        }

        // Load quận huyện theo mã tỉnh
        public List<DiaChi> LayQuanHuyen(string maTinh)
        {
            string path = Path.Combine(basePath, "quan_huyen.json");
            var allQuan = TaiLen(path);
            return allQuan.Where(q => q.parent_code == maTinh).ToList();
        }

        public List<DiaChi> LayXaPhuong(string maHuyen)
        {
            string path = Path.Combine(basePath, "xa_phuong.json");
            var allXa = TaiLen(path);
            return allXa.Where(x => x.parent_code == maHuyen).ToList();
        }


        private List<DiaChi> TaiLen(string path)
        {
            if (!File.Exists(path)) return new List<DiaChi>();
            var json = File.ReadAllText(path);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, DiaChi>>(json);
            return dict.Values.ToList();
        }
    }


}

