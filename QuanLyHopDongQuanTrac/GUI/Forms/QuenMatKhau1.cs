using BLL;
using DTO;
using System;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class QuenMatKhau1 : Form
    {
        private readonly OTPBLL otpBLL = new OTPBLL();

        public QuenMatKhau1()
        {
            InitializeComponent();
            SetupRoundedPanels();

            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;
        }

        private void SetupRoundedPanels()
        {
            // Bo 2 góc trên của panel2
            panel2.Paint += (s, e) =>
            {
                GraphicsPath path = GetRoundedRectangle(panel2.ClientRectangle, 30, true, true, false, false);
                panel2.Region = new Region(path);
            };

            // Bo 2 góc dưới của panel3
            panel3.Paint += (s, e) =>
            {
                GraphicsPath path = GetRoundedRectangle(panel3.ClientRectangle, 30, false, false, true, true);
                panel3.Region = new Region(path);
            };
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius, bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (topLeft)
                path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            else
                path.AddLine(bounds.X, bounds.Y, bounds.X, bounds.Y);

            path.AddLine(topLeft ? bounds.X + radius : bounds.X, bounds.Y,
                        topRight ? bounds.Right - radius : bounds.Right, bounds.Y);

            if (topRight)
                path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            else
                path.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y);

            path.AddLine(bounds.Right, topRight ? bounds.Y + radius : bounds.Y,
                        bounds.Right, bottomRight ? bounds.Bottom - radius : bounds.Bottom);

            if (bottomRight)
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            else
                path.AddLine(bounds.Right, bounds.Bottom, bounds.Right, bounds.Bottom);

            path.AddLine(bottomRight ? bounds.Right - radius : bounds.Right, bounds.Bottom,
                        bottomLeft ? bounds.X + radius : bounds.X, bounds.Bottom);

            if (bottomLeft)
                path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            else
                path.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Bottom);

            path.AddLine(bounds.X, bottomLeft ? bounds.Bottom - radius : bounds.Bottom,
                        bounds.X, topLeft ? bounds.Y + radius : bounds.Y);

            path.CloseFigure();
            return path;
        }

        private void QuenMatKhau1_Load(object sender, EventArgs e)
        {
            // Cập nhật text label hướng dẫn
            label2.Text = "Nhập email hoặc số điện thoại đã đăng ký";

            button1.Enabled = true;
            button1.Text = "Gửi mã xác nhận";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string contactInfo = textBox1.Text.Trim();

            // 1. Validate input
            if (string.IsNullOrWhiteSpace(contactInfo))
            {
                MessageBox.Show(
                    "Vui lòng nhập email hoặc số điện thoại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            button1.Enabled = false;
            button1.Text = "Đang gửi...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // 3. Gọi API gửi OTP (bất đồng bộ)
                SendOTPResult result = await otpBLL.GuiOTPAsync(contactInfo);

                if (result.Success)
                {
                    // THÀNH CÔNG - KHÔNG HIỂN THỊ MÃ OTP
                    string messageType = contactInfo.Contains("@") ? "email" : "số điện thoại";

                    MessageBox.Show(
                        $"✅ ĐÃ GỬI MÃ OTP\n\n" +
                        $"Mã OTP đã được gửi đến {messageType} của bạn.\n" +
                        $"Vui lòng kiểm tra {(contactInfo.Contains("@") ? "hộp thư email" : "tin nhắn")}!\n\n" +
                        $"⏰ Có hiệu lực trong 5 phút.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Chuyển sang form xác thực OTP
                    XacThucSMS verifyForm = new XacThucSMS(contactInfo);
                    verifyForm.Show();
                    this.Close();
                }
                else
                {
                    // ❌ THẤT BẠI
                    MessageBox.Show(
                        $"❌ {result.Message}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi hệ thống: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // 4. Enable lại button
                button1.Enabled = true;
                button1.Text = "Gửi mã xác nhận";
                this.Cursor = Cursors.Default;
            }
        }

        // SỬA LẠI - Đổi tên hàm cho đúng với button2
        private void button2_Click(object sender, EventArgs e)
        {
            DangNhap loginForm = new DangNhap();
            loginForm.Show();
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}