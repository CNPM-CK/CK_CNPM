using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DAL
{
    public class NhanDienKhuonMatDAL
    {
        /// <summary>
        /// Lưu dữ liệu khuôn mặt vào database
        /// </summary>
        public (bool thanhCong, string thongBao) LuuDuLieuKhuonMat(string tenTK, byte[] duLieuKhuonMat)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LuuFaceData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@tenTK", tenTK);
                        cmd.Parameters.AddWithValue("@faceData", duLieuKhuonMat);

                        // Output parameters
                        SqlParameter successParam = new SqlParameter("@Success", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(successParam);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        bool ketQua = Convert.ToBoolean(successParam.Value);
                        string thongBao = messageParam.Value?.ToString() ?? "Không có thông báo";

                        return (ketQua, thongBao);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối database: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy dữ liệu khuôn mặt của 1 tài khoản
        /// </summary>
        public byte[] LayDuLieuKhuonMat(string tenTK)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LayFaceDataTheoTK", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenTK", tenTK);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("faceData")))
                            {
                                return (byte[])reader["faceData"];
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LayDuLieuKhuonMat Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu khuôn mặt để so sánh
        /// </summary>
        public DataTable LayTatCaDuLieuKhuonMat()
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LayTatCaFaceData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LayTatCaDuLieuKhuonMat Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra tài khoản đã đăng ký Face ID chưa
        /// </summary>
        public bool KiemTraDaDangKyFace(string tenTK)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_KiemTraFaceDataTonTai", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenTK", tenTK);

                        SqlParameter tonTaiParam = new SqlParameter("@TonTai", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(tonTaiParam);

                        cmd.ExecuteNonQuery();

                        return Convert.ToBoolean(tonTaiParam.Value);
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Chuyển hình ảnh thành mảng byte (PNG format)
        /// </summary>
        public byte[] ChuyenHinhAnhThanhMangByte(Image hinhAnh)
        {
            if (hinhAnh == null)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    hinhAnh.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ChuyenHinhAnhThanhMangByte Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Chuyển mảng byte thành hình ảnh
        /// </summary>
        public Image ChuyenMangByteThanhHinhAnh(byte[] mangByte)
        {
            if (mangByte == null || mangByte.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(mangByte))
                {
                    // Clone image để tránh lỗi khi stream bị dispose
                    return (Image)Image.FromStream(ms).Clone();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ChuyenMangByteThanhHinhAnh Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Xóa dữ liệu Face của tài khoản
        /// </summary>
        public (bool thanhCong, string thongBao) XoaDuLieuKhuonMat(string tenTK)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaFaceData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenTK", tenTK);

                        SqlParameter successParam = new SqlParameter("@Success", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(successParam);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        bool ketQua = Convert.ToBoolean(successParam.Value);
                        string thongBao = messageParam.Value?.ToString() ?? "Không có thông báo";

                        return (ketQua, thongBao);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
    }
}