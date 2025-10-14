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

            List<NhanVien> dsNhanvien = new List<NhanVien>() ;
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
                                ngaySinh = Convert.ToDateTime( reader["ngaySinh"]),
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
                        // Hiển thị thông báo lỗi từ SQL (ví dụ: tuổi không hợp lệ, email trùng,...)
                        throw new Exception("Lỗi khi sửa nhân viên: " + ex.Message);
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


    }
}
