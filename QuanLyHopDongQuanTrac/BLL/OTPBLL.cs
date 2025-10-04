using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class OTPBLL
    {
        private readonly OTPDataAccess dal = new OTPDataAccess();

        // =============================================
        // GỬI OTP
        // =============================================
        public SendOTPResult GuiOTP(string contactInfo)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(contactInfo))
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = "Vui lòng nhập email hoặc số điện thoại"
                };
            }

            // Xác định là email hay số điện thoại
            bool isEmail = contactInfo.Contains("@");
            bool isPhone = Regex.IsMatch(contactInfo, @"^0\d{9}$");

            if (!isEmail && !isPhone)
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = "Email hoặc số điện thoại không hợp lệ"
                };
            }

            // Kiểm tra tồn tại trong hệ thống
            var (exists, tenTK) = dal.KiemTraContactTonTai(contactInfo);
            if (!exists)
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = $"{(isEmail ? "Email" : "Số điện thoại")} không tồn tại trong hệ thống"
                };
            }

            // Kiểm tra giới hạn gửi OTP (tối đa 5 lần/ngày)
            int soLanGuiHomNay = dal.LaySoLanGuiOTPHomNay(contactInfo);
            if (soLanGuiHomNay >= 5)
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = "Bạn đã gửi OTP quá 5 lần hôm nay. Vui lòng thử lại sau."
                };
            }

            // Tạo mã OTP 6 số ngẫu nhiên
            Random random = new Random();
            string otpCode = random.Next(100000, 999999).ToString();

            // Lưu OTP vào database
            bool saved = dal.LuuOTP(contactInfo, otpCode, expiryMinutes: 5);

            if (!saved)
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = "Lỗi lưu OTP. Vui lòng thử lại!"
                };
            }

            // Thành công
            return new SendOTPResult
            {
                Success = true,
                Message = "Gửi OTP thành công",
                OTPCode = otpCode, // Chỉ để demo, trong thực tế không trả về
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };
        }

        // =============================================
        // XÁC THỰC OTP
        // =============================================
        public OTPVerificationResult XacThucOTP(string contactInfo, string otpCode)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(contactInfo) || string.IsNullOrWhiteSpace(otpCode))
            {
                return new OTPVerificationResult
                {
                    IsValid = false,
                    Message = "Vui lòng nhập đầy đủ thông tin"
                };
            }

            // Kiểm tra OTP phải là 6 số
            if (!Regex.IsMatch(otpCode, @"^\d{6}$"))
            {
                return new OTPVerificationResult
                {
                    IsValid = false,
                    Message = "Mã OTP phải là 6 số"
                };
            }

            // Gọi DAL để xác thực
            return dal.XacThucOTP(contactInfo, otpCode);
        }

        // =============================================
        // ĐẶT LẠI MẬT KHẨU
        // =============================================
        public ResetPasswordResult DatLaiMatKhau(string contactInfo, string matKhauMoi)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = "Vui lòng nhập mật khẩu mới"
                };
            }

            // Kiểm tra độ dài mật khẩu
            if (matKhauMoi.Length < 6)
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = "Mật khẩu phải có ít nhất 6 ký tự"
                };
            }

            // Kiểm tra mật khẩu không chứa khoảng trắng
            if (Regex.IsMatch(matKhauMoi, @"\s"))
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = "Mật khẩu không được chứa khoảng trắng"
                };
            }

            try
            {
                // Tạo salt ngẫu nhiên
                string salt = GenerateSalt();

                // Hash mật khẩu với BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

                // Cập nhật vào database
                return dal.CapNhatMatKhau(contactInfo, hashedPassword, salt);
            }
            catch (Exception ex)
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }

        // =============================================
        // KIỂM TRA ĐỘ MẠNH MẬT KHẨU
        // =============================================
        public (bool isStrong, string message) KiemTraDoManhMatKhau(string password)
        {
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");

            if (password.Length >= 8 && hasUpper && hasLower && hasDigit)
            {
                return (true, "Mật khẩu mạnh");
            }
            else if (password.Length >= 6)
            {
                return (false, "Mật khẩu yếu. Nên có chữ hoa, chữ thường và số");
            }
            else
            {
                return (false, "Mật khẩu quá ngắn");
            }
        }

        // =============================================
        // TẠO SALT NGẪU NHIÊN
        // =============================================
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
