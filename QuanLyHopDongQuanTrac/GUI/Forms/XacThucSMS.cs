using BLL;
using DTO;
namespace GUI.Forms
{
    public partial class XacThucSMS : Form
    {
        private readonly OTPBLL otpBLL = new OTPBLL();
        private string soDienThoai;
        public XacThucSMS()
        {
            InitializeComponent();
        }
        public XacThucSMS(string sdt)
        {
            InitializeComponent();
            soDienThoai = sdt;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox? current = sender as TextBox;
            if (current != null)
            {
                // Khi vừa nhập 1 ký tự thì nhảy sang textbox kế tiếp
                if (current.Text.Length == 1)
                {
                    this.SelectNextControl(current, true, true, true, true);
                }
            }
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox? current = sender as TextBox;
            if (current != null)
            {
                if (e.KeyCode == Keys.Back && current.Text.Length == 0)
                {
                    this.SelectNextControl(current, false, true, true, true);
                }
            }
        }


        private string LayMaOTP()
        {
            // Thay txtOTP1, txtOTP2... bằng tên 6 TextBox của bạn
            return textBox1.Text + textBox2.Text + textBox3.Text +
                   textBox4.Text + textBox5.Text + textBox6.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void XacThucSMS_Load(object sender, EventArgs e)
        {
            // Gán sự kiện TextChanged cho 6 textbox
            textBox1.TextChanged += TextBox_TextChanged;
            textBox2.TextChanged += TextBox_TextChanged;
            textBox3.TextChanged += TextBox_TextChanged;
            textBox4.TextChanged += TextBox_TextChanged;
            textBox5.TextChanged += TextBox_TextChanged;
            textBox6.TextChanged += TextBox_TextChanged;

            textBox1.KeyDown += TextBox_KeyDown;
            textBox2.KeyDown += TextBox_KeyDown;
            textBox3.KeyDown += TextBox_KeyDown;
            textBox4.KeyDown += TextBox_KeyDown;
            textBox5.KeyDown += TextBox_KeyDown;
            textBox6.KeyDown += TextBox_KeyDown;

            // Giới hạn mỗi textbox chỉ nhập 1 ký tự
            textBox1.MaxLength = 1;
            textBox2.MaxLength = 1;
            textBox3.MaxLength = 1;
            textBox4.MaxLength = 1;
            textBox5.MaxLength = 1;
            textBox6.MaxLength = 1;
        }

        private void button1_Click(object sender, EventArgs e) //nút xác nhận
        {
            // 1. Lấy mã OTP từ 6 TextBox
            string otpCode = LayMaOTP();

            // 2. Validate
            if (otpCode.Length != 6)
            {
                MessageBox.Show("Vui lòng nhập đủ 6 số OTP!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Disable button
            button1.Enabled = false;
            button1.Text = "Đang xác thực...";

            try
            {
                // 4. Xác thực OTP
                OTPVerificationResult result = otpBLL.XacThucOTP(soDienThoai, otpCode);

                if (result.IsValid)
                {
                    // ✅ OTP ĐÚNG
                    MessageBox.Show("✅ Xác thực thành công!\n\nBạn có thể đặt lại mật khẩu.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Chuyển sang form đặt lại mật khẩu
                    DatLaiMatKhau resetForm = new DatLaiMatKhau(soDienThoai);
                    resetForm.Show();
                    this.Close();
                }
                else
                {
                    // ❌ OTP SAI
                    MessageBox.Show(
                        $"❌ {result.Message}\n\nSố lần nhập sai: {result.FailedAttempts}/5",
                        "Xác thực thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Xóa OTP
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Xác nhận";
            }
        }

        private void button2_Click(object sender, EventArgs e) // nút gửi lại mã OTP
        {
            DialogResult confirm = MessageBox.Show("Bạn muốn gửi lại mã OTP?",
        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    SendOTPResult result = otpBLL.GuiOTP(soDienThoai);

                    if (result.Success)
                    {
                        MessageBox.Show(
                            $"📱 ĐÃ GỬI LẠI MÃ OTP\n\nMã OTP mới: {result.OTPCode}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Xóa OTP cũ
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox1.Focus();
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
