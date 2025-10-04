using BLL;
using DTO;
namespace GUI.Forms
{
    public partial class QuenMatKhau1 : Form
    {
        private readonly OTPBLL otpBLL = new OTPBLL();
        public QuenMatKhau1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) //nút quay lại đăng nhập
        {
            DangNhap loginForm = new DangNhap();
            loginForm.Show();
            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void QuenMatKhau1_Load(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e) // nút xác nhận sdt
        //{
        //    string soDienThoai = textBox1.Text.Trim();

        //    // Validate rỗng
        //    if (string.IsNullOrWhiteSpace(soDienThoai))
        //    {
        //        MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
        //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Validate định dạng số điện thoại VN
        //    if (!System.Text.RegularExpressions.Regex.IsMatch(soDienThoai, @"^0\d{9}$"))
        //    {
        //        MessageBox.Show("Số điện thoại không hợp lệ!\nVui lòng nhập 10 số, bắt đầu bằng 0.",
        //            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Disable button
        //    button2.Enabled = false;  // ← ĐỔI button2 thành tên button của bạn
        //    button2.Text = "Đang gửi...";

        //    try
        //    {
        //        // Gọi BLL để gửi OTP
        //        SendOTPResult result = otpBLL.GuiOTP(soDienThoai);

        //        if (!result.Success)
        //        {
        //            MessageBox.Show(result.Message, "Lỗi",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        // THÀNH CÔNG - Hiển thị OTP
        //        MessageBox.Show(
        //            $"📱 MÃ OTP CỦA BẠN\n\n" +
        //            $"Số điện thoại: {soDienThoai}\n" +
        //            $"Mã OTP: {result.OTPCode}\n" +
        //            $"Có hiệu lực: 5 phút\n\n" +
        //            $"(Trong thực tế, mã này sẽ được gửi qua SMS)",
        //            "Thông báo OTP",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Information
        //        );

        //        // CHUYỂN SANG FORM XacThucSMS
        //        XacThucSMS xacThucForm = new XacThucSMS(soDienThoai);
        //        xacThucForm.Show();
        //        this.Hide();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        button2.Enabled = true;  // ← ĐỔI button2
        //        button2.Text = "Xác nhận";
        //    }
        //}

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

        private void button1_Click_1(object sender, EventArgs e)
        {
            string soDienThoai = textBox1.Text.Trim();  // ← ĐỔI textBox1 thành tên TextBox nhập SĐT của bạn
                                                        // ==================================================

            // Validate rỗng
            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate định dạng số điện thoại VN
            if (!System.Text.RegularExpressions.Regex.IsMatch(soDienThoai, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!\nVui lòng nhập 10 số, bắt đầu bằng 0.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable button
            button2.Enabled = false;  // ← ĐỔI button2 thành tên button của bạn
            button2.Text = "Đang gửi...";

            try
            {
                // Gọi BLL để gửi OTP
                SendOTPResult result = otpBLL.GuiOTP(soDienThoai);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // THÀNH CÔNG - Hiển thị OTP
                MessageBox.Show(
                    $"📱 MÃ OTP CỦA BẠN\n\n" +
                    $"Số điện thoại: {soDienThoai}\n" +
                    $"Mã OTP: {result.OTPCode}\n" +
                    $"Có hiệu lực: 5 phút",
                    "Thông báo OTP",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // CHUYỂN SANG FORM XacThucSMS
                XacThucSMS xacThucForm = new XacThucSMS(soDienThoai);
                xacThucForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button2.Enabled = true;  // ← ĐỔI button2
                button2.Text = "Xác nhận";
            }
        }
    }
}
