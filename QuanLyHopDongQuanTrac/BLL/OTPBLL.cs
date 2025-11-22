using DAL;
using DTO;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class OTPBLL
    {
        private readonly OTPDataAccess dal = new OTPDataAccess();
        private readonly EmailService emailService = new EmailService();

        /// <summary>
        /// Gửi OTP qua Email (đồng bộ)
        /// </summary>
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

            // 🆕 GỬI EMAIL OTP
            if (isEmail)
            {
                DateTime expiryTime = DateTime.Now.AddMinutes(5);
                bool emailSent = emailService.GuiEmailOTP(contactInfo, otpCode, expiryTime);

                if (!emailSent)
                {
                    return new SendOTPResult
                    {
                        Success = false,
                        Message = "Không thể gửi email. Vui lòng kiểm tra lại địa chỉ email!"
                    };
                }

                return new SendOTPResult
                {
                    Success = true,
                    Message = $"Mã OTP đã được gửi đến email {contactInfo}",
                    OTPCode = null, // ✅ Không trả về mã OTP
                    ExpiryTime = expiryTime
                };
            }
            else
            {
                // Trường hợp SMS (giữ nguyên logic cũ)
                return new SendOTPResult
                {
                    Success = true,
                    Message = "Gửi OTP thành công (SMS giả lập)",
                    // ✅ ĐÃ XÓA: OTPCode = otpCode
                    ExpiryTime = DateTime.Now.AddMinutes(5)
                };
            }
        }

        /// <summary>
        /// Gửi OTP qua Email (bất đồng bộ) - KHUYẾN NGHỊ SỬ DỤNG
        /// </summary>
        public async Task<SendOTPResult> GuiOTPAsync(string contactInfo)
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

            // Kiểm tra giới hạn gửi OTP
            int soLanGuiHomNay = dal.LaySoLanGuiOTPHomNay(contactInfo);
            if (soLanGuiHomNay >= 5)
            {
                return new SendOTPResult
                {
                    Success = false,
                    Message = "Bạn đã gửi OTP quá 5 lần hôm nay. Vui lòng thử lại sau."
                };
            }

            // Tạo mã OTP
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

            // 🆕 GỬI EMAIL OTP BẤT ĐỒNG BỘ
            if (isEmail)
            {
                DateTime expiryTime = DateTime.Now.AddMinutes(5);
                bool emailSent = await emailService.GuiEmailOTPAsync(contactInfo, otpCode, expiryTime);

                if (!emailSent)
                {
                    return new SendOTPResult
                    {
                        Success = false,
                        Message = "Không thể gửi email. Vui lòng kiểm tra lại!"
                    };
                }

                return new SendOTPResult
                {
                    Success = true,
                    Message = $"Mã OTP đã được gửi đến email {contactInfo}. Vui lòng kiểm tra hộp thư!",
                    // ✅ ĐÃ XÓA: OTPCode = otpCode (Không trả về mã OTP)
                    ExpiryTime = expiryTime
                };
            }
            else
            {
                return new SendOTPResult
                {
                    Success = true,
                    Message = "Gửi SMS OTP thành công",
                    // ✅ ĐÃ XÓA: OTPCode = otpCode
                    ExpiryTime = DateTime.Now.AddMinutes(5)
                };
            }
        }

        /// <summary>
        /// Xác thực OTP (giữ nguyên)
        /// </summary>
        public OTPVerificationResult XacThucOTP(string contactInfo, string otpCode)
        {
            if (string.IsNullOrWhiteSpace(contactInfo) || string.IsNullOrWhiteSpace(otpCode))
            {
                return new OTPVerificationResult
                {
                    IsValid = false,
                    Message = "Vui lòng nhập đầy đủ thông tin"
                };
            }

            if (!Regex.IsMatch(otpCode, @"^\d{6}$"))
            {
                return new OTPVerificationResult
                {
                    IsValid = false,
                    Message = "Mã OTP phải là 6 số"
                };
            }

            return dal.XacThucOTP(contactInfo, otpCode);
        }

        /// <summary>
        /// Đặt lại mật khẩu (giữ nguyên)
        /// </summary>
        public ResetPasswordResult DatLaiMatKhau(string contactInfo, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = "Vui lòng nhập mật khẩu mới"
                };
            }

            if (matKhauMoi.Length < 6)
            {
                return new ResetPasswordResult
                {
                    Success = false,
                    Message = "Mật khẩu phải có ít nhất 6 ký tự"
                };
            }

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
                string salt = GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

                var result = dal.CapNhatMatKhau(contactInfo, hashedPassword);

                // 🆕 GỬI EMAIL THÔNG BÁO THÀNH CÔNG
                if (result.Success && contactInfo.Contains("@"))
                {
                    _ = emailService.GuiEmailThanhCongAsync(contactInfo, "Người dùng");
                }

                return result;
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

        public (bool isStrong, string message) KiemTraDoManhMatKhau(string password)
        {
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

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