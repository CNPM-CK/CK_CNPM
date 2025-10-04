using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class OTPVerification
    {
        public int ID { get; set; }
        public string ContactInfo { get; set; }
        public string OTPCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // =============================================
    // CLASS KẾT QUẢ XÁC THỰC OTP
    // =============================================
    public class OTPVerificationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public int FailedAttempts { get; set; }
    }

    // =============================================
    // CLASS KẾT QUẢ GỬI OTP
    // =============================================
    public class SendOTPResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OTPCode { get; set; }  // Chỉ dùng cho demo
        public DateTime ExpiryTime { get; set; }
    }

    // =============================================
    // CLASS KẾT QUẢ ĐẶT LẠI MẬT KHẨU
    // =============================================
    public class ResetPasswordResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    // =============================================
    // CLASS LỊCH SỬ ĐẶT LẠI MẬT KHẨU
    // =============================================
    public class PasswordResetHistory
    {
        public int ID { get; set; }
        public string TenTK { get; set; }
        public string ContactInfo { get; set; }
        public string ResetMethod { get; set; }
        public DateTime ResetTime { get; set; }
        public bool Success { get; set; }
    }
}
