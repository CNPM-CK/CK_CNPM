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
                            var nv = new NhanVien
                            {
                                maNV = reader["maNV"].ToString(),
                                maPhong = reader["maPhong"].ToString(),
                                tenPhong = reader["tenPhong"].ToString(),
                                hoTen = reader["hoTen"].ToString(),
                                ngaySinh = Convert.ToDateTime( reader["ngaySinh"]),
                                gioiTinh = reader["gioiTinh"].ToString(),
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
    }
}
