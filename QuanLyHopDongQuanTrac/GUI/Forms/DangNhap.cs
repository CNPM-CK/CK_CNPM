using BLL;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GUI.Forms
{
    public partial class DangNhap : Form
    {

        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private bool isPasswordVisible = false; // THÊM DÒNG NÀY

        public DangNhap()
        {
            InitializeComponent();
            this.AcceptButton = button1;
            textBoxmatkhau.KeyDown += textBoxMatKhau_KeyDown;
            button4.Click += button4_Click; // THÊM DÒNG NÀY
        }

        // THÊM HÀM NÀY
        private void button4_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Hiện mật khẩu
                textBoxmatkhau.PasswordChar = '\0';
                button4.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("openeye");
            }
            else
            {
                // Ẩn mật khẩu
                textBoxmatkhau.PasswordChar = '*';
                button4.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("closeeye");
            }
        }

        private void BoGocButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, btn.Width - radius, 0);
            path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), -90, 90);
            path.AddLine(btn.Width, radius, btn.Width, btn.Height - radius);
            path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
            path.AddLine(btn.Width - radius, btn.Height, radius, btn.Height);
            path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void BoGocPanel(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), -90, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void ApplyRoundedInput(Panel panel, Control ctrl, int borderRadius, int borderSize, Color borderColor)
        {
            // Gỡ event cũ (tránh vẽ chồng)
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;

            // Cài đặt nền và kiểu cho control
            panel.BackColor = Color.White;
            ctrl.BackColor = Color.White;

            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.None;
            }
            else if (ctrl is ComboBox cbo)
            {
                cbo.FlatStyle = FlatStyle.Flat;
                if (cbo.DropDownStyle != ComboBoxStyle.DropDown)
                    cbo.DropDownStyle = ComboBoxStyle.DropDown;
            }

            // Căn chỉnh vị trí & kích thước control con trong panel
            ctrl.Location = new Point(borderSize + 5, (panel.Height - ctrl.Height) / 2);
            ctrl.Width = panel.Width - (borderSize + 5) * 2;
            ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            // Hàm vẽ bo tròn
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
                                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                                e.Graphics.DrawPath(pen, borderPath);
                            }
                        }
                    }
                }
            }

            void Panel_Resize(object s, EventArgs e)
            {
                ctrl.Location = new Point(borderSize + 5, (panel.Height - ctrl.Height) / 2);
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
                panel.Region = new Region(path);

            panel.Invalidate();
        }


        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
                return path;

            //int diameter = radius * 2;
            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            // Đảm bảo radius không lớn hơn kích thước
            diameter = Math.Min(diameter, Math.Min(rect.Width, rect.Height));

            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Top left
            path.AddArc(arc, 180, 90);

            // Top right
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // ô nhập tài khoản
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e) // ô nhập mật khẩu
        {
        }

        private void button1_Click(object sender, EventArgs e) // nút đăng nhập
        {
            string username = txtTentk.Text.Trim();
            string password = textBoxmatkhau.Text.Trim();

            var result = taiKhoanBLL.dangNhap(username, password);
            if (!result.success)
            {
                MessageBox.Show(result.message, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Chuyển hướng nếu vai trò là admin
            if (result.account!.vaiTro == 1 || result.account!.vaiTro == 2)
            {

                DanhSachNhanVien listEmployees = new DanhSachNhanVien();
                listEmployees.Show();
                this.Hide();
            }
            else
            {
                var nvBLL = new NhanVienBLL();
                string maPhong = nvBLL.layPhongBanTheoTaiKhoan(result.account.tenTK);

                if (string.IsNullOrEmpty(maPhong))
                {
                    MessageBox.Show("Không tìm thấy phòng ban cho nhân viên này!",
                                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Form formPhong = null;

                switch (maPhong)
                {
                    case "P001":
                        formPhong = new DanhSachKhachHang();
                        break;
                    case "P002":
                        formPhong = new DanhSachKeHoach();
                        break;
                    //case "P003":
                    //    formPhong = new PhongHienTruongForm();
                    //    break;
                    //case "P004":
                    //    formPhong = new PhongThiNghiemForm();
                    //    break;
                    //case "P005":
                    //    formPhong = new PhongKetQuaForm();
                    //    break;
                    //case "P006":
                    //    formPhong = new PhongQuanTracForm();
                    //    break;
                    default:
                        MessageBox.Show("Phòng ban chưa được hỗ trợ!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                }

                if (formPhong != null)
                {
                    formPhong.Show();
                    this.Hide();
                }
            }


        }

        private void textBoxMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Bo góc button đăng nhập
            BoGocButton(button1, 25);

            // Áp dụng bo góc cho input tài khoản và mật khẩu
            ApplyRoundedInput(panelTentk, txtTentk, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMatkhau, textBoxmatkhau, 12, 2, Color.FromArgb(0, 152, 70));

            // Ẩn mật khẩu thành dấu *
            textBoxmatkhau.PasswordChar = '*';

            // Bo góc panel bên phải (trắng)
            BoGocPanel(panel2, 30);

            // Bo góc panel bên trái (xanh) - panel3
            BoGocPanel(panel3, 30);

            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.BackColor = Color.Transparent;
            button3.ForeColor = Color.Green; // Màu chữ xanh
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button3.Cursor = Cursors.Hand;
            button3.TextAlign = ContentAlignment.MiddleCenter;

            // THÊM PHẦN NÀY - Cấu hình button4
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.BackColor = Color.Transparent;
            button4.Cursor = Cursors.Hand;
            button4.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("closeeye");
            button4.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void button3_Click(object sender, EventArgs e) // nút quên mật khẩu
        {
            QuenMatKhau1 quenMKForm = new QuenMatKhau1();
            quenMKForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}