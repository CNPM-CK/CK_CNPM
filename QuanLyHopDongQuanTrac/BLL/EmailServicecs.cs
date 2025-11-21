using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace BLL
{
    public class EmailService
    {
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private const string EMAIL_FROM = "tritam199999@gmail.com"; // Email gửi đi
        private const string EMAIL_PASSWORD = "kpeb kqhb ffpm kezz"; // Mật khẩu ứng dụng
        private const string EMAIL_FROM_NAME = "ECOS System";

        public bool GuiEmailOTP(string emailTo, string otpCode, DateTime expiryTime)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = "[ECOS] Mã xác thực OTP của bạn";
                    mail.IsBodyHtml = true;

                    // Template email đẹp
                    mail.Body = TaoNoiDungEmail(otpCode, expiryTime);

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000; // 10 giây

                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> GuiEmailOTPAsync(string emailTo, string otpCode, DateTime expiryTime)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = "[ECOS] Mã xác thực OTP của bạn";
                    mail.IsBodyHtml = true;
                    mail.Body = TaoNoiDungEmail(otpCode, expiryTime);

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000;

                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }

        private string TaoNoiDungEmail(string otpCode, DateTime expiryTime)
        {
            int phutConLai = (int)(expiryTime - DateTime.Now).TotalMinutes;

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: white;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .header {{
            background: linear-gradient(135deg, #009846 0%, #00c853 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .otp-box {{
            background: linear-gradient(135deg, #f5f5f5 0%, #e8f5e9 100%);
            border: 2px dashed #009846;
            border-radius: 10px;
            padding: 30px;
            text-align: center;
            margin: 30px 0;
        }}
        .otp-code {{
            font-size: 42px;
            font-weight: bold;
            color: #009846;
            letter-spacing: 8px;
            margin: 10px 0;
        }}
        .warning {{
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
        }}
        .footer {{
            background-color: #f5f5f5;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
        }}
        .time-remaining {{
            color: #d32f2f;
            font-weight: bold;
            font-size: 18px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🔐 Xác Thực Tài Khoản ECOS</h1>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px; color: #333;'>
                Xin chào,<br><br>
                Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản ECOS của mình. 
                Vui lòng sử dụng mã OTP bên dưới để xác thực:
            </p>
            
            <div class='otp-box'>
                <p style='margin: 0; color: #666; font-size: 14px;'>MÃ XÁC THỰC CỦA BẠN</p>
                <div class='otp-code'>{otpCode}</div>
                <p style='margin: 10px 0 0 0; color: #999; font-size: 13px;'>
                    (Mã gồm 6 chữ số)
                </p>
            </div>
            
            <div class='warning'>
                <strong>⏰ Thời gian còn lại:</strong> 
                <span class='time-remaining'>{phutConLai} phút</span>
                <br>
                <small>Mã OTP sẽ hết hạn lúc {expiryTime:HH:mm:ss dd/MM/yyyy}</small>
            </div>
            
            <p style='color: #666; font-size: 14px; line-height: 1.6;'>
                <strong>⚠️ Lưu ý quan trọng:</strong><br>
                • Không chia sẻ mã này với bất kỳ ai<br>
                • ECOS sẽ không bao giờ yêu cầu mã OTP qua điện thoại<br>
                • Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này
            </p>
            
            <p style='margin-top: 30px; color: #999; font-size: 13px;'>
                Nếu bạn gặp vấn đề, vui lòng liên hệ bộ phận hỗ trợ ECOS.
            </p>
        </div>
        
        <div class='footer'>
            © 2025 ECOS - Hệ thống quản lý hợp đồng quan trắc môi trường<br>
            Email này được gửi tự động, vui lòng không trả lời.
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Gửi email thông báo đặt lại mật khẩu thành công
        /// </summary>
        public async Task<bool> GuiEmailThanhCongAsync(string emailTo, string tenNguoiDung)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = "[ECOS] Đặt lại mật khẩu thành công";
                    mail.IsBodyHtml = true;
                    mail.Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #009846 0%, #00c853 100%); color: white; padding: 30px; text-align: center; }}
        .content {{ padding: 40px 30px; }}
        .success-icon {{ font-size: 60px; text-align: center; margin: 20px 0; }}
        .footer {{ background-color: #f5f5f5; padding: 20px; text-align: center; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✅ Đặt Lại Mật Khẩu Thành Công</h1>
        </div>
        <div class='content'>
            <div class='success-icon'>✅</div>
            <p style='font-size: 16px; color: #333;'>
                Xin chào <strong>{tenNguoiDung}</strong>,<br><br>
                Mật khẩu của bạn đã được đặt lại thành công vào lúc <strong>{DateTime.Now:HH:mm:ss dd/MM/yyyy}</strong>.
            </p>
            <p style='color: #d32f2f; font-size: 14px; background-color: #ffebee; padding: 15px; border-radius: 5px;'>
                <strong>⚠️ Nếu bạn không thực hiện thay đổi này:</strong><br>
                Vui lòng liên hệ ngay với bộ phận hỗ trợ để bảo vệ tài khoản của bạn.
            </p>
        </div>
        <div class='footer'>
            © 2025 ECOS - Hệ thống quản lý hợp đồng quan trắc môi trường
        </div>
    </div>
</body>
</html>";

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Gửi email báo cáo kèm file HTML đính kèm (BẤT ĐỒNG BỘ)
        /// </summary>
        public async Task<bool> GuiEmailBaoCaoAsync(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string filePath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = $"[ECOS] Báo cáo kết quả quan trắc - {maKQ}";
                    mail.IsBodyHtml = true;
                    mail.Body = TaoNoiDungEmailBaoCao(tenKhachHang, maKQ, dotQuanTrac);

                    // Đính kèm file báo cáo
                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    {
                        string fileName = $"BaoCao_{maKQ}_{DateTime.Now:yyyyMMdd}.html";
                        Attachment attachment = new Attachment(filePath);
                        attachment.Name = fileName;
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 30000; // 30 giây cho file lớn

                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email báo cáo: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gửi email báo cáo đồng bộ (nếu cần)
        /// </summary>
        public bool GuiEmailBaoCao(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string filePath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = $"[ECOS] Báo cáo kết quả quan trắc - {maKQ}";
                    mail.IsBodyHtml = true;
                    mail.Body = TaoNoiDungEmailBaoCao(tenKhachHang, maKQ, dotQuanTrac);

                    // Đính kèm file báo cáo
                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    {
                        string fileName = $"BaoCao_{maKQ}_{DateTime.Now:yyyyMMdd}.html";
                        Attachment attachment = new Attachment(filePath);
                        attachment.Name = fileName;
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 30000;

                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email báo cáo: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tạo nội dung HTML cho email báo cáo
        /// </summary>
        private string TaoNoiDungEmailBaoCao(string tenKhachHang, string maKQ, string dotQuanTrac)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: white;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .header {{
            background: linear-gradient(135deg, #009846 0%, #00c853 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .info-box {{
            background: linear-gradient(135deg, #f5f5f5 0%, #e8f5e9 100%);
            border: 2px solid #009846;
            border-radius: 10px;
            padding: 25px;
            margin: 20px 0;
        }}
        .info-item {{
            margin: 10px 0;
            font-size: 15px;
        }}
        .info-label {{
            font-weight: bold;
            color: #009846;
        }}
        .attachment-box {{
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
        }}
        .note {{
            background-color: #e7f3ff;
            border-left: 4px solid #0066cc;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
            font-size: 14px;
        }}
        .footer {{
            background-color: #f5f5f5;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
        }}
        .btn {{
            display: inline-block;
            background-color: #009846;
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 5px;
            margin: 15px 0;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📊 BÁO CÁO KẾT QUẢ QUAN TRẮC</h1>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px; color: #333;'>
                Kính gửi: <strong>{tenKhachHang}</strong>,
            </p>
            
            <p style='font-size: 15px; color: #555; line-height: 1.6;'>
                ECOS xin gửi đến Quý khách hàng báo cáo kết quả quan trắc môi trường.
                Vui lòng xem chi tiết trong file đính kèm.
            </p>
            
            <div class='info-box'>
                <div class='info-item'>
                    <span class='info-label'>📋 Mã kết quả:</span> {maKQ}
                </div>
                <div class='info-item'>
                    <span class='info-label'>📅 Đợt quan trắc:</span> {dotQuanTrac}
                </div>
                <div class='info-item'>
                    <span class='info-label'>⏰ Ngày gửi:</span> {DateTime.Now:dd/MM/yyyy HH:mm:ss}
                </div>
            </div>
            
            <div class='attachment-box'>
                <strong>📎 File đính kèm:</strong><br>
                Báo cáo chi tiết kết quả quan trắc (định dạng HTML)<br>
                <small>Vui lòng tải file đính kèm để xem báo cáo đầy đủ</small>
            </div>
            
            <div class='note'>
                <strong>📌 Lưu ý:</strong><br>
                • File báo cáo có thể mở bằng trình duyệt web (Chrome, Firefox, Edge...)<br>
                • Để in báo cáo, vui lòng sử dụng chức năng Print trong trình duyệt<br>
                • Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi
            </div>
            
            <p style='margin-top: 30px; color: #666; font-size: 14px; line-height: 1.6;'>
                <strong>Thông tin liên hệ:</strong><br>
                📞 Hotline: 1900 1234<br>
                📧 Email: ecos@gmail.com<br>
                🏢 Địa chỉ: 19 Nguyễn Hữu Thọ, phường Tân Hưng, TPHCM
            </p>
            
            <p style='margin-top: 25px; font-size: 14px;'>
                Trân trọng,<br>
                <strong>Đội ngũ ECOS</strong>
            </p>
        </div>
        
        <div class='footer'>
            © 2025 ECOS - Hệ thống quản lý hợp đồng quan trắc môi trường<br>
            Email này được gửi tự động từ hệ thống.<br>
            Vui lòng không trả lời trực tiếp email này.
        </div>
    </div>
</body>
</html>";
        }
    }
}