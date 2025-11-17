using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using DAL;
using DTO;

namespace BLL
{
    public class ThongBaoBLL
    {
        private readonly DatabaseAccess dal = new DatabaseAccess();

        /// <summary>
        /// Gọi stored procedure để kiểm tra các đợt quan trắc quá hạn
        /// và tự động sinh thông báo vào bảng ThongBao.
        /// </summary>
        public void kiemTraVaSinhThongBaoQuaHan()
        {
            dal.kiemTraQuaHan();
        }

        public DataTable layDanhSachThongBao()
        {
            return dal.layDanhSachThongBao();
        }
        

        public void guiEmailCanhBao(string maDot, string tenKH, int soNgayTre)
        {
            List<string> truongPhongEmails = dal.layEmailTruongPhong();

            if (truongPhongEmails.Count == 0)
                return;



            // 🔹 Lấy toàn bộ nội dung thông báo chi tiết của đợt
            DataTable dtThongBao = dal.layThongBaoTheoDot(maDot);
            if (dtThongBao.Rows.Count == 0)
                return;

            if (dtThongBao.Rows.Count == 0)
            {
                return;
            }

            DataRow tb = dtThongBao.Rows[0];
            string maTB = tb["maTB"].ToString();
            string maHD = tb["maHD"].ToString();
            string tieuDe = tb["tieuDe"].ToString();
            string noiDung = tb["noiDung"].ToString();
            string ngayTao = Convert.ToDateTime(tb["ngayTao"]).ToString("dd/MM/yyyy");
            string loaiTB = tb["loaiTB"].ToString();

            string subject = $"📢 [Cảnh báo] Đợt quan trắc {maDot} - {tenKH} đã quá hạn {soNgayTre} ngày";

            string body = $@"
<div style='font-family:Segoe UI,Arial,sans-serif; color:#333; line-height:1.6;'>
    <h2 style='color:#d93025;'>⚠️ Cảnh báo đợt quan trắc quá hạn</h2>
    <p>Hệ thống phát hiện một đợt quan trắc đã quá hạn. Dưới đây là thông tin chi tiết:</p>

    <table style='border-collapse:collapse; width:100%; margin:15px 0; font-size:14px;'>
        <tr style='background-color:#f0f0f0;'>
            <th style='border:1px solid #ddd; padding:8px;'>Mã thông báo</th>
            <td style='border:1px solid #ddd; padding:8px;'>{maTB}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Mã đợt</th>
            <td style='border:1px solid #ddd; padding:8px;'>{maDot}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Khách hàng</th>
            <td style='border:1px solid #ddd; padding:8px;'>{tenKH}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Mã hợp đồng</th>
            <td style='border:1px solid #ddd; padding:8px;'>{maHD}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Tiêu đề</th>
            <td style='border:1px solid #ddd; padding:8px;'>{tieuDe}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Nội dung</th>
            <td style='border:1px solid #ddd; padding:8px;'>{noiDung}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Ngày tạo</th>
            <td style='border:1px solid #ddd; padding:8px;'>{ngayTao}</td>
        </tr>
        <tr>
            <th style='border:1px solid #ddd; padding:8px;'>Loại thông báo</th>
            <td style='border:1px solid #ddd; padding:8px;'>{loaiTB}</td>
        </tr>
        <tr style='background-color:#ffe8e8;'>
            <th style='border:1px solid #ddd; padding:8px;'>Số ngày trễ</th>
            <td style='border:1px solid #ddd; padding:8px; color:red; font-weight:bold;'>{soNgayTre} ngày</td>
        </tr>
    </table>

    <p>Vui lòng kiểm tra tiến độ và hoàn tất báo cáo trong thời gian sớm nhất.</p>

    <hr style='margin-top:20px;'/>
    <p style='font-size:12px;color:#777;'>
        Email được gửi tự động từ <b>Hệ thống Quan trắc môi trường</b>.<br/>
        Vui lòng không trả lời email này.
    </p>
</div>";

            foreach (string email in truongPhongEmails)
            {
                guiEmail(email, subject, body);
            }
            Console.WriteLine($"✅ HOÀN TẤT gửi email cho đợt {maDot}");
        }



        private void guiEmail(string to, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("dhzhhxhddgh@gmail.com", "Hệ thống Quan trắc");
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true; 

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("dhzhhxhddgh@gmail.com", "nitvqgclplgtzcjo");
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000;
                        smtp.Send(mail);
                    }
                }

                Console.WriteLine($"✅ Gửi mail tới {to} thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi gửi mail: {ex.Message}");
            }
        }


        public void capNhatTrangThaiEmail(string maDot)
        {
            dal.capNhatTrangThaiEmail(maDot);
        }

        public DataTable layThongBaoTheoNhanVien(string maNV)
        => dal.layThongBaoTheoNhanVien(maNV);

        public void danhDauThongBaoDaDoc(string maTB, string maNV)
            => dal.danhDauThongBaoDaDoc(maTB, maNV);

        public void xoaThongBaoNguoiDung(string maTB, string maNV)
            => dal.xoaThongBaoNguoiDung(maTB, maNV);

        public int demThongBaoChuaDoc(string maNV)
            => dal.demThongBaoChuaDoc(maNV);


        public void kiemTraHopDongQuaHan()
        {
            dal.kiemTraHopDongQuaHan();

            DataTable ds = dal.layDanhSachThongBaoHopDongQuaHan();
            foreach (DataRow r in ds.Rows)
            {
                string maTB = r["maTB"].ToString();
                string maHD = r["maHD"].ToString();
                string tenKH = r["tenKhachHang"].ToString();

                guiEmailCanhBaoHopDong(maHD, tenKH);

                // 🔥 Quan trọng: đánh dấu đã gửi email
                dal.capNhatTrangThaiEmailHopDong(maTB);
            }
        }


        public void guiEmailCanhBaoHopDong(string maHD, string tenKH)
        {
            List<string> truongPhongEmails = dal.layEmailTruongPhong();
            if (truongPhongEmails.Count == 0) return;

            string subject = $"⚠️ Hợp đồng {maHD} đã quá hạn!";
            string body = $"Hợp đồng của khách hàng {tenKH} đã quá hạn và chưa được cập nhật trạng thái.";

            foreach (var email in truongPhongEmails)
            {
                guiEmail(email, subject, body);
            }
        }

        public DataTable layThongBaoTheoNhanVien_PhanTrang(string maNV, int pageNumber, int pageSize)
        {
            DataTable dt = dal.layThongBaoTheoNhanVien_PhanTrang(maNV, pageNumber, pageSize);
            chuyenDoiLoaiThongBao(dt);

            return dt;
        }

        public int demTongSoThongBao(string maNV)
        {
            return dal.demTongSoThongBao(maNV);
        }

        private void chuyenDoiLoaiThongBao(DataTable dt)
        {
            if (dt == null || !dt.Columns.Contains("loaiTB")) return;

            foreach (DataRow row in dt.Rows)
            {
                string loaiTB = row["loaiTB"]?.ToString();

                switch (loaiTB)
                {
                    case "QUA_HAN_DOT":
                        row["loaiTB"] = "Trễ hạn quan trắc";
                        break;
                    case "HOP_DONG_QUA_HAN":
                        row["loaiTB"] = "Hợp đồng quá hạn";
                        break;
                    // Thêm các loại khác nếu cần
                    case "THONG_BAO_CHUNG":
                        row["loaiTB"] = "Thông báo chung";
                        break;
                        // ... các loại khác
                }
            }
        }

        public void sinhThongBaoNhoKyHopDong()
        {
            dal.kiemTraVaSinhThongBaoNhacKyHopDong();
        }

        public DataTable layDanhSachNhacKyHopDong()
        {
            return dal.layDSNhacKyHopDong();
        }

        public void capNhatEmailDaGui_NhacHD(string maTB)
        {
            dal.capNhatTrangThaiEmailNhacHD(maTB);
        }

        public void guiEmailNhacHopDong(string to, string tenKH, string maHD, DateTime ngayBatDau, string tanSuat)
        {
            // Xác định chu kỳ
            string chuKy = "";

            if (tanSuat == "TSQT03") // Quý
            {
                int quy = (ngayBatDau.Month - 1) / 3 + 1;
                chuKy = $"Quý {quy}";
            }
            else if (tanSuat == "TSQT02") // 6 tháng
            {
                chuKy = ngayBatDau.Month <= 6 ? "Kỳ 1 (6 tháng đầu năm)" : "Kỳ 2 (6 tháng cuối năm)";
            }
            else
            {
                chuKy = "chu kỳ quan trắc";
            }

            string subject = $"📢 Nhắc lịch ký hợp đồng quan trắc mới ";

            string body = $@"
<div style='font-family:Segoe UI,Arial,sans-serif; color:#333; line-height:1.6;'>
    <h2 style='color:#00796b;'>🌿 Nhắc lịch ký hợp đồng quan trắc môi trường</h2>

    <p>Kính gửi <b>{tenKH}</b>,</p>

    <p>Đợt quan trắc môi trường của Quý khách trong <b>{chuKy}</b> đã hoàn tất.</p>

    <p>
        Để đảm bảo việc tuân thủ quy định về quan trắc môi trường và không bị gián đoạn 
        trong chu kỳ tiếp theo, kính mong Quý khách vui lòng liên hệ lại với 
        <b>Phòng Kinh Doanh</b> để tiến hành ký hợp đồng mới.
    </p>

    <p>Chúng tôi luôn sẵn sàng hỗ trợ Quý khách trong mọi thắc mắc.</p>

    <p>Trân trọng,</p>

    <p><b>Hệ thống Quan trắc môi trường ECOS</b></p>
    <hr/>
    <p style='font-size:12px;color:#777;'>Email được gửi tự động. Vui lòng không phản hồi email này.</p>
</div>";

            guiEmail(to, subject, body);
        }


    }
}
