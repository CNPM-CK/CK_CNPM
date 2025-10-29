using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
namespace DAL
{
    public class SqlConnectionData
    {
        public static SqlConnection Connect()
        {

            string connectionStr = "Data Source=ThaiQuangTran\\SQLEXPRESS;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";


            //string connectionStr = "Data Source=PTT;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            //string connectionStr = "Data Source=LAPTOP-61AGFMMJ\\TONTHAI;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            SqlConnection conn = new SqlConnection(connectionStr);
            return conn;
        }
    }
    public class DatabaseAccess
    {
        public TaiKhoan? KiemTraDangNhap(string username)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("layTaikhoan", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenTK", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaiKhoan
                            {
                                tenTK = reader.GetString(0),
                                matKhau = reader.GetString(1),
                                vaiTro = reader.GetBoolean(2) ? 1 : 0

                            };
                        }
                    }

                }
            }
            return null;
        }


        public List<NhanVien> LayDanhSachNhanVien() {

            List<NhanVien> dsNhanvien = new List<NhanVien>();
            using (SqlConnection conn = SqlConnectionData.Connect())
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("layDanhSachNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        while (reader.Read())


                        {
                            string gioiTinh = "0";
                            if (reader["gioiTinh"] != DBNull.Value)
                            {
                                var gioiTinhValue = reader["gioiTinh"];

                                // Nếu là bool/bit
                                if (gioiTinhValue is bool boolValue)
                                {
                                    gioiTinh = boolValue ? "1" : "0";
                                }
                                // Nếu là string
                                else
                                {
                                    string strValue = gioiTinhValue.ToString().Trim().ToLower();
                                    if (strValue == "1" || strValue == "true" || strValue == "nữ" || strValue == "nu")
                                        gioiTinh = "1";
                                    else
                                        gioiTinh = "0";
                                }
                            }

                            var nv = new NhanVien
                            {
                                maNV = reader["maNV"].ToString(),
                                maPhong = reader["maPhong"].ToString(),
                                tenPhong = reader["tenPhong"].ToString(),
                                hoTen = reader["hoTen"].ToString(),
                                ngaySinh = Convert.ToDateTime(reader["ngaySinh"]),
                                gioiTinh = gioiTinh,
                                diaChi = reader["diaChi"].ToString(),
                                email = reader["email"].ToString(),
                                soDienThoai = reader["soDienThoai"].ToString()

                            };
                            dsNhanvien.Add(nv);

                        }

                    }

                }

            }
            return dsNhanvien;

        }
        public List<KhachHang> LayDanhSachKH()
        {

            List<KhachHang> dsKhachhang = new List<KhachHang>();
            using (SqlConnection conn = SqlConnectionData.Connect())
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("LayDSKH", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachHang kh = new KhachHang();
                            kh.maKH = reader["maKH"].ToString();
                            kh.tenDoanhNghiep = reader["tenDoanhNghiep"].ToString();
                            kh.kyHieuDN = reader["kyHieuDN"] == DBNull.Value ? null : reader["kyHieuDN"].ToString();
                            kh.diaChi = reader["diaChi"].ToString();
                            kh.nguoiDaiDien = reader["nguoiDaiDien"].ToString();
                            kh.soDienThoaiKH = reader["soDienThoaiKH"].ToString();
                            dsKhachhang.Add(kh);
                        }
                    }
                }
            }
            return dsKhachhang;
        }


        public void ThemNhanVien(NhanVien nv, bool isTruongPhong)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ThemNhanVien", conn)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maPhong", nv.maPhong);    
                    cmd.Parameters.AddWithValue("@hoTen", nv.hoTen);
                    cmd.Parameters.AddWithValue("@ngaySinh", nv.ngaySinh);
                    cmd.Parameters.AddWithValue("@gioiTinh", nv.gioiTinh);
                    cmd.Parameters.AddWithValue("@diaChi", nv.diaChi);
                    cmd.Parameters.AddWithValue("@soDienThoai", nv.soDienThoai);
                    cmd.Parameters.AddWithValue("@email", nv.email);
                    cmd.Parameters.AddWithValue("@isTruongPhong", isTruongPhong ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void ThemKhachHang(KhachHang kh)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.ThemKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tenDoanhNghiep", kh.tenDoanhNghiep);
                    cmd.Parameters.AddWithValue("@kyHieuDN", string.IsNullOrEmpty(kh.kyHieuDN) ? (object)DBNull.Value : kh.kyHieuDN);
                    cmd.Parameters.AddWithValue("@diaChi", kh.diaChi);
                    cmd.Parameters.AddWithValue("@nguoiDaiDien", kh.nguoiDaiDien);
                    cmd.Parameters.AddWithValue("@soDienThoaiKH", kh.soDienThoaiKH);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SuaNhanVien(NhanVien nv, bool isTruongPhong)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SuaNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maNV", nv.maNV);
                    cmd.Parameters.AddWithValue("@maPhong", nv.maPhong);
                    cmd.Parameters.AddWithValue("@hoTen", nv.hoTen);
                    cmd.Parameters.AddWithValue("@ngaySinh", nv.ngaySinh);

                    bool gioiTinhBit = nv.gioiTinh == "1" || nv.gioiTinh.ToLower() == "nữ";
                    cmd.Parameters.AddWithValue("@gioiTinh", gioiTinhBit ? 1 : 0);

                    cmd.Parameters.AddWithValue("@diaChi", nv.diaChi);
                    cmd.Parameters.AddWithValue("@soDienThoai", nv.soDienThoai);
                    cmd.Parameters.AddWithValue("@Email", nv.email);
                    cmd.Parameters.AddWithValue("@isTruongPhong", isTruongPhong ? 1 : 0);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi sửa nhân viên: " + ex.Message);
                    }
                }
            }
        }


        public void SuaKhachHang(KhachHang kh)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SuaKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maKH", kh.maKH);
                    cmd.Parameters.AddWithValue("@tenDoanhNghiep", kh.tenDoanhNghiep);
                    cmd.Parameters.AddWithValue("@kyHieuDN", kh.kyHieuDN);
                    cmd.Parameters.AddWithValue("@diaChi", kh.diaChi);
                    cmd.Parameters.AddWithValue("@nguoiDaiDien", kh.nguoiDaiDien);
                    cmd.Parameters.AddWithValue("@soDienThoaiKH", kh.soDienThoaiKH);
          
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi sửa khách hàng : " + ex.Message);
                    }
                }
            }
        }


        public void XoaKhachHang(string maKH)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.XoaKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maKH", maKH);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi xóa khách hàng : " + ex.Message);
                    }
                }
            }
        }

        public void XoaNhanVien(string maNV)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maNV", maNV);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi xóa nhân viên: " + ex.Message);
                    }
                }
            }
        }

        public List<PhongBan> LayDSPhongBan()
        {
            var list = new List<PhongBan>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("LayDSPhongBan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PhongBan
                            {
                                maPhong = reader["maPhong"].ToString(),
                                tenPhong = reader["tenPhong"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }


        public List<PhongBan> layPTNvaPHT()
        {
            var list = new List<PhongBan>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("TaiLenPTNvaPHT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PhongBan
                            {
                                maPhong = reader["maPhong"].ToString(),
                                tenPhong = reader["tenPhong"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }



        public PhongBan? LayPhongBanTheoMa(string maPhong)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("LayPhongBanTheoMa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maPhong", maPhong);

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PhongBan
                                {
                                    maPhong = reader["maPhong"].ToString(),
                                    tenPhong = reader["tenPhong"].ToString(),
                                    truongPhong = reader["truongPhong"]?.ToString()
                                };
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi lấy thông tin phòng ban: " + ex.Message);
                    }
                }
            }
            return null;

        }

        public string LayPhongBanTheoTaiKhoan(string tenTK)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("LayPhongBanTheoTaiKhoan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenTK", tenTK);

                    var result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }


        public bool ThemThongSoMoiTruong(ThongSo ts)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                using (SqlCommand cmd = new SqlCommand("sp_ThemThongSoMoiTruong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenTS", ts.TenTS);
                    cmd.Parameters.AddWithValue("@donVi", ts.DonVi);
                    cmd.Parameters.AddWithValue("@phuongPhap", ts.phuongPhap);
                    cmd.Parameters.AddWithValue("@giaTriToiDa", (object)ts.GiaTriToiDa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@giaTriToiThieu", (object)ts.GiaTriToiThieu ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery(); // thực thi proc
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Lỗi SQL: " + ex.Message);
                return false;
            }
        }


        public List<ThongSo> LayDSThongSo()
        {
            var list = new List<ThongSo>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayTenThongSoMoiTruong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ThongSo
                            {
                                MaTS = reader["maTS"].ToString(),
                                TenTS = reader["tenTS"].ToString(),
                                DonVi = reader["donVi"].ToString(),
                                GiaTriToiThieu = reader["giaTriToiThieu"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiThieu"]),
                                GiaTriToiDa = reader["giaTriToiDa"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiDa"]),

                            });
                        }
                    }
                }
            }

            return list;
        }


        public List<ChiTietQuanTracView> LayChiTietTheoNen(string maNen)
        {
            var list = new List<ChiTietQuanTracView>();
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayChiTietQuanTracTheoNen", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maNen", maNen);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietQuanTracView
                            {
                                MaNen = reader["maNen"].ToString(),
                                MaTS = reader["maTS"].ToString(),
                                TenTS = reader["tenTS"].ToString(),
                                DonVi = reader["donVi"].ToString(),
                                GiaTriToiThieu = reader["giaTriToiThieu"] as double?,
                                GiaTriToiDa = reader["giaTriToiDa"] as double?,
                                MaPhong = reader["maPhong"].ToString(),
                                TenPhong = reader["tenPhong"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }


        public string ThemNenMau(string tenNenMau, string moTa)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ThemNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@moTa", SqlDbType.NVarChar, -1).Value = (object)moTa ?? DBNull.Value;
                    cmd.Parameters.Add("@tenNenMau", SqlDbType.NVarChar, 100).Value = (object)tenNenMau ?? DBNull.Value;

                    object result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }


        public List<HopDong> LayDanhSachHopDong()
        {
            List<HopDong> list = new List<HopDong>();

            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachHopDong", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HopDong dto = new HopDong
                                {
                                    MaHD = reader["maHD"].ToString(),
                                    MaKH = reader["maKH"].ToString(),
                                    TenDoanhNghiep = reader["tenDoanhNghiep"].ToString(),
                                    NguoiDaiDien = reader["nguoiDaiDien"].ToString(),
                                    NgayKy = Convert.ToDateTime(reader["ngayKy"]),
                                    NgayDuKien = Convert.ToDateTime(reader["ngayDuKien"]),
                                    TrangThai = Convert.ToBoolean(reader["trangThai"]),
                                    DisplayText = reader["displayText"].ToString()
                                };

                                list.Add(dto);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hợp đồng: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi: " + ex.Message);
            }
            return list;
        }



        public List<ThongSo> GetDanhSachThongSo()
        {
            List<ThongSo> list = new List<ThongSo>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachThongSo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new ThongSo
                        {
                            MaTS = reader["maTS"].ToString(),
                            TenTS = reader["tenTS"].ToString(),
                            DonVi = reader["donVi"].ToString(),
                            GiaTriToiDa = reader["giaTriToiDa"] as double?,
                            GiaTriToiThieu = reader["giaTriToiThieu"] as double?,
                            phuongPhap = reader["phuongPhap"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public List<NenMau> GetDanhSachNenMau()
        {
            List<NenMau> list = new List<NenMau>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new NenMau
                        {
                            maNen = reader["maNen"].ToString(),
                            tenNenMau = reader["tenNenMau"].ToString(),
                            moTa = reader["moTa"].ToString(),
                           
                        });
                    }
                }
            }

            return list;
        }


        public void XoaNenMau(string maNen)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_XoaNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maNen", maNen);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi xóa nền mẫu : " + ex.Message);
                    }
                }
            }
        }


        public string TaoDotQuanTracDraft()
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_TaoDotQuanTracDraft", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter pMaDot = new SqlParameter("@maDot", SqlDbType.VarChar, 15)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMaDot);

                    cmd.ExecuteNonQuery();

                    return pMaDot.Value?.ToString();
                }
            }
        }


        public List<DTO_DotQuanTrac> LayDanhSachQuanTrac()
        {

            List<DTO_DotQuanTrac> dsDQT = new List<DTO_DotQuanTrac>();
            using (SqlConnection conn = SqlConnectionData.Connect())
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachDotQuanTrac", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nv = new DTO_DotQuanTrac
                            {
                                MaDot = reader["maDot"].ToString(),
                                MaHD = reader["maHD"].ToString(),
                                NoiDung = reader["noiDung"].ToString(),
                                DotQuanTrac = reader["dotQuanTrac"].ToString(),
                                NgayBatDau= Convert.ToDateTime(reader["ngayBatDau"]),
                                NgayDuKien = Convert.ToDateTime(reader["ngayDuKien"]),
                                NgayTraKQ = reader["ngayTraKQ"] != DBNull.Value ? Convert.ToDateTime(reader["ngayTraKQ"]) : (DateTime?)null,
                                TrangThai = reader["TrangThai"]?.ToString()

                            };
                            dsDQT.Add(nv);

                        }

                    }

                }

            }
            return dsDQT;

        }


        public DTO_DotNen ThemNenMauVaoDot(string maDot, string maNen)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ThemNenMauVaoDot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maDot", maDot);
                    cmd.Parameters.AddWithValue("@maNen", maNen);

                    SqlParameter paramMaDN = new SqlParameter("@maDN", SqlDbType.VarChar, 15);
                    paramMaDN.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramMaDN);

                    DTO_DotNen result = null;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new DTO_DotNen
                            {
                                MaDN = reader["maDN"].ToString(),
                                MaDot = reader["maDot"].ToString(),
                                MaNen = reader["maNen"].ToString(),
                                TenViTri = null,
                                ToaDo = null,
                                GhiChu = null
                            };
                        }
                    }

                    return result;
                }
            }
        }


        public DataTable LayDanhSachTrangThai()
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachTrangThai", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }


        public bool LuuChiTietNenMau(string maDN,string tenViTri,string toaDo,string ghiChu,List<ChiTietQuanTracView> danhSachThongSo)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LuuChiTietNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@maDN", maDN);
                    cmd.Parameters.AddWithValue("@tenViTri", tenViTri);
                    cmd.Parameters.AddWithValue("@toaDo", string.IsNullOrWhiteSpace(toaDo) ? (object)DBNull.Value : toaDo);
                    cmd.Parameters.AddWithValue("@ghiChu", string.IsNullOrWhiteSpace(ghiChu) ? (object)DBNull.Value : ghiChu);

                    // Chuyển danh sách thông số thành JSON
                    string jsonThongSo = ConvertToJsonArray(danhSachThongSo);
                    cmd.Parameters.AddWithValue("@danhSachThongSo", jsonThongSo);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Lỗi lưu chi tiết nền mẫu: {ex.Message}");
                    }
                }
            }
        }


        private string ConvertToJsonArray(List<ChiTietQuanTracView> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return "[]";

            var jsonItems = new List<string>();

            foreach (var item in danhSach)
            {
                string giaTriMin = item.GiaTriToiThieu.HasValue
                    ? item.GiaTriToiThieu.Value.ToString("0.##").Replace(",", ".")
                    : "null";

                string giaTriMax = item.GiaTriToiDa.HasValue
                    ? item.GiaTriToiDa.Value.ToString("0.##").Replace(",", ".")
                    : "null";
                string tenTS = EscapeJson(item.TenTS ?? "");
                string donVi = EscapeJson(item.DonVi ?? "");
                string phuongPhap = EscapeJson(item.PhuongPhap ?? "");
                string maPhong = item.MaPhong ?? "";

                string jsonObject = $@"{{
                    ""maTS"": ""{item.MaTS}"",
                    ""tenTS"": ""{tenTS}"",
                    ""donVi"": ""{donVi}"",
                    ""giaTriToiThieu"": {giaTriMin},
                    ""giaTriToiDa"": {giaTriMax},
                    ""phuongPhap"": ""{phuongPhap}"",
                    ""maPhong"": ""{maPhong}""
                }}";

                jsonItems.Add(jsonObject);
            }

            return "[" + string.Join(",", jsonItems) + "]";
        }

        private string EscapeJson(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input
                .Replace("\\", "\\\\")  // Backslash
                .Replace("\"", "\\\"")  // Double quote
                .Replace("\n", "\\n")   // Newline
                .Replace("\r", "\\r")   // Carriage return
                .Replace("\t", "\\t");  // Tab
        }


        //public DTO_DotNen LayThongTinDotNen(string maDN)
        //{
        //    using (SqlConnection conn = SqlConnectionData.Connect())
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = new SqlCommand("sp_LayThongTinDotNen", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@maDN", maDN);

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    return new DTO_DotNen
        //                    {
        //                        MaDN = reader["maDN"].ToString(),
        //                        MaDot = reader["maDot"].ToString(),
        //                        MaNen = reader["maNen"].ToString(),
        //                        TenViTri = reader["tenViTri"]?.ToString(),
        //                        ToaDo = reader["toaDo"]?.ToString(),
        //                        GhiChu = reader["ghiChu"]?.ToString()
        //                    };
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}


        public (bool Success, string Message) XoaDotQuanTrac(string maDot)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaDotQuanTrac", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maDot", maDot);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool result = Convert.ToInt32(reader["Result"]) == 1;
                                string message = reader["Message"].ToString();
                                return (result, message);
                            }
                        }
                    }
                }
                return (false, "Không nhận được kết quả từ server.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public bool HoanTatKeHoachQuanTrac(DTO_DotQuanTrac dto)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_HoanTatKeHoachQuanTrac", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@maDot", dto.MaDot);
                        cmd.Parameters.AddWithValue("@maHD", dto.MaHD);
                        cmd.Parameters.AddWithValue("@noiDung", string.IsNullOrEmpty(dto.NoiDung) ? (object)DBNull.Value : dto.NoiDung);
                        cmd.Parameters.AddWithValue("@dotQuanTrac", dto.DotQuanTrac);
                        cmd.Parameters.AddWithValue("@ngayBatDau", dto.NgayBatDau);
                        cmd.Parameters.AddWithValue("@ngayDuKien", dto.NgayDuKien);
                        cmd.Parameters.AddWithValue("@ngayTraKQ", dto.NgayTraKQ.HasValue ? (object)dto.NgayTraKQ.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@trangThai", dto.TrangThai);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi khi hoàn tất kế hoạch quan trắc: {ex.Message}", ex);
            }
        }


        public bool SuaThongSoMoiTruong(ThongSo ts)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SuaThongSoMoiTruong", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@maTS", ts.MaTS);
                        cmd.Parameters.AddWithValue("@tenTS", ts.TenTS);
                        cmd.Parameters.AddWithValue("@giaTriToiDa", ts.GiaTriToiDa);
                        cmd.Parameters.AddWithValue("@giaTriToiThieu", ts.GiaTriToiThieu);
                        cmd.Parameters.AddWithValue("@donVi", ts.DonVi);
                        cmd.Parameters.AddWithValue("@phuongPhap", ts.phuongPhap);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DAL: " + ex.Message);
                return false;
            }
        }


        public bool XoaThongSoMoiTruong(string maTS, out string ketQua)
        {
            ketQua = "";

            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_XoaThongSoMoiTruong", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@maTS", maTS);

                        SqlParameter outputParam = new SqlParameter("@ketQua", SqlDbType.NVarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        ketQua = outputParam.Value.ToString();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ketQua = "Lỗi DAL: " + ex.Message;
                return false;
            }
        }
    }
}

