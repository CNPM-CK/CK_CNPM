using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL
{
    public class SqlConnectionData
    {
        public static SqlConnection Connect()
        {
            //string connectionStr = "Data Source=ThaiQuangTran\\SQLEXPRESS;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            string connectionStr = "Data Source=PTT;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            //string connectionStr = "Data Source=LAPTOP-61AGFMMJ\\TONTHAI;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

            SqlConnection conn = new SqlConnection(connectionStr);
            return conn;
        }
    }

    public class DatabaseAccess
    {
        public TaiKhoan? kiemTraDangNhap(string username)
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

        public List<NhanVien> layDanhSachNhanVien()
        {
            List<NhanVien> dsNhanvien = new List<NhanVien>();
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("layDanhSachNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string gioiTinh = "0";
                            if (reader["gioiTinh"] != DBNull.Value)
                            {
                                var gioiTinhValue = reader["gioiTinh"];

                                if (gioiTinhValue is bool boolValue)
                                {
                                    gioiTinh = boolValue ? "1" : "0";
                                }
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
                                soDienThoai = reader["soDienThoai"].ToString(),
                                isTruongPhong = reader["isTruongPhong"] != DBNull.Value && Convert.ToInt32(reader["isTruongPhong"]) == 1,
                                trangThai = Convert.ToInt32(reader["trangThai"])
                            };
                            dsNhanvien.Add(nv);
                        }
                    }
                }
            }
            return dsNhanvien;
        }

        public List<NhanVien> layDanhSachNhanVien_PhanTrang(int pageNumber, int pageSize)
        {
            List<NhanVien> dsNhanvien = new List<NhanVien>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("LayDanhSachNhanVien_PhanTrang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string gioiTinh = "0";
                            if (reader["gioiTinh"] != DBNull.Value)
                            {
                                var gioiTinhValue = reader["gioiTinh"];
                                if (gioiTinhValue is bool boolValue)
                                    gioiTinh = boolValue ? "1" : "0";
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
                                soDienThoai = reader["soDienThoai"].ToString(),
                                isTruongPhong = reader["isTruongPhong"] != DBNull.Value && Convert.ToInt32(reader["isTruongPhong"]) == 1,
                                trangThai = Convert.ToInt32(reader["trangThai"])
                            };

                            dsNhanvien.Add(nv);
                        }
                    }
                }
            }

            return dsNhanvien;
        }

        public List<KhachHang> layDanhSachKH()
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
                            kh.emailDoanhNghiep = reader["emailDoanhNghiep"].ToString();
                            kh.emailNguoiDaiDien = reader["emailNguoiDaiDien"].ToString();
                            kh.maSoThue = reader["maSoThue"].ToString();
                            kh.trangThai = Convert.ToInt32(reader["trangThai"]);
                            dsKhachhang.Add(kh);
                        }
                    }
                }
            }
            return dsKhachhang;
        }

        public List<KhachHang> layDanhSachKH_PhanTrang(int pageNumber, int pageSize)
        {
            List<KhachHang> dsKhachhang = new List<KhachHang>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("LayDSKH_PhanTrang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachHang kh = new KhachHang
                            {
                                maKH = reader["maKH"].ToString(),
                                tenDoanhNghiep = reader["tenDoanhNghiep"].ToString(),
                                kyHieuDN = reader["kyHieuDN"]?.ToString(),
                                diaChi = reader["diaChi"]?.ToString(),
                                nguoiDaiDien = reader["nguoiDaiDien"]?.ToString(),
                                soDienThoaiKH = reader["soDienThoaiKH"]?.ToString(),
                                maSoThue = reader["maSoThue"]?.ToString(),
                                emailNguoiDaiDien = reader["emailNguoiDaiDien"]?.ToString(),
                                emailDoanhNghiep = reader["emailDoanhNghiep"]?.ToString(),
                                trangThai = Convert.ToInt32(reader["trangThai"])
                            };
                            dsKhachhang.Add(kh);
                        }
                    }
                }
            }
            return dsKhachhang;
        }

        public void themNhanVien(NhanVien nv, bool isTruongPhong)
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
                    cmd.Parameters.AddWithValue("@trangThai", nv.trangThai);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void themKhachHang(KhachHang kh)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.ThemKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tenDoanhNghiep", kh.tenDoanhNghiep);
                    cmd.Parameters.AddWithValue("@kyHieuDN", string.IsNullOrEmpty(kh.kyHieuDN) ? (object)DBNull.Value : kh.kyHieuDN);
                    cmd.Parameters.AddWithValue("@diaChi", string.IsNullOrEmpty(kh.diaChi) ? (object)DBNull.Value : kh.diaChi);
                    cmd.Parameters.AddWithValue("@nguoiDaiDien", string.IsNullOrEmpty(kh.nguoiDaiDien) ? (object)DBNull.Value : kh.nguoiDaiDien);
                    cmd.Parameters.AddWithValue("@soDienThoaiKH", string.IsNullOrEmpty(kh.soDienThoaiKH) ? (object)DBNull.Value : kh.soDienThoaiKH);
                    cmd.Parameters.AddWithValue("@maSoThue", string.IsNullOrEmpty(kh.maSoThue) ? (object)DBNull.Value : kh.maSoThue);
                    cmd.Parameters.AddWithValue("@emailNguoiDaiDien", string.IsNullOrEmpty(kh.emailNguoiDaiDien) ? (object)DBNull.Value : kh.emailNguoiDaiDien);
                    cmd.Parameters.AddWithValue("@emailDoanhNghiep", string.IsNullOrEmpty(kh.emailDoanhNghiep) ? (object)DBNull.Value : kh.emailDoanhNghiep);
                    cmd.Parameters.AddWithValue("@trangThai", kh.trangThai > 0 ? kh.trangThai : (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void suaNhanVien(NhanVien nv, bool isTruongPhong)
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
                    cmd.Parameters.AddWithValue("@trangThai", nv.trangThai);
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

        public void suaKhachHang(KhachHang kh)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SuaKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maKH", kh.maKH);
                    cmd.Parameters.AddWithValue("@tenDoanhNghiep", kh.tenDoanhNghiep);
                    cmd.Parameters.AddWithValue("@kyHieuDN", (object)kh.kyHieuDN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@diaChi", kh.diaChi);
                    cmd.Parameters.AddWithValue("@nguoiDaiDien", kh.nguoiDaiDien);
                    cmd.Parameters.AddWithValue("@soDienThoaiKH", kh.soDienThoaiKH);
                    cmd.Parameters.AddWithValue("@maSoThue", (object)kh.maSoThue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailNguoiDaiDien", (object)kh.emailNguoiDaiDien ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailDoanhNghiep", (object)kh.emailDoanhNghiep ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@trangThai", kh.trangThai);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi sửa khách hàng: " + ex.Message, ex);
                    }
                }
            }
        }

        public void xoaKhachHang(string maKH)
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

        public void xoaNhanVien(string maNV)
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

        public List<PhongBan> layDSPhongBan()
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

        public PhongBan? layPhongBanTheoMa(string maPhong)
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

        public string layPhongBanTheoTaiKhoan(string tenTK)
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

        public bool themThongSoMoiTruong(ThongSo ts)
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
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Lỗi SQL: " + ex.Message);
                return false;
            }
        }

        public List<ThongSo> layDSThongSo()
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

        public List<HopDong> layDanhSachHD()
        {
            var list = new List<HopDong>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("layDanhSachHopDong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new HopDong
                            {
                                maHD = r["maHD"].ToString(),
                                maKH = r["maKH"].ToString(),
                                ngayKy = r.GetDateTime(r.GetOrdinal("ngayKy")),
                                ngayKetThucHD = r.GetDateTime(r.GetOrdinal("ngayKetThucHD")),
                                trangThai = r["trangThai"].ToString(),
                                tanSuatQuanTrac = r["tanSuatQuanTrac"].ToString(),
                                soHD = r["soHD"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public string themNenMau(string tenNenMau, string moTa)
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

        public List<ThongSo> layDanhSachThongSo()
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

        public List<NenMau> layDanhSachNenMau()
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

        public void xoaNenMau(string maNen)
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

        public bool suaNenMau(string maNen, string moTa)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SuaMoTaNenMau", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@maNen", maNen);
                        cmd.Parameters.AddWithValue("@moTa", moTa ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DAL.SuaNenMau: " + ex.Message);
                return false;
            }
        }

        public string taoDotQuanTracNhap()
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

        public List<DTO_DotQuanTrac> layDanhSachQuanTrac()
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
                                NgayBatDau = Convert.ToDateTime(reader["ngayBatDau"]),
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

        public DTO_DotNen themNenMauVaoDot(string maDot, string maNen)
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

        public DataTable layDanhSachTrangThai()
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

        public bool luuChiTietNenMau(string maDN, string tenViTri, string toaDo, string ghiChu, List<ChiTietQuanTracView> danhSachThongSo)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LuuChiTietNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maDN", maDN);
                    cmd.Parameters.AddWithValue("@tenViTri", tenViTri);
                    cmd.Parameters.AddWithValue("@toaDo", string.IsNullOrWhiteSpace(toaDo) ? (object)DBNull.Value : toaDo);
                    cmd.Parameters.AddWithValue("@ghiChu", string.IsNullOrWhiteSpace(ghiChu) ? (object)DBNull.Value : ghiChu);

                    string jsonThongSo = chuyendoiJsonquaArray(danhSachThongSo);
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

        public bool suaChiTietNenMau(string maDN, string tenViTri, string toaDo, string ghiChu, List<ChiTietQuanTracView> danhSachThongSo)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SuaChiTietNenMau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maDN", maDN);
                    cmd.Parameters.AddWithValue("@tenViTri", tenViTri);
                    cmd.Parameters.AddWithValue("@toaDo", string.IsNullOrWhiteSpace(toaDo) ? (object)DBNull.Value : toaDo);
                    cmd.Parameters.AddWithValue("@ghiChu", string.IsNullOrWhiteSpace(ghiChu) ? (object)DBNull.Value : ghiChu);

                    string jsonThongSo = chuyendoiJsonquaArray(danhSachThongSo);
                    cmd.Parameters.AddWithValue("@danhSachThongSo", jsonThongSo);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Lỗi sửa chi tiết nền mẫu: {ex.Message}");
                    }
                }
            }
        }

        private string chuyendoiJsonquaArray(List<ChiTietQuanTracView> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return "[]";

            var jsonItems = new List<string>();

            foreach (var item in danhSach)
            {
                var properties = new List<string>();

                if (!string.IsNullOrWhiteSpace(item.MaDNTS))
                    properties.Add($"\"maDNTS\":\"{escapeJson(item.MaDNTS)}\"");

                if (!string.IsNullOrWhiteSpace(item.MaTS))
                    properties.Add($"\"maTS\":\"{escapeJson(item.MaTS)}\"");

                if (!string.IsNullOrWhiteSpace(item.TenTS))
                    properties.Add($"\"tenTS\":\"{escapeJson(item.TenTS)}\"");

                if (!string.IsNullOrWhiteSpace(item.DonVi))
                    properties.Add($"\"donVi\":\"{escapeJson(item.DonVi)}\"");

                if (item.GiaTriToiThieu.HasValue)
                    properties.Add($"\"giaTriToiThieu\":{item.GiaTriToiThieu.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

                if (item.GiaTriToiDa.HasValue)
                    properties.Add($"\"giaTriToiDa\":{item.GiaTriToiDa.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

                if (!string.IsNullOrWhiteSpace(item.PhuongPhap))
                    properties.Add($"\"phuongPhap\":\"{escapeJson(item.PhuongPhap)}\"");

                if (!string.IsNullOrWhiteSpace(item.MaPhong))
                    properties.Add($"\"maPhong\":\"{escapeJson(item.MaPhong)}\"");

                jsonItems.Add("{" + string.Join(",", properties) + "}");
            }

            return "[" + string.Join(",", jsonItems) + "]";
        }

        private string escapeJson(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t");
        }

        public (bool Success, string Message) xoaDotQuanTrac(string maDot)
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

        public bool hoanTatKeHoachQuanTrac(DTO_DotQuanTrac dto)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    Debug.WriteLine(dto.TrangThai);
                    Debug.WriteLine(dto.MaDot);
                    Debug.WriteLine(dto.MaHD);
                    Debug.WriteLine(dto.NoiDung);
                    Debug.WriteLine(dto.DotQuanTrac);
                    Debug.WriteLine(dto.NgayBatDau);
                    Debug.WriteLine(dto.NgayDuKien);
                    Debug.WriteLine(dto.NgayTraKQ);
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

        public bool suaThongSoMoiTruong(ThongSo ts)
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

        public bool xoaThongSoMoiTruong(string maTS, out string ketQua)
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

        public List<HopDongVaTenDN> layDanhSachHopDong()
        {
            var list = new List<HopDongVaTenDN>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("layDanhSachHopDongVaTenDN", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new HopDongVaTenDN
                            {
                                maHD = r["maHD"].ToString(),
                                maHDVaKH = r["maHDVaKH"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void ThemHopDong(HopDong hd)
        {
            // Method implementation commented out in original
        }

        public List<TrangThaiKhachHang> layTrangThaiKhachHang()
        {
            List<TrangThaiKhachHang> list = new List<TrangThaiKhachHang>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.sp_LayTrangThaiKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TrangThaiKhachHang
                            {
                                maTrangThai = reader.GetInt32(0),
                                tenTrangThai = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return list;
        }

        public int demTongSoKhachHang()
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DemTongKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<DTO_DotQuanTrac> layDanhSachDotQuanTrac_PhanTrang(int pageNumber, int pageSize)
        {
            List<DTO_DotQuanTrac> list = new List<DTO_DotQuanTrac>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.LayDotQuanTrac_PhanTrang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTO_DotQuanTrac dqt = new DTO_DotQuanTrac
                            {
                                MaDot = reader["MaDot"].ToString(),
                                MaHD = reader["MaHD"].ToString(),
                                NoiDung = reader["NoiDung"]?.ToString(),
                                DotQuanTrac = reader["DotQuanTrac"]?.ToString(),
                                NgayBatDau = reader["NgayBatDau"] != DBNull.Value ? Convert.ToDateTime(reader["NgayBatDau"]) : DateTime.MinValue,
                                NgayDuKien = reader["NgayDuKien"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDuKien"]) : DateTime.MinValue,
                                NgayTraKQ = reader["NgayTraKQ"] != DBNull.Value ? Convert.ToDateTime(reader["NgayTraKQ"]) : (DateTime?)null,
                                TrangThai = reader["TrangThai"]?.ToString()
                            };
                            list.Add(dqt);
                        }
                    }
                }
            }

            return list;
        }

        public int demTongKHQT()
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DemTongKHQT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int demTongDSNV()
        {
            int count = 0;

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DemTongNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    count = (int)cmd.ExecuteScalar();
                }
            }

            return count;
        }

        public List<TrangThaiNhanVien> layTrangThaiNhanVien()
        {
            List<TrangThaiNhanVien> list = new List<TrangThaiNhanVien>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayTrangThaiNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrangThaiNhanVien tt = new TrangThaiNhanVien
                            {
                                MaTrangThai = Convert.ToInt32(reader["maTrangThai"]),
                                TenTrangThai = reader["tenTrangThai"].ToString()
                            };
                            list.Add(tt);
                        }
                    }
                }
            }
            return list;
        }

        // =============================================
        // PHẦN QUẢN LÝ KẾT QUẢ (HỆ THỐNG CŨ)
        // =============================================

        public List<DTO_KetQua> LayDanhSachKetQua()
        {
            List<DTO_KetQua> list = new List<DTO_KetQua>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachKetQua", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DTO_KetQua
                            {
                                MaKQ = reader["maKQ"].ToString(),
                                MaNen = reader["maNen"].ToString(),
                                TenNenMau = reader["tenNenMau"].ToString(),
                                MaTS = reader["maTS"].ToString(),
                                TenTS = reader["tenTS"].ToString(),
                                NhanVienNhap = reader["nhanVienNhap"].ToString(),
                                TenNhanVien = reader["tenNhanVien"].ToString(),
                                NgayDo = Convert.ToDateTime(reader["ngayDo"]),
                                GiaTriDoDuoc = Convert.ToInt32(reader["giaTriDoDuoc"]),
                                DonVi = reader["donVi"].ToString(),
                                GiaTriToiThieu = reader["giaTriToiThieu"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiThieu"]),
                                GiaTriToiDa = reader["giaTriToiDa"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiDa"]),
                                TrangThai = reader["trangThai"].ToString(),
                                TrangThaiXacNhanBit = Convert.ToBoolean(reader["trangThaiXacNhanBit"]),
                                GhiChu = reader["ghiChu"] == DBNull.Value ? "" : reader["ghiChu"].ToString(),
                                MaBC = reader["maBC"] == DBNull.Value ? "" : reader["maBC"].ToString(),
                                MaDNTS = reader["maDNTS"] == DBNull.Value ? "" : reader["maDNTS"].ToString(),
                                TinhTrang = reader["tinhTrang"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }

        public DTO_KetQua LayChiTietKetQua(string maKQ)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayChiTietKetQua", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maKQ", maKQ);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DTO_KetQua
                            {
                                MaKQ = reader["maKQ"].ToString(),
                                MaNen = reader["maNen"].ToString(),
                                TenNenMau = reader["tenNenMau"].ToString(),
                                MoTaNen = reader["moTaNen"] == DBNull.Value ? "" : reader["moTaNen"].ToString(),
                                MaTS = reader["maTS"].ToString(),
                                TenTS = reader["tenTS"].ToString(),
                                DonVi = reader["donVi"].ToString(),
                                GiaTriToiThieu = reader["giaTriToiThieu"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiThieu"]),
                                GiaTriToiDa = reader["giaTriToiDa"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiDa"]),
                                PhuongPhap = reader["phuongPhap"] == DBNull.Value ? "" : reader["phuongPhap"].ToString(),
                                NhanVienNhap = reader["nhanVienNhap"].ToString(),
                                TenNhanVien = reader["tenNhanVien"].ToString(),
                                EmailNhanVien = reader["emailNhanVien"] == DBNull.Value ? "" : reader["emailNhanVien"].ToString(),
                                TenPhong = reader["tenPhong"].ToString(),
                                NgayDo = Convert.ToDateTime(reader["ngayDo"]),
                                GiaTriDoDuoc = Convert.ToInt32(reader["giaTriDoDuoc"]),
                                TrangThaiXacNhanBit = Convert.ToBoolean(reader["trangThaiXacNhan"]),
                                TrangThai = reader["trangThai"].ToString(),
                                GhiChu = reader["ghiChu"] == DBNull.Value ? "" : reader["ghiChu"].ToString(),
                                MaBC = reader["maBC"] == DBNull.Value ? "" : reader["maBC"].ToString(),
                                NgayXuat = reader["ngayXuat"] == DBNull.Value ? null : Convert.ToDateTime(reader["ngayXuat"]),
                                NguoiXuat = reader["nguoiXuat"] == DBNull.Value ? "" : reader["nguoiXuat"].ToString(),
                                TenNguoiXuat = reader["tenNguoiXuat"] == DBNull.Value ? "" : reader["tenNguoiXuat"].ToString(),
                                MaDNTS = reader["maDNTS"] == DBNull.Value ? "" : reader["maDNTS"].ToString(),
                                TenViTri = reader["tenViTri"] == DBNull.Value ? "" : reader["tenViTri"].ToString(),
                                ToaDo = reader["toaDo"] == DBNull.Value ? "" : reader["toaDo"].ToString(),
                                TinhTrang = reader["tinhTrang"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public (bool Success, string Message) CapNhatTrangThaiKetQua(string maKQ, bool trangThaiXacNhan)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CapNhatTrangThaiKetQua", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maKQ", maKQ);
                        cmd.Parameters.AddWithValue("@trangThaiXacNhan", trangThaiXacNhan);

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

        // =============================================
        // PHẦN QUẢN LÝ BÁO CÁO
        // =============================================

        public List<DTO_BaoCao> LayDanhSachBaoCao()
        {
            List<DTO_BaoCao> list = new List<DTO_BaoCao>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachBaoCao", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var baoCao = new DTO_BaoCao();
                            baoCao.MaBC = reader["MaBC"].ToString();
                            baoCao.MaDot = reader["MaDot"] == DBNull.Value ? "" : reader["MaDot"].ToString();
                            baoCao.TenNguoiXuat = reader["TenNguoiXuat"].ToString();
                            baoCao.NgayXuat = Convert.ToDateTime(reader["NgayXuat"]);
                            baoCao.SoNenMau = Convert.ToInt32(reader["SoNenMau"]);
                            baoCao.TongSoThongSo = Convert.ToInt32(reader["TongSoThongSo"]);
                            baoCao.TrangThai = reader["TrangThai"].ToString();

                            list.Add(baoCao);
                        }
                    }
                }
            }

            return list;
        }

        public List<DTO_KetQua> LayChiTietKetQuaTheoBC(string maBC)
        {
            List<DTO_KetQua> list = new List<DTO_KetQua>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayChiTietKetQuaTheoBC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maBC", maBC);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DTO_KetQua
                            {
                                MaKQ = reader["maKQ"].ToString(),
                                MaNen = reader["maNen"].ToString(),
                                TenNenMau = reader["tenNenMau"].ToString(),
                                MoTaNen = reader["moTaNen"] == DBNull.Value ? "" : reader["moTaNen"].ToString(),
                                MaTS = reader["maTS"].ToString(),
                                TenTS = reader["tenTS"].ToString(),
                                DonVi = reader["donVi"].ToString(),
                                GiaTriToiThieu = reader["giaTriToiThieu"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiThieu"]),
                                GiaTriToiDa = reader["giaTriToiDa"] == DBNull.Value ? null : Convert.ToDouble(reader["giaTriToiDa"]),
                                PhuongPhap = reader["phuongPhap"] == DBNull.Value ? "" : reader["phuongPhap"].ToString(),
                                NhanVienNhap = reader["nhanVienNhap"].ToString(),
                                TenNhanVien = reader["tenNhanVien"].ToString(),
                                EmailNhanVien = reader["emailNhanVien"] == DBNull.Value ? "" : reader["emailNhanVien"].ToString(),
                                TenPhong = reader["tenPhong"].ToString(),
                                NgayDo = Convert.ToDateTime(reader["ngayDo"]),
                                GiaTriDoDuoc = Convert.ToInt32(reader["giaTriDoDuoc"]),
                                GhiChu = reader["ghiChu"] == DBNull.Value ? "" : reader["ghiChu"].ToString(),
                                MaBC = reader["maBC"].ToString(),
                                MaDNTS = reader["maDNTS"] == DBNull.Value ? "" : reader["maDNTS"].ToString(),
                                TrangThaiXacNhanBit = Convert.ToBoolean(reader["trangThaiXacNhanBit"]),
                                TrangThai = reader["trangThai"].ToString(),
                                TinhTrang = reader["tinhTrang"].ToString(),
                                TenViTri = reader["tenViTri"] == DBNull.Value ? "" : reader["tenViTri"].ToString(),
                                ToaDo = reader["toaDo"] == DBNull.Value ? "" : reader["toaDo"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }

        // =============================================
        // PHẦN MỚI: QUẢN LÝ KẾT QUẢ (HỆ THỐNG MỚI)
        // =============================================

        /// <summary>
        /// Lấy danh sách kết quả (hiển thị trên dgvDanhsachketqua)
        /// </summary>
        public List<DTO_KetQuaHeader> LayDanhSachKetQuaMoi()
        {
            List<DTO_KetQuaHeader> list = new List<DTO_KetQuaHeader>();

            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LayDanhSachKetQua", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DTO_KetQuaHeader header = new DTO_KetQuaHeader
                                {
                                    MaKQ = reader["maKQ"].ToString(),
                                    NgayTao = Convert.ToDateTime(reader["ngayTao"]),
                                    NgayTraKQ = reader["ngayTraKQ"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["ngayTraKQ"])
                                        : (DateTime?)null,
                                    TenNhanVien = reader["NguoiNhap"].ToString(),

                                    // ✅ FIX: Đọc từ cột TrangThai (TEXT) và chuyển sang bool
                                    TrangThaiXacNhan = reader["TrangThai"].ToString().Trim()
                                        .Equals("Đã xác nhận", StringComparison.OrdinalIgnoreCase),

                                    GhiChu = reader["ghiChu"] != DBNull.Value ? reader["ghiChu"].ToString() : "",
                                    DotQuanTrac = reader["dotQuanTrac"] != DBNull.Value ? reader["dotQuanTrac"].ToString() : "",
                                    MaDot = reader["maDot"] != DBNull.Value ? reader["maDot"].ToString() : "",
                                    SoNenMau = Convert.ToInt32(reader["SoNenMau"])
                                };

                                list.Add(header);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách kết quả: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy chi tiết kết quả theo mã KQ (hiển thị trên dgvChiTiet)
        /// </summary>
        public DTO_KetQuaFull LayChiTietKetQuaTheoMaKQ(string maKQ)
        {
            DTO_KetQuaFull result = new DTO_KetQuaFull();
            Dictionary<string, DTO_KetQuaNenMau> dictNenMau = new Dictionary<string, DTO_KetQuaNenMau>();

            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayChiTietKetQuaTheoMaKQ", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maKQ", maKQ);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool headerLoaded = false;

                        while (reader.Read())
                        {
                            // Load header (chỉ 1 lần)
                            if (!headerLoaded)
                            {
                                result.Header = new DTO_KetQuaHeader
                                {
                                    MaKQ = reader["maKQ"].ToString(),
                                    NgayTao = Convert.ToDateTime(reader["ngayTao"]),
                                    NgayTraKQ = reader["ngayTraKQ"] == DBNull.Value ? null : Convert.ToDateTime(reader["ngayTraKQ"]),
                                    TenNhanVien = reader["NguoiNhap"].ToString(),
                                    TrangThaiXacNhan = Convert.ToBoolean(reader["trangThaiXacNhan"]),
                                    GhiChu = reader["ghiChu"] == DBNull.Value ? "" : reader["ghiChu"].ToString(),
                                    DotQuanTrac = reader["dotQuanTrac"] == DBNull.Value ? "" : reader["dotQuanTrac"].ToString()
                                };
                                headerLoaded = true;
                            }

                            // Load nền mẫu
                            string maKQNen = reader["maKQNen"] == DBNull.Value ? "" : reader["maKQNen"].ToString();

                            if (!string.IsNullOrEmpty(maKQNen))
                            {
                                if (!dictNenMau.ContainsKey(maKQNen))
                                {
                                    dictNenMau[maKQNen] = new DTO_KetQuaNenMau
                                    {
                                        MaKQNen = maKQNen,
                                        MaKQ = reader["maKQ"].ToString(),
                                        MaNen = reader["maNen"].ToString(),
                                        TenNenMau = reader["tenNenMau"].ToString(),
                                        ViTri = reader["viTri"] == DBNull.Value ? "" : reader["viTri"].ToString(),
                                        ToaDo = reader["toaDo"] == DBNull.Value ? "" : reader["toaDo"].ToString()
                                    };
                                }

                                // Load chi tiết thông số
                                string maKQCT = reader["maKQCT"] == DBNull.Value ? "" : reader["maKQCT"].ToString();
                                if (!string.IsNullOrEmpty(maKQCT))
                                {
                                    dictNenMau[maKQNen].DanhSachThongSo.Add(new DTO_KetQuaChiTiet
                                    {
                                        MaKQCT = maKQCT,
                                        MaKQNen = maKQNen,
                                        MaTS = reader["maTS"].ToString(),
                                        TenTS = reader["tenTS"].ToString(),
                                        DonVi = reader["donVi"] == DBNull.Value ? "" : reader["donVi"].ToString(),
                                        PhuongPhapPhanTich = reader["phuongPhapPhanTich"] == DBNull.Value ? "" : reader["phuongPhapPhanTich"].ToString(),
                                        KetQua = Convert.ToDouble(reader["ketQua"]),
                                        GioiHanPhatHien = reader["gioiHanPhatHien"] == DBNull.Value ? "" : reader["gioiHanPhatHien"].ToString(),
                                        QCVN = reader["qcvn"] == DBNull.Value ? "" : reader["qcvn"].ToString(),
                                        TinhTrang = reader["TinhTrang"] == DBNull.Value ? "" : reader["TinhTrang"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }

            // Chuyển dictionary thành list
            result.DanhSachNenMau = new List<DTO_KetQuaNenMau>(dictNenMau.Values);
            return result;
        }

        /// <summary>
        /// Cập nhật trạng thái xác nhận kết quả
        /// </summary>
        public (bool Success, string Message) CapNhatTrangThaiKetQuaMoi(string maKQ, bool trangThaiXacNhan)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CapNhatTrangThaiKetQua", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maKQ", maKQ);
                        cmd.Parameters.AddWithValue("@trangThaiXacNhan", trangThaiXacNhan);

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

        /// <summary>
        /// Thêm mới kết quả header
        /// </summary>
        public (bool Success, string Message, string MaKQ) ThemKetQuaHeader(string maDot, string nhanVienNhap, DateTime? ngayTraKQ, string ghiChu)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemKetQuaHeader", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maDot", maDot);
                        cmd.Parameters.AddWithValue("@nhanVienNhap", nhanVienNhap);
                        cmd.Parameters.AddWithValue("@ngayTraKQ", ngayTraKQ.HasValue ? (object)ngayTraKQ.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ghiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);

                        SqlParameter paramMaKQ = new SqlParameter("@maKQ", SqlDbType.VarChar, 15);
                        paramMaKQ.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramMaKQ);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool result = Convert.ToInt32(reader["Result"]) == 1;
                                string message = reader["Message"].ToString();
                                string maKQ = reader["MaKQ"] == DBNull.Value ? "" : reader["MaKQ"].ToString();
                                return (result, message, maKQ);
                            }
                        }
                    }
                }
                return (false, "Không nhận được kết quả từ server.", "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message, "");
            }
        }

        /// <summary>
        /// Thêm nền mẫu vào kết quả
        /// </summary>
        public (bool Success, string Message, string MaKQNen) ThemKetQuaNenMau(string maKQ, string maNen, string viTri, string toaDo)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemKetQuaNenMau", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maKQ", maKQ);
                        cmd.Parameters.AddWithValue("@maNen", maNen);
                        cmd.Parameters.AddWithValue("@viTri", string.IsNullOrEmpty(viTri) ? (object)DBNull.Value : viTri);
                        cmd.Parameters.AddWithValue("@toaDo", string.IsNullOrEmpty(toaDo) ? (object)DBNull.Value : toaDo);

                        SqlParameter paramMaKQNen = new SqlParameter("@maKQNen", SqlDbType.VarChar, 15);
                        paramMaKQNen.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramMaKQNen);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool result = Convert.ToInt32(reader["Result"]) == 1;
                                string message = reader["Message"].ToString();
                                string maKQNen = reader["MaKQNen"] == DBNull.Value ? "" : reader["MaKQNen"].ToString();
                                return (result, message, maKQNen);
                            }
                        }
                    }
                }
                return (false, "Không nhận được kết quả từ server.", "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message, "");
            }
        }

        /// <summary>
        /// Thêm chi tiết thông số đo
        /// </summary>
        public (bool Success, string Message) ThemKetQuaChiTiet(DTO_KetQuaChiTiet chiTiet)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemKetQuaChiTiet", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maKQNen", chiTiet.MaKQNen);
                        cmd.Parameters.AddWithValue("@maTS", chiTiet.MaTS);
                        cmd.Parameters.AddWithValue("@donVi", string.IsNullOrEmpty(chiTiet.DonVi) ? (object)DBNull.Value : chiTiet.DonVi);
                        cmd.Parameters.AddWithValue("@phuongPhapPhanTich", string.IsNullOrEmpty(chiTiet.PhuongPhapPhanTich) ? (object)DBNull.Value : chiTiet.PhuongPhapPhanTich);
                        cmd.Parameters.AddWithValue("@ketQua", chiTiet.KetQua);
                        cmd.Parameters.AddWithValue("@gioiHanPhatHien", string.IsNullOrEmpty(chiTiet.GioiHanPhatHien) ? (object)DBNull.Value : chiTiet.GioiHanPhatHien);
                        cmd.Parameters.AddWithValue("@qcvn", string.IsNullOrEmpty(chiTiet.QCVN) ? (object)DBNull.Value : chiTiet.QCVN);

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

        /// <summary>
        /// Xóa kết quả
        /// </summary>
        public (bool Success, string Message) XoaKetQua(string maKQ)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaKetQua", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@maKQ", maKQ);

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

        /// <summary>
        /// Lấy thông tin tổng quan của kết quả
        /// </summary>
        public DTO_KetQuaHeader LayThongTinKetQua(string maKQ)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LayThongTinKetQua", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maKQ", maKQ);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DTO_KetQuaHeader
                            {
                                MaKQ = reader["maKQ"].ToString(),
                                NgayTao = Convert.ToDateTime(reader["ngayTao"]),
                                NgayTraKQ = reader["ngayTraKQ"] == DBNull.Value ? null : Convert.ToDateTime(reader["ngayTraKQ"]),
                                TenNhanVien = reader["NguoiNhap"].ToString(),
                                TrangThaiXacNhan = Convert.ToBoolean(reader["trangThaiXacNhan"]),
                                GhiChu = reader["ghiChu"] == DBNull.Value ? "" : reader["ghiChu"].ToString(),
                                DotQuanTrac = reader["dotQuanTrac"] == DBNull.Value ? "" : reader["dotQuanTrac"].ToString(),
                                SoNenMau = Convert.ToInt32(reader["TongSoNenMau"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        private DataTable ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object>? parameters)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}