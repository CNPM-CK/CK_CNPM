namespace GUI.Forms;
using BLL;
using DTO;
using System.Drawing.Drawing2D;

public partial class DatLaiMatKhau : Form
{
    private string soDienThoai;

    // Trạng thái hiển thị mật khẩu
    private bool isPasswordVisible1 = false; // Mật khẩu mới
    private bool isPasswordVisible2 = false; // Xác nhận mật khẩu

    // ResourceManager để lấy ảnh từ DatLaiMatKhau.resx
    private System.ComponentModel.ComponentResourceManager formResources;

    public DatLaiMatKhau()
    {
        InitializeComponent();

        // Khởi tạo ResourceManager
        formResources = new System.ComponentModel.ComponentResourceManager(typeof(DatLaiMatKhau));

        SetupRoundedPanels();
    }

    public DatLaiMatKhau(string sdt)
    {
        InitializeComponent();
        soDienThoai = sdt;

        // Khởi tạo ResourceManager
        formResources = new System.ComponentModel.ComponentResourceManager(typeof(DatLaiMatKhau));

        SetupRoundedPanels();
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

    /// Tạo GraphicsPath với các góc bo tròn tùy chọn
    private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius, bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
    {
        GraphicsPath path = new GraphicsPath();
        int diameter = radius * 2;

        // Góc trên trái
        if (topLeft)
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
        else
            path.AddLine(bounds.X, bounds.Y, bounds.X, bounds.Y);

        // Cạnh trên
        path.AddLine(topLeft ? bounds.X + radius : bounds.X, bounds.Y,
                    topRight ? bounds.Right - radius : bounds.Right, bounds.Y);

        // Góc trên phải
        if (topRight)
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
        else
            path.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y);

        // Cạnh phải
        path.AddLine(bounds.Right, topRight ? bounds.Y + radius : bounds.Y,
                    bounds.Right, bottomRight ? bounds.Bottom - radius : bounds.Bottom);

        // Góc dưới phải
        if (bottomRight)
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        else
            path.AddLine(bounds.Right, bounds.Bottom, bounds.Right, bounds.Bottom);

        // Cạnh dưới
        path.AddLine(bottomRight ? bounds.Right - radius : bounds.Right, bounds.Bottom,
                    bottomLeft ? bounds.X + radius : bounds.X, bounds.Bottom);

        // Góc dưới trái
        if (bottomLeft)
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        else
            path.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Bottom);

        // Cạnh trái
        path.AddLine(bounds.X, bottomLeft ? bounds.Bottom - radius : bounds.Bottom,
                    bounds.X, topLeft ? bounds.Y + radius : bounds.Y);

        path.CloseFigure();
        return path;
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void DatLaiMatKhau_Load(object sender, EventArgs e)
    {
        // Thiết lập PasswordChar ban đầu - hiển thị dấu * khi nhập
        textBox1.PasswordChar = '*';
        textBox2.PasswordChar = '*';

        // Đặt UseSystemPasswordChar = false để hiển thị chữ thường khi nhập
        textBox1.UseSystemPasswordChar = false;
        textBox2.UseSystemPasswordChar = false;

        // Cấu hình button2 (Ẩn/hiện mật khẩu mới)
        button2.FlatStyle = FlatStyle.Flat;
        button2.FlatAppearance.BorderSize = 0;
        button2.BackColor = Color.Transparent;
        button2.Cursor = Cursors.Hand;
        button2.BackgroundImageLayout = ImageLayout.Zoom;

        // Cấu hình button3 (Ẩn/hiện xác nhận mật khẩu)
        button3.FlatStyle = FlatStyle.Flat;
        button3.FlatAppearance.BorderSize = 0;
        button3.BackColor = Color.Transparent;
        button3.Cursor = Cursors.Hand;
        button3.BackgroundImageLayout = ImageLayout.Zoom;

        // Load ảnh ban đầu (mắt đóng) cho CÁ 2 button
        try
        {
            var closeEyeImg = formResources.GetObject("closeeye");

            if (closeEyeImg != null)
            {
                button2.BackgroundImage = (Image)closeEyeImg;
                button3.BackgroundImage = (Image)((Image)closeEyeImg).Clone();
            }
            else
            {
                MessageBox.Show(
                    "Không tìm thấy ảnh 'closeeye' trong DatLaiMatKhau.resx!\n\n" +
                    "Vui lòng thêm ảnh vào Resources với tên 'closeeye' và 'openeye'.",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi load ảnh: {ex.Message}", "Lỗi");
        }
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
            textBox1.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(xacNhanMatKhau))
        {
            MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox2.Focus();
            return;
        }

        // 3. Validate độ dài
        if (matKhauMoi.Length < 6)
        {
            MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox1.Focus();
            return;
        }

        // 4. Validate khớp nhau
        if (matKhauMoi != xacNhanMatKhau)
        {
            MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox2.Clear();
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
            OTPBLL otpBll = new OTPBLL();
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

    private void button2_Click(object sender, EventArgs e)
    {
        try
        {
            // Đổi trạng thái
            isPasswordVisible1 = !isPasswordVisible1;

            if (isPasswordVisible1)
            {
                // HIỂN THỊ mật khẩu - cho phép xem chữ thường
                textBox1.PasswordChar = '\0';

                // Lấy ảnh mắt mở từ DatLaiMatKhau.resx
                var img = formResources.GetObject("openeye");
                if (img != null)
                {
                    button2.BackgroundImage = (Image)img;
                }
            }
            else
            {
                // ẨN mật khẩu - hiển thị dấu *
                textBox1.PasswordChar = '*';

                // Lấy ảnh mắt đóng từ DatLaiMatKhau.resx
                var img = formResources.GetObject("closeeye");
                if (img != null)
                {
                    button2.BackgroundImage = (Image)img;
                }
            }

            // Force refresh button
            button2.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
        }
    }

    private void button3_Click(object sender, EventArgs e)
    {
        try
        {
            // Đổi trạng thái
            isPasswordVisible2 = !isPasswordVisible2;

            if (isPasswordVisible2)
            {
                // HIỂN THỊ mật khẩu - cho phép xem chữ thường
                textBox2.PasswordChar = '\0';

                // Lấy ảnh mắt mở từ DatLaiMatKhau.resx
                var img = formResources.GetObject("openeye");
                if (img != null)
                {
                    button3.BackgroundImage = (Image)img;
                }
            }
            else
            {
                // ẨN mật khẩu - hiển thị dấu *
                textBox2.PasswordChar = '*';

                // Lấy ảnh mắt đóng từ DatLaiMatKhau.resx
                var img = formResources.GetObject("closeeye");
                if (img != null)
                {
                    button3.BackgroundImage = (Image)img;
                }
            }

            // Force refresh button
            button3.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
        }
    }

    private void panel2_Paint(object sender, PaintEventArgs e)
    {

    }
}