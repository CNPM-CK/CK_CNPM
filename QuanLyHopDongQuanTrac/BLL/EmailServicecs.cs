using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BLL
{
    public class EmailService
    {
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private const string EMAIL_FROM = "tritam199999@gmail.com";
        private const string EMAIL_PASSWORD = "kpeb kqhb ffpm kezz";
        private const string EMAIL_FROM_NAME = "ECOS System";

        // ================== GỬI OTP (Giữ nguyên) ==================

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
                    mail.Body = TaoNoiDungEmail(otpCode, expiryTime);

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000;
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
<head><meta charset='UTF-8'></head>
<body style='font-family: Segoe UI, Arial; background:#f4f4f4; padding:20px;'>
    <div style='max-width:600px; margin:0 auto; background:white; border-radius:10px; overflow:hidden; box-shadow:0 2px 10px rgba(0,0,0,0.1);'>
        <div style='background:linear-gradient(135deg,#009846,#00c853); color:white; padding:30px; text-align:center;'>
            <h1 style='margin:0; font-size:28px;'>🔐 Xác Thực Tài Khoản ECOS</h1>
        </div>
        <div style='padding:40px 30px;'>
            <p style='font-size:16px; color:#333;'>Xin chào,<br><br>Bạn đã yêu cầu đặt lại mật khẩu. Vui lòng sử dụng mã OTP bên dưới:</p>
            <div style='background:linear-gradient(135deg,#f5f5f5,#e8f5e9); border:2px dashed #009846; border-radius:10px; padding:30px; text-align:center; margin:30px 0;'>
                <p style='margin:0; color:#666; font-size:14px;'>MÃ XÁC THỰC CỦA BẠN</p>
                <div style='font-size:42px; font-weight:bold; color:#009846; letter-spacing:8px; margin:10px 0;'>{otpCode}</div>
            </div>
            <div style='background-color:#fff3cd; border-left:4px solid #ffc107; padding:15px; margin:20px 0; border-radius:5px;'>
                <strong>⏰ Thời gian còn lại:</strong> <span style='color:#d32f2f; font-weight:bold;'>{phutConLai} phút</span>
                <br><small>Mã OTP sẽ hết hạn lúc {expiryTime:HH:mm:ss dd/MM/yyyy}</small>
            </div>
        </div>
        <div style='background-color:#f5f5f5; padding:20px; text-align:center; color:#666; font-size:12px;'>
            © 2025 ECOS - Hệ thống quản lý hợp đồng quan trắc môi trường
        </div>
    </div>
</body>
</html>";
        }

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
<head><meta charset='UTF-8'></head>
<body style='font-family: Segoe UI, Arial; background:#f4f4f4; padding:20px;'>
    <div style='max-width:600px; margin:0 auto; background:white; border-radius:10px; overflow:hidden;'>
        <div style='background:linear-gradient(135deg,#009846,#00c853); color:white; padding:30px; text-align:center;'>
            <h1>✅ Đặt Lại Mật Khẩu Thành Công</h1>
        </div>
        <div style='padding:40px 30px;'>
            <div style='font-size:60px; text-align:center; margin:20px 0;'>✅</div>
            <p style='font-size:16px; color:#333;'>
                Xin chào <strong>{tenNguoiDung}</strong>,<br><br>
                Mật khẩu của bạn đã được đặt lại thành công vào lúc <strong>{DateTime.Now:HH:mm:ss dd/MM/yyyy}</strong>.
            </p>
        </div>
        <div style='background-color:#f5f5f5; padding:20px; text-align:center; color:#666; font-size:12px;'>
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
            catch { return false; }
        }

        // ================== GỬI BÁO CÁO PDF (MỚI) ==================

        /// <summary>
        /// Gửi email báo cáo kèm file PDF đính kèm (BẤT ĐỒNG BỘ)
        /// </summary>
        public async Task<bool> GuiEmailBaoCaoPdfAsync(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string pdfFilePath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = $"[ECOS] Báo cáo kết quả quan trắc - {maKQ}";
                    mail.IsBodyHtml = true;
                    mail.Body = TaoNoiDungEmailBaoCaoPdf(tenKhachHang, maKQ, dotQuanTrac);

                    // Đính kèm file PDF
                    if (!string.IsNullOrEmpty(pdfFilePath) && File.Exists(pdfFilePath))
                    {
                        string fileName = $"BaoCao_{maKQ}_{DateTime.Now:yyyyMMdd}.pdf";
                        Attachment attachment = new Attachment(pdfFilePath);
                        attachment.Name = fileName;
                        attachment.ContentType = new System.Net.Mime.ContentType("application/pdf");
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 60000; // 60 giây cho file PDF lớn

                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email báo cáo PDF: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gửi email báo cáo PDF đồng bộ
        /// </summary>
        public bool GuiEmailBaoCaoPdf(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string pdfFilePath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_FROM, EMAIL_FROM_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = $"[ECOS] Báo cáo kết quả quan trắc - {maKQ}";
                    mail.IsBodyHtml = true;
                    mail.Body = TaoNoiDungEmailBaoCaoPdf(tenKhachHang, maKQ, dotQuanTrac);

                    if (!string.IsNullOrEmpty(pdfFilePath) && File.Exists(pdfFilePath))
                    {
                        string fileName = $"BaoCao_{maKQ}_{DateTime.Now:yyyyMMdd}.pdf";
                        Attachment attachment = new Attachment(pdfFilePath);
                        attachment.Name = fileName;
                        attachment.ContentType = new System.Net.Mime.ContentType("application/pdf");
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 60000;
                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email báo cáo PDF: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Nội dung email khi gửi báo cáo PDF
        /// </summary>
        private string TaoNoiDungEmailBaoCaoPdf(string tenKhachHang, string maKQ, string dotQuanTrac)
        {
            return $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Segoe UI, Arial, sans-serif; background-color:#f4f4f4; margin:0; padding:20px;'>
    <div style='max-width:600px; margin:0 auto; background-color:white; border-radius:10px; overflow:hidden; box-shadow:0 2px 10px rgba(0,0,0,0.1);'>
        <div style='background:linear-gradient(135deg,#009846 0%,#00c853 100%); color:white; padding:30px; text-align:center;'>
            <h1 style='margin:0; font-size:28px;'>📊 BÁO CÁO KẾT QUẢ QUAN TRẮC</h1>
        </div>
        
        <div style='padding:40px 30px;'>
            <p style='font-size:16px; color:#333;'>
                Kính gửi: <strong>{tenKhachHang}</strong>,
            </p>
            
            <p style='font-size:15px; color:#555; line-height:1.6;'>
                ECOS xin gửi đến Quý khách hàng báo cáo kết quả quan trắc môi trường.
                Vui lòng xem chi tiết trong <strong>file PDF</strong> đính kèm.
            </p>
            
            <div style='background:linear-gradient(135deg,#f5f5f5 0%,#e8f5e9 100%); border:2px solid #009846; border-radius:10px; padding:25px; margin:20px 0;'>
                <div style='margin:10px 0; font-size:15px;'>
                    <span style='font-weight:bold; color:#009846;'>📋 Mã kết quả:</span> {maKQ}
                </div>
                <div style='margin:10px 0; font-size:15px;'>
                    <span style='font-weight:bold; color:#009846;'>📅 Đợt quan trắc:</span> {dotQuanTrac}
                </div>
                <div style='margin:10px 0; font-size:15px;'>
                    <span style='font-weight:bold; color:#009846;'>⏰ Ngày gửi:</span> {DateTime.Now:dd/MM/yyyy HH:mm:ss}
                </div>
            </div>
            
            <div style='background-color:#e3f2fd; border-left:4px solid #2196f3; padding:15px; margin:20px 0; border-radius:5px;'>
                <strong>📎 File đính kèm:</strong><br>
                <span style='color:#1976d2;'>📄 BaoCao_{maKQ}_{DateTime.Now:yyyyMMdd}.pdf</span><br>
                <small style='color:#666;'>File PDF có thể mở và in trực tiếp</small>
            </div>
            
            <div style='background-color:#e7f3ff; border-left:4px solid #0066cc; padding:15px; margin:20px 0; border-radius:5px; font-size:14px;'>
                <strong>📌 Lưu ý:</strong><br>
                • File báo cáo định dạng PDF, có thể mở bằng Adobe Reader hoặc trình duyệt<br>
                • Báo cáo đã được định dạng sẵn, có thể in trực tiếp<br>
                • Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi
            </div>
            
            <p style='margin-top:30px; color:#666; font-size:14px; line-height:1.6;'>
                <strong>Thông tin liên hệ:</strong><br>
                📞 Hotline: 1900 1234<br>
                📧 Email: ecos@gmail.com<br>
                🏢 Địa chỉ: 19 Nguyễn Hữu Thọ, phường Tân Hưng, TPHCM
            </p>
            
            <p style='margin-top:25px; font-size:14px;'>
                Trân trọng,<br>
                <strong>Đội ngũ ECOS</strong>
            </p>
        </div>
        
        <div style='background-color:#f5f5f5; padding:20px; text-align:center; color:#666; font-size:12px;'>
            © 2025 ECOS - Hệ thống quản lý hợp đồng quan trắc môi trường<br>
            Email này được gửi tự động từ hệ thống.
        </div>
    </div>
</body>
</html>";
        }

        // ================== GIỮ LẠI PHƯƠNG THỨC CŨ CHO TƯƠNG THÍCH ==================

        [Obsolete("Sử dụng GuiEmailBaoCaoPdfAsync thay thế")]
        public async Task<bool> GuiEmailBaoCaoAsync(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string filePath)
        {
            return await GuiEmailBaoCaoPdfAsync(emailTo, tenKhachHang, maKQ, dotQuanTrac, filePath);
        }

        [Obsolete("Sử dụng GuiEmailBaoCaoPdf thay thế")]
        public bool GuiEmailBaoCao(string emailTo, string tenKhachHang, string maKQ, string dotQuanTrac, string filePath)
        {
            return GuiEmailBaoCaoPdf(emailTo, tenKhachHang, maKQ, dotQuanTrac, filePath);
        }
    }
}