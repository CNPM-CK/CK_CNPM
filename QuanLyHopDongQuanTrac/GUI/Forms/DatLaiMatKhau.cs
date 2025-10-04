namespace GUI.Forms;
using BLL;
using DTO;
public partial class DatLaiMatKhau : Form
{
    private string soDienThoai;
    public DatLaiMatKhau()
    {
        InitializeComponent();
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    public DatLaiMatKhau(string sdt)
    {
        InitializeComponent();
        soDienThoai = sdt;
    }

    private void DatLaiMatKhau_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e) // nút hoàn tất
    {
        // 1. Lấy dữ liệu từ 2 TextBox
        string matKhauMoi = textBox1.Text.Trim();
        string xacNhanMatKhau = textBox2.Text.Trim();

        // 2. Validate rỗng
        if (string.IsNullOrWhiteSpace(matKhauMoi))
        {
            MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(xacNhanMatKhau))
        {
            MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 3. Validate độ dài
        if (matKhauMoi.Length < 6)
        {
            MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 4. Validate khớp nhau
        if (matKhauMoi != xacNhanMatKhau)
        {
            MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox1.Clear();
            textBox2.Focus();
            return;
        }

        // 5. Xác nhận cuối
        DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn đặt lại mật khẩu?",
            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.No)
            return;

        // 6. Disable button
        button1.Enabled = false;
        button1.Text = "Đang xử lý...";

        try
        {
            // 7. Gọi BLL để đặt lại mật khẩu
            OTPBLL otpBll = new OTPBLL(); // Create an instance of OTPBLL
            ResetPasswordResult resetResult = otpBll.DatLaiMatKhau(soDienThoai, matKhauMoi);

            if (resetResult.Success)
            {
                // ✅ THÀNH CÔNG
                MessageBox.Show(
                    "✅ Đặt lại mật khẩu thành công!\n\nBạn có thể đăng nhập với mật khẩu mới.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Quay lại trang đăng nhập
                DangNhap loginForm = new DangNhap();
                loginForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"❌ {resetResult.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            button1.Text = "Hoàn tất";
        }
    }
}

