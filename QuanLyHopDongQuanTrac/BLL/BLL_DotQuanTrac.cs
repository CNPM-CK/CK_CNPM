using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace BLL
{
    public class BLL_DotQuanTrac
    {
        private DatabaseAccess dal;


        public BLL_DotQuanTrac()
        {
            dal = new DatabaseAccess();
        }


        public List<HopDongVaTenDN> layDanhSachHopDong()
        {
            try
            {
                return dal.layDanhSachHopDong();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL - Lỗi lấy danh sách hợp đồng: " + ex.Message);
            }
        }


        public List<DTO_DotQuanTrac> layDanhSachDotQuanTrac()
        {
            return dal.layDanhSachQuanTrac();
        }


        public bool kiemTraHopDongTonTai(string maHD)
        {
            try
            {
                var dsHopDong = dal.layDanhSachHD();
                return dsHopDong.Exists(hd => hd.maHD == maHD);
            }
            catch
            {
                return false;
            }
        }


        public string taoKeHoachNhap()
        {
            return dal.taoDotQuanTracNhap();
        }


        public DTO_DotNen themNenMauVaoDot(string maDot, string maNen)
        {
            if (string.IsNullOrWhiteSpace(maDot))
                throw new ArgumentException("Mã đợt không hợp lệ!");

            if (string.IsNullOrWhiteSpace(maNen))
                throw new ArgumentException("Mã nền mẫu không hợp lệ!");

            return dal.themNenMauVaoDot(maDot, maNen);
        }


        public DataTable LayDanhSachTrangThai()
        {
            return dal.layDanhSachTrangThai();
        }


        public bool luuChiTietNenMau(
            string maDN,
            string tenViTri,
            string toaDo,
            string ghiChu,
            List<ChiTietQuanTracView> danhSachThongSo)
        {
            if (string.IsNullOrWhiteSpace(maDN))
                throw new Exception("Mã đợt nền không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tenViTri))
                throw new Exception("Vui lòng nhập tên vị trí!");

            if (danhSachThongSo == null || danhSachThongSo.Count == 0)
                throw new Exception("Phải có ít nhất một thông số!");

            foreach (var ts in danhSachThongSo)
            {
                if (string.IsNullOrWhiteSpace(ts.MaTS))
                    throw new Exception("Mã thông số không được để trống!");

                if (string.IsNullOrWhiteSpace(ts.DonVi))
                    throw new Exception($"Thông số '{ts.TenTS}' chưa có đơn vị!");

                if (ts.GiaTriToiThieu.HasValue && ts.GiaTriToiDa.HasValue)
                {
                    if (ts.GiaTriToiThieu.Value > ts.GiaTriToiDa.Value)
                        throw new Exception($"Thông số '{ts.TenTS}': Giá trị tối thiểu không được lớn hơn tối đa!");
                }
            }

            return dal.luuChiTietNenMau(maDN, tenViTri, toaDo, ghiChu, danhSachThongSo);
        }

        public bool suaChiTietNenMau(string maDN, string tenViTri, string toaDo, string ghiChu, List<ChiTietQuanTracView> danhSachThongSo)
        {
            // VALIDATION
            if (string.IsNullOrWhiteSpace(maDN))
                throw new ArgumentException("Mã đợt nền không được để trống!");

            if (string.IsNullOrWhiteSpace(tenViTri))
                throw new ArgumentException("Tên vị trí không được để trống!");

            if (danhSachThongSo == null || danhSachThongSo.Count == 0)
                throw new ArgumentException("Phải có ít nhất một thông số!");

            // Kiểm tra từng thông số
            for (int i = 0; i < danhSachThongSo.Count; i++)
            {
                var ts = danhSachThongSo[i];

                if (string.IsNullOrWhiteSpace(ts.MaTS))
                    throw new ArgumentException($"Thông số thứ {i + 1}: Thiếu mã thông số (maTS)!");

                if (ts.GiaTriToiThieu.HasValue && ts.GiaTriToiDa.HasValue)
                {
                    if (ts.GiaTriToiThieu.Value > ts.GiaTriToiDa.Value)
                        throw new ArgumentException($"Thông số thứ {i + 1}: Giá trị tối thiểu không được lớn hơn giá trị tối đa!");
                }
            }

            // Kiểm tra trùng maTS
            var duplicateTS = danhSachThongSo
                .GroupBy(x => x.MaTS)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(duplicateTS))
                throw new ArgumentException($"Thông số '{duplicateTS}' bị trùng lặp trong danh sách!");

            return dal.suaChiTietNenMau(maDN, tenViTri, toaDo, ghiChu, danhSachThongSo);
        }


        public (bool Success, string Message) xoaDotQuanTrac(string maDot)
        {
            if (string.IsNullOrWhiteSpace(maDot))
                return (false, "Mã đợt quan trắc không hợp lệ!");

            return dal.xoaDotQuanTrac(maDot);
        }


        public bool hoanTatKeHoachQuanTrac(DTO_DotQuanTrac dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaDot))
                throw new ArgumentException("Mã đợt không được để trống!");

            if (string.IsNullOrWhiteSpace(dto.MaHD))
                throw new ArgumentException("Phải chọn hợp đồng!");

            if (string.IsNullOrWhiteSpace(dto.DotQuanTrac))
                throw new ArgumentException("Tên đợt quan trắc không được để trống!");

            if (dto.NgayDuKien < dto.NgayBatDau)
                throw new ArgumentException("Ngày dự kiến phải >= ngày bắt đầu!");

            if (dto.NgayTraKQ.HasValue && dto.NgayTraKQ.Value < dto.NgayBatDau)
                throw new ArgumentException("Ngày trả kết quả phải >= ngày bắt đầu!");

            return dal.hoanTatKeHoachQuanTrac(dto);
        }


        public List<DTO_DotQuanTrac> layDanhSachDotQuanTrac_PhanTrang(int pageNumber, int pageSize)
        {
            return dal.layDanhSachDotQuanTrac_PhanTrang(pageNumber, pageSize);
        }

        public int demTongKHQT()
        {
            return dal.demTongKHQT();
        }

        public bool xoaNenMauKhoiDot(string maDN)
        {
            if (string.IsNullOrWhiteSpace(maDN))
                throw new ArgumentException("Mã đợt nền không hợp lệ!");
            return dal.xoaNenMauKhoiDot(maDN);
        }


        public class ChiTietDotQuanTracDTO
        {
            public DTO_DotQuanTrac ThongTinDot { get; set; }
            public List<NenMauTrongDot> DanhSachNenMau { get; set; }
        }

        public class NenMauTrongDot
        {
            public string MaDN { get; set; }
            public string MaNen { get; set; }
            public string TenNenMau { get; set; }
            public string MoTaNen { get; set; }
            public string TenViTri { get; set; }
            public string ToaDo { get; set; }
            public string GhiChu { get; set; }
            public List<ChiTietQuanTracView> DanhSachThongSo { get; set; }
        }

        public ChiTietDotQuanTracDTO LayChiTietDotQuanTrac(string maDot)
        {
            try
            {
                DataSet ds = dal.layChiTietDotQuanTrac(maDot);

                if (ds == null || ds.Tables.Count < 3)
                    return null;

                ChiTietDotQuanTracDTO result = new ChiTietDotQuanTracDTO();

                // ✅ Bảng 0: Thông tin đợt
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    result.ThongTinDot = new DTO_DotQuanTrac
                    {
                        MaDot = row["maDot"].ToString(),
                        MaHD = row["maHD"].ToString(),
                        NoiDung = row["noiDung"].ToString(),
                        DotQuanTrac = row["dotQuanTrac"].ToString(),
                        NgayBatDau = Convert.ToDateTime(row["ngayBatDau"]),
                        NgayDuKien = Convert.ToDateTime(row["ngayDuKien"]),
                        NgayTraKQ = row["ngayTraKQ"] != DBNull.Value
                            ? Convert.ToDateTime(row["ngayTraKQ"])
                            : (DateTime?)null,
                        TrangThai = row["trangThai"].ToString()
                    };
                }

                // ✅ Bảng 1: Danh sách nền mẫu
                result.DanhSachNenMau = new List<NenMauTrongDot>();

                foreach (DataRow rowNen in ds.Tables[1].Rows)
                {
                    string maDN = rowNen["maDN"].ToString();

                    var nenMau = new NenMauTrongDot
                    {
                        MaDN = maDN,
                        MaNen = rowNen["maNen"].ToString(),
                        TenNenMau = rowNen["tenNenMau"].ToString(),
                        MoTaNen = rowNen["moTa"].ToString(),
                        TenViTri = rowNen["tenViTri"].ToString(),
                        ToaDo = rowNen["toaDo"].ToString(),
                        GhiChu = rowNen["ghiChu"].ToString(),
                        DanhSachThongSo = new List<ChiTietQuanTracView>()
                    };

                    // ✅ Bảng 2: Lấy thông số của nền mẫu này
                    DataRow[] thongSoRows = ds.Tables[2].Select($"maDN = '{maDN}'");

                    foreach (DataRow rowTS in thongSoRows)
                    {
                        nenMau.DanhSachThongSo.Add(new ChiTietQuanTracView
                        {
                            MaTS = rowTS["maTS"].ToString(),
                            TenTS = rowTS["tenTS"].ToString(),
                            DonVi = rowTS["donVi"].ToString(),
                            GiaTriToiThieu = rowTS["giaTriToiThieu"] != DBNull.Value
                                ? Convert.ToDouble(rowTS["giaTriToiThieu"])
                                : (double?)null,
                            GiaTriToiDa = rowTS["giaTriToiDa"] != DBNull.Value
                                ? Convert.ToDouble(rowTS["giaTriToiDa"])
                                : (double?)null,
                            PhuongPhap = rowTS["phuongPhap"].ToString(),
                            MaPhong = rowTS["maPhong"].ToString(),
                            TenPhong = rowTS["tenPhong"].ToString()
                        });
                    }

                    result.DanhSachNenMau.Add(nenMau);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL_LayChiTietDotQuanTrac: " + ex.Message);
            }
        }
    }
}