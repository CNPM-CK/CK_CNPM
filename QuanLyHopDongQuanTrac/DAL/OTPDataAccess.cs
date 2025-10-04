using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class OTPDataAccess
    {
        // =============================================
        // KIỂM TRA EMAIL/SĐT TỒN TẠI
        // =============================================
        public (bool exists, string tenTK) KiemTraContactTonTai(string contactInfo)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_KiemTraContactTonTai", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);

                    SqlParameter tonTaiParam = new SqlParameter("@TonTai", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(tonTaiParam);

                    SqlParameter tenTKParam = new SqlParameter("@TenTK", SqlDbType.NVarChar, 30)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(tenTKParam);

                    cmd.ExecuteNonQuery();

                    bool exists = (bool)tonTaiParam.Value;
                    string tenTK = tenTKParam.Value == DBNull.Value ? null : tenTKParam.Value.ToString();

                    return (exists, tenTK);
                }
            }
        }

        // =============================================
        // LƯU OTP VÀO DATABASE
        // =============================================
        public bool LuuOTP(string contactInfo, string otpCode, int expiryMinutes = 5)
        {
            try
            {
                using (SqlConnection conn = SqlConnectionData.Connect())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LuuOTP", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                        cmd.Parameters.AddWithValue("@OTPCode", otpCode);
                        cmd.Parameters.AddWithValue("@ExpiryMinutes", expiryMinutes);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // =============================================
        // XÁC THỰC OTP
        // =============================================
        public OTPVerificationResult XacThucOTP(string contactInfo, string otpCode)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_XacThucOTP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                    cmd.Parameters.AddWithValue("@OTPCode", otpCode);

                    SqlParameter isValidParam = new SqlParameter("@IsValid", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(isValidParam);

                    SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(messageParam);

                    SqlParameter failedParam = new SqlParameter("@FailedAttempts", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(failedParam);

                    cmd.ExecuteNonQuery();

                    return new OTPVerificationResult
                    {
                        IsValid = (bool)isValidParam.Value,
                        Message = messageParam.Value.ToString(),
                        FailedAttempts = failedParam.Value == DBNull.Value ? 0 : (int)failedParam.Value
                    };
                }
            }
        }

        // =============================================
        // CẬP NHẬT MẬT KHẨU
        // =============================================
        public ResetPasswordResult CapNhatMatKhau(string contactInfo, string matKhauMoi, string salt)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CapNhatMatKhau", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                    cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                    cmd.Parameters.AddWithValue("@Salt", salt);

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

                    return new ResetPasswordResult
                    {
                        Success = (bool)successParam.Value,
                        Message = messageParam.Value.ToString()
                    };
                }
            }
        }

        // =============================================
        // KIỂM TRA SỐ LẦN GỬI OTP HÔM NAY
        // =============================================
        public int LaySoLanGuiOTPHomNay(string contactInfo)
        {
            using (SqlConnection conn = SqlConnectionData.Connect())
            {
                conn.Open();
                string query = @"
                    SELECT COUNT(*) 
                    FROM OTPVerification 
                    WHERE ContactInfo = @ContactInfo 
                    AND CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
