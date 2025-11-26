using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GUI.Forms
{
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

        /// <summary>
        /// Tạo GraphicsPath với các góc bo tròn tùy chọn
        /// </summary>
        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius,
            bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
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
            textBoxNhapMatKhauMoi.PasswordChar = '*';
            textBoxNhapLaiMatKhau.PasswordChar = '*';

            // Đặt UseSystemPasswordChar = false để hiển thị chữ thường khi nhập
            textBoxNhapMatKhauMoi.UseSystemPasswordChar = false;
            textBoxNhapLaiMatKhau.UseSystemPasswordChar = false;

            // Bo góc + căn giữa dọc cho 2 ô nhập giống form Đăng nhập
            ApplyRoundedInput(panelNhapMatKhauMoi, textBoxNhapMatKhauMoi, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNhapLaiMatKhau, textBoxNhapLaiMatKhau, 12, 2, Color.FromArgb(0, 152, 70));

            // Thêm căn giữa text bên trong TextBox (padding trên/dưới)
            CenterTextBoxVertically(textBoxNhapMatKhauMoi);
            CenterTextBoxVertically(textBoxNhapLaiMatKhau);

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

            // Load ảnh ban đầu (mắt đóng) cho CẢ 2 button
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

        // Căn giữa text trong TextBox theo chiều dọc (bằng EM_SETMARGINS)
        private void CenterTextBoxVertically(TextBox textBox)
        {
            int padding = (textBox.Height - textBox.Font.Height) / 2;

            if (padding < 0) padding = 0;

            // EM_SETMARGINS = 0xD3, set top/bottom margin
            SendMessage(textBox.Handle, 0xD3, (IntPtr)2, (IntPtr)(padding << 16 | padding));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void button1_Click(object sender, EventArgs e) // nút hoàn tất
        {
            // 1. Lấy dữ liệu từ 2 TextBox
            string matKhauMoi = textBoxNhapMatKhauMoi.Text.Trim();
            string xacNhanMatKhau = textBoxNhapLaiMatKhau.Text.Trim();

            // 2. Validate rỗng
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNhapMatKhauMoi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(xacNhanMatKhau))
            {
                MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNhapLaiMatKhau.Focus();
                return;
            }

            // 3. Validate độ dài
            if (matKhauMoi.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNhapMatKhauMoi.Focus();
                return;
            }

            // 4. Validate khớp nhau
            if (matKhauMoi != xacNhanMatKhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNhapLaiMatKhau.Clear();
                textBoxNhapLaiMatKhau.Focus();
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
                    textBoxNhapMatKhauMoi.PasswordChar = '\0';

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
                    textBoxNhapMatKhauMoi.PasswordChar = '*';

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
                    textBoxNhapLaiMatKhau.PasswordChar = '\0';

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
                    textBoxNhapLaiMatKhau.PasswordChar = '*';

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

        // ====== HÀM BO GÓC + CĂN GIỮA PANEL & TEXTBOX (copy từ DangNhap) ======
        private void ApplyRoundedInput(Panel panel, Control ctrl, int borderRadius, int borderSize, Color borderColor)
        {
            // Màu nền
            panel.BackColor = Color.White;
            ctrl.BackColor = Color.White;

            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.None;
                txt.Multiline = true; // để căn giữa dọc

                int textHeight = TextRenderer.MeasureText("Ag", txt.Font).Height + 4;
                txt.Height = textHeight;
            }
            else if (ctrl is ComboBox cbo)
            {
                cbo.FlatStyle = FlatStyle.Flat;
                if (cbo.DropDownStyle != ComboBoxStyle.DropDown)
                    cbo.DropDownStyle = ComboBoxStyle.DropDown;
            }

            // Căn giữa theo chiều dọc trong panel
            int yPos = (panel.Height - ctrl.Height) / 2;
            ctrl.Location = new Point(borderSize + 5, yPos);
            ctrl.Width = panel.Width - (borderSize + 5) * 2;
            ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            void Panel_Paint(object s, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                {
                    using (SolidBrush brush = new SolidBrush(panel.BackColor))
                        e.Graphics.FillPath(brush, path);

                    if (borderSize > 0)
                    {
                        using (GraphicsPath borderPath = CreateRoundedPath(
                            new Rectangle(
                                borderSize / 2,
                                borderSize / 2,
                                panel.Width - borderSize,
                                panel.Height - borderSize
                            ),
                            borderRadius))
                        {
                            using (Pen pen = new Pen(borderColor, borderSize))
                            {
                                pen.Alignment = PenAlignment.Inset;
                                e.Graphics.DrawPath(pen, borderPath);
                            }
                        }
                    }
                }
            }

            void Panel_Resize(object s, EventArgs e)
            {
                // Tính lại vị trí để căn giữa dọc khi resize
                int y = (panel.Height - ctrl.Height) / 2;
                ctrl.Location = new Point(borderSize + 5, y);
                ctrl.Width = panel.Width - (borderSize + 5) * 2;

                using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                {
                    panel.Region = new Region(path);
                }
                panel.Invalidate();
            }

            panel.Paint += Panel_Paint;
            panel.Resize += Panel_Resize;

            using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
            {
                panel.Region = new Region(path);
            }

            panel.Invalidate();
        }

        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
                return path;

            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
