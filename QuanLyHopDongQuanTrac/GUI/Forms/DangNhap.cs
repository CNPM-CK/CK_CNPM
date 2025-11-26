using BLL;
using Emgu.CV;
using Emgu.CV.Structure;
using GUI.Common;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DangNhap : Form
    {
        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private readonly NhanDienKhuonMatBLL nhanDienKhuonMatBLL = new NhanDienKhuonMatBLL();
        private bool isPasswordVisible = false;

        private readonly string rememberFilePath = Path.Combine(Application.StartupPath, "remember.txt");

        private System.ComponentModel.ComponentResourceManager formResources;

        public DangNhap()
        {
            InitializeComponent();

            formResources = new System.ComponentModel.ComponentResourceManager(typeof(DangNhap));

            this.AcceptButton = button1;
            textBoxmatkhau.KeyDown += textBoxMatKhau_KeyDown;
        }

        private void LuuThongTinDangNhap(string username, string password)
        {
            try
            {
                // Mã hóa đơn giản bằng Base64
                string encodedUsername = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

                string content = $"{encodedUsername}|{encodedPassword}";
                File.WriteAllText(rememberFilePath, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin: {ex.Message}", "Lỗi");
            }
        }

        private void TaiThongTinDangNhap()
        {
            try
            {
                if (File.Exists(rememberFilePath))
                {
                    string content = File.ReadAllText(rememberFilePath);
                    string[] parts = content.Split('|');

                    if (parts.Length == 2)
                    {
                        // Giải mã Base64
                        string username = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));
                        string password = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));

                        txtTentk.Text = username;
                        textBoxmatkhau.Text = password;
                        checkBox1.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu file bị lỗi, xóa file và bỏ qua
                if (File.Exists(rememberFilePath))
                {
                    File.Delete(rememberFilePath);
                }
            }
        }

        private void XoaThongTinDangNhap()
        {
            try
            {
                if (File.Exists(rememberFilePath))
                {
                    File.Delete(rememberFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa thông tin: {ex.Message}", "Lỗi");
            }
        }

        // ===== SỬA LỖI 2: Thêm // trước comment block =====
        // ===== FACE ID LOGIN =====
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string tenTaiKhoan = txtTentk.Text.Trim();

                if (string.IsNullOrEmpty(tenTaiKhoan))
                {
                    MessageBox.Show(
                        "Vui lòng nhập tên tài khoản trước khi sử dụng Face ID!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtTentk.Focus();
                    return;
                }

                var account = taiKhoanBLL.layThongTinTaiKhoan(tenTaiKhoan);
                if (account == null)
                {
                    MessageBox.Show(
                        "Tài khoản không tồn tại!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (!nhanDienKhuonMatBLL.KiemTraKhuonMatDaTonTai(tenTaiKhoan))
                {
                    MessageBox.Show(
                        "Tài khoản này chưa đăng ký Face ID!\n\n" +
                        "Vui lòng sử dụng mật khẩu để đăng nhập hoặc đăng ký Face ID trước.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                using (NhanDienKhuonMat faceLoginForm = new NhanDienKhuonMat(tenTaiKhoan))
                {
                    DialogResult dlg = faceLoginForm.ShowDialog();

                    if (dlg == DialogResult.OK && faceLoginForm.NhanDienThanhCong)
                    {
                        SessionStore.Current.SignIn(
                            account.tenTK,
                            account.vaiTro
                        );
                        Debug.WriteLine(account.vaiTro);
                        if (account.vaiTro != 1 && account.vaiTro != 2)
                        {
                            var nvBLL = new NhanVienBLL();
                            string maPhong = nvBLL.layPhongBanTheoTaiKhoan(account.tenTK);
                            if (string.IsNullOrEmpty(maPhong))
                            {
                                MessageBox.Show(
                                    "Không tìm thấy phòng ban cho nhân viên này!",
                                    "Lỗi dữ liệu",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }
                            SessionStore.Current.MaPhong = maPhong;
                        }

                        TrangChu trangChu = new TrangChu();
                        trangChu.FormClosed += (s, _) => this.Close();
                        trangChu.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi đăng nhập bằng Face ID: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DieuHuongTheoVaiTro(dynamic account)
        {
            if (account.vaiTro == 1 || account.vaiTro == 2)
            {
                DanhSachNhanVien listEmployees = new DanhSachNhanVien();
                listEmployees.Show();
                this.Hide();
            }
            else
            {
                var nvBLL = new NhanVienBLL();
                string maPhong = nvBLL.layPhongBanTheoTaiKhoan(account.tenTK);

                if (string.IsNullOrEmpty(maPhong))
                {
                    MessageBox.Show(
                        "Không tìm thấy phòng ban cho nhân viên này!",
                        "Lỗi dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Form formPhong = null;

                switch (maPhong)
                {
                    case "P001":
                        formPhong = new DanhSachKhachHang();
                        break;
                    //case "P002":
                    //    formPhong = new DanhSachKeHoach();
                    //    break;
                    default:
                        MessageBox.Show(
                            "Phòng ban chưa được hỗ trợ!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return;
                }

                if (formPhong != null)
                {
                    formPhong.Show();
                    this.Hide();
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Đổi trạng thái
                isPasswordVisible = !isPasswordVisible;

                if (isPasswordVisible)
                {
                    // HIỂN THỊ mật khẩu
                    textBoxmatkhau.PasswordChar = '\0';

                    // Lấy ảnh từ DangNhap.resx
                    var img = formResources.GetObject("openeye");
                    if (img != null)
                    {
                        button4.BackgroundImage = (Image)img;
                    }
                }
                else
                {
                    // ẨN mật khẩu
                    textBoxmatkhau.PasswordChar = '*';

                    // Lấy ảnh từ DangNhap.resx
                    var img = formResources.GetObject("closeeye");
                    if (img != null)
                    {
                        button4.BackgroundImage = (Image)img;
                    }
                }

                // Force refresh button
                button4.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string username = txtTentk.Text.Trim();
        //    string password = textBoxmatkhau.Text.Trim();

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        MessageBox.Show(
        //            "Vui lòng nhập tên tài khoản!",
        //            "Cảnh báo",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Warning);
        //        txtTentk.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(password))
        //    {
        //        MessageBox.Show(
        //            "Vui lòng nhập mật khẩu!",
        //            "Cảnh báo",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Warning);
        //        textBoxmatkhau.Focus();
        //        return;
        //    }

        //    var result = taiKhoanBLL.dangNhap(username, password);

        //    if (!result.success)
        //    {
        //        MessageBox.Show(
        //            result.message,
        //            "Đăng nhập thất bại",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //        return;
        //    }

        //    // XỬ LÝ GHI NHỚ ĐĂNG NHẬP
        //    if (checkBox1.Checked)
        //    {
        //        LuuThongTinDangNhap(username, password);
        //    }
        //    else
        //    {
        //        XoaThongTinDangNhap();
        //    }

        //    DieuHuongTheoVaiTro(result.account);
        //}

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
            SessionStore.Current.SignIn(
                result.account!.tenTK,
                result.account!.vaiTro
            );
            Debug.WriteLine(result.account.vaiTro);
            if (result.account!.vaiTro != 1 && result.account!.vaiTro != 2)
            {
                var nvBLL = new NhanVienBLL();
                string maPhong = nvBLL.layPhongBanTheoTaiKhoan(result.account.tenTK);
                if (string.IsNullOrEmpty(maPhong))
                {
                    MessageBox.Show("Không tìm thấy phòng ban cho nhân viên này!",
                                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SessionStore.Current.MaPhong = maPhong;
            }
            //Form next = CreateNextFormFromSession();
            //next.FormClosed += (s, _) => this.Close();
            //next.Show();
            //this.Hide();
            TrangChu trangChu = new TrangChu();
            trangChu.FormClosed += (s, _) => this.Close();
            trangChu.Show();
            this.Hide();

        }

        private void textBoxMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QuenMatKhau1 quenMKForm = new QuenMatKhau1();
            quenMKForm.Show();
            this.Hide();
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
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;

            panel.BackColor = Color.White;
            ctrl.BackColor = Color.White;

            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.None;
                txt.Multiline = true; // Cho phép căn giữa dọc

                // Tính chiều cao phù hợp
                int textHeight = TextRenderer.MeasureText("Ag", txt.Font).Height + 4;
                txt.Height = textHeight;
            }
            else if (ctrl is ComboBox cbo)
            {
                cbo.FlatStyle = FlatStyle.Flat;
                if (cbo.DropDownStyle != ComboBoxStyle.DropDown)
                    cbo.DropDownStyle = ComboBoxStyle.DropDown;
            }
            // Căn giữa theo chiều dọc
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
                                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                                e.Graphics.DrawPath(pen, borderPath);
                            }
                        }
                    }
                }
            }

            void Panel_Resize(object s, EventArgs e)
            {
                // Tính lại vị trí để căn giữa dọc khi resize
                int yPos = (panel.Height - ctrl.Height) / 2;
                ctrl.Location = new Point(borderSize + 5, yPos);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            BoGocButton(button1, 25);

            ApplyRoundedInput(panelTentk, txtTentk, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMatkhau, textBoxmatkhau, 12, 2, Color.FromArgb(0, 152, 70));

            textBoxmatkhau.PasswordChar = '*';

            BoGocPanel(panel2, 30);
            BoGocPanel(panel3, 30);

            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.BackColor = Color.Transparent;
            button3.ForeColor = Color.Green;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button3.Cursor = Cursors.Hand;
            button3.TextAlign = ContentAlignment.MiddleCenter;

            // Cấu hình button4 - Nút hiện/ẩn mật khẩu
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.BackColor = Color.Transparent;
            button4.Cursor = Cursors.Hand;
            button4.BackgroundImageLayout = ImageLayout.Zoom;

            // Load ảnh ban đầu từ DangNhap.resx
            try
            {
                var img = formResources.GetObject("closeeye");
                if (img != null)
                {
                    button4.BackgroundImage = (Image)img;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ảnh 'closeeye' trong DangNhap.resx!\n\nVui lòng kiểm tra lại tên ảnh.", "Cảnh báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load ảnh closeeye: {ex.Message}", "Lỗi");
            }

            // TẢI THÔNG TIN ĐĂNG NHẬP ĐÃ LƯU (NẾU CÓ)
            TaiThongTinDangNhap();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }

    }
}