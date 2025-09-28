using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;

namespace DAL
{
    public class SqlConnectionData
    {
        public static SqlConnection Connect()
        {
            string connectionStr = "Data Source=ThaiQuangTran\\SQLEXPRESS;Initial Catalog=QuanLyHopDongQuanTrac;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
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
    }
}
