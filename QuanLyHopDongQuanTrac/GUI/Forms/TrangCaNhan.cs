using BLL;
using DTO;
using GUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class TrangCaNhan : UserControl
    {
        public static event Action anhDaiDienDaThayDoi;

        private string maNVHienTai;
        private string anhDaiDienHienTai;
        public TrangCaNhan()
        {
            InitializeComponent();
        }
        #region Custom TextBox và Label cho Form Nhân viên
        private void InitializeButtonStyles()
        {

            BoGocButton(btnLuu, 18);
            BoGocButton(btnQuenmk, 18);

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
                //if (cbo.DropDownStyle != ComboBoxStyle.DropDown)
                //    cbo.DropDownStyle = ComboBoxStyle.DropDown;
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
        #endregion


        private void TrangCaNhan_Load(object sender, EventArgs e)
        {
            InitializeButtonStyles();
            ApplyRoundedInput(panelHoten, txtHoten, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelEmail, txtEmail, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(paneSdt, txtSdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelDiachi, txtDiachi, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMatkhaucu, txtMatkhaucu, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMatkhaumoi, txtMatkhaumoi, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelXacnhan, txtXacnhan, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNgaysinh, dateTimePicker1, 12, 2, Color.FromArgb(0, 152, 70));
            taiThongTinCaNhan();
        }


        private void taiThongTinCaNhan()
        {
            try
            {
                string email = SessionStore.Current.UserName;
                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Không tìm thấy thông tin đăng nhập!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                NhanVienBLL bll = new NhanVienBLL();
                NhanVien nv = bll.layThongTinCaNhan(email);

                if (nv == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin cá nhân!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 🔸 Hiển thị thông tin lên form
                txtHoten.Text = nv.hoTen;
                txtEmail.Text = nv.email;
                txtSdt.Text = nv.soDienThoai;
                txtDiachi.Text = nv.diaChi;

                // Ngày sinh
                if (nv.ngaySinh != DateTime.MinValue)
                    dateTimePicker1.Value = nv.ngaySinh;

                // Giới tính
                if (!string.IsNullOrEmpty(nv.gioiTinh))
                {
                    if (nv.gioiTinh == "0")
                        radioBtnnam.Checked = true;
                    else if (nv.gioiTinh == "1")
                        radioBtnnu.Checked = true;
                }

                maNVHienTai = nv.maNV;
                anhDaiDienHienTai = nv.anhDaiDien;
                taiAnhDaiDien(nv.anhDaiDien);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin cá nhân: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void taiAnhDaiDien(string anhDaiDien)
        {
            // Giải phóng ảnh cũ trước
            if (ptbAnhcanhan.Image != null)
            {
                ptbAnhcanhan.Image.Dispose();
                ptbAnhcanhan.Image = null;
            }

            if (!string.IsNullOrEmpty(anhDaiDien))
            {
                // ✅ ĐÃ SỬA: Đổi từ "Images/NhanVien" → "Avatars"
                string imgPath = Path.Combine(Application.StartupPath, "Avatars", anhDaiDien);

                if (File.Exists(imgPath))
                {
                    // ✅ Load ảnh KHÔNG BỊ LOCK FILE
                    using (var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                    {
                        ptbAnhcanhan.Image = Image.FromStream(fs);
                    }
                }
                else
                {
                    // Nếu không tìm thấy ảnh, dùng ảnh mặc định
                    ptbAnhcanhan.Image = Properties.Resources.macdinh;
                }
            }
            else
            {
                // Nếu chưa có ảnh đại diện, dùng ảnh mặc định
                ptbAnhcanhan.Image = Properties.Resources.macdinh;
            }
        }


        private void btnQuenmk_Click(object sender, EventArgs e)
        {
            QuenMatKhau1 form = new QuenMatKhau1();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void chinhSuaAnh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maNVHienTai))
            {
                MessageBox.Show("Khong xac dinh duoc nhan vien!", "Loi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện ";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string avatarsFolder = Path.Combine(Application.StartupPath, "Avatars");
                        if (!Directory.Exists(avatarsFolder))
                            Directory.CreateDirectory(avatarsFolder);

                        string ext = Path.GetExtension(ofd.FileName);
                        string fileName = maNVHienTai + ext; // VD: NV001.jpg
                        string destPath = Path.Combine(avatarsFolder, fileName);

                        //// Copy (overwrite nếu đã có)
                        //File.Copy(ofd.FileName, destPath, true);
                        //anhDaiDienHienTai = fileName;

                        // Giải phóng ảnh cũ trước khi load mới để tránh lỗi locked file
                        if (ptbAnhcanhan.Image != null)
                        {
                            ptbAnhcanhan.Image.Dispose();
                            ptbAnhcanhan.Image = null;
                        }

                        File.Copy(ofd.FileName, destPath, true);
                        anhDaiDienHienTai = fileName;
                        using (FileStream fs = new FileStream(destPath, FileMode.Open, FileAccess.Read))
                        {
                            ptbAnhcanhan.Image = Image.FromStream(fs);
                        }
                        //ptbAnhcanhan.Image = Image.FromFile(destPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật ảnh đại diện : " + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var bll = new NhanVienBLL();
            //    var nv = new NhanVien
            //    {
            //        maNV = maNVHienTai,
            //        hoTen = txtHoten.Text.Trim(),
            //        email = txtEmail.Text.Trim(),
            //        soDienThoai = txtSdt.Text.Trim(),
            //        diaChi = txtDiachi.Text.Trim(),
            //        ngaySinh = dateTimePicker1.Value,
            //        gioiTinh = radioBtnnam.Checked ? "0" : "1",
            //        anhDaiDien = anhDaiDienHienTai
            //    };

            //    bll.capNhatThongTinCaNhan(nv);
            //    MessageBox.Show("Cập nhật thông tin cá nhân thành công !",
            //                    "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    anhDaiDienDaThayDoi?.Invoke();
            //    taiThongTinCaNhan();


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Loi: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            try
            {
                //ĐỔI MẬT KHẨU
                string mkCu = txtMatkhaucu.Text.Trim();
                string mkMoi = txtMatkhaumoi.Text.Trim();
                string mkXacNhan = txtXacnhan.Text.Trim();

                // Người dùng có ý định đổi mật khẩu (ít nhất 1 ô được nhập)
                bool userWantsChangePassword =
                    !string.IsNullOrWhiteSpace(mkCu) ||
                    !string.IsNullOrWhiteSpace(mkMoi) ||
                    !string.IsNullOrWhiteSpace(mkXacNhan);

                if (userWantsChangePassword)
                {
                    // Kiểm tra đủ 3 trường
                    if (string.IsNullOrWhiteSpace(mkCu) ||
                        string.IsNullOrWhiteSpace(mkMoi) ||
                        string.IsNullOrWhiteSpace(mkXacNhan))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ 3 trường mật khẩu.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tenTK = SessionStore.Current.UserName;
                    TaiKhoanBLL tkBll = new TaiKhoanBLL();

                    var kqDoiMK = tkBll.doiMatKhau(tenTK, mkCu, mkMoi, mkXacNhan);

                    if (!kqDoiMK.success)
                    {
                        MessageBox.Show(kqDoiMK.message,
                            "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // ❌ Dừng lại, KHÔNG lưu thông tin cá nhân nếu đổi mật khẩu thất bại
                    }
                    else
                    {
                        MessageBox.Show(kqDoiMK.message,
                            "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Xóa trắng các ô
                        txtMatkhaucu.Clear();
                        txtMatkhaumoi.Clear();
                        txtXacnhan.Clear();
                    }
                }

                // CẬP NHẬT THÔNG TIN CÁ NHÂN
                var bll = new NhanVienBLL();
                var nv = new NhanVien
                {
                    maNV = maNVHienTai,
                    hoTen = txtHoten.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    soDienThoai = txtSdt.Text.Trim(),
                    diaChi = txtDiachi.Text.Trim(),
                    ngaySinh = dateTimePicker1.Value,
                    gioiTinh = radioBtnnam.Checked ? "0" : "1",
                    anhDaiDien = anhDaiDienHienTai
                };

                bll.capNhatThongTinCaNhan(nv);

                MessageBox.Show("Cập nhật thông tin cá nhân thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gọi sự kiện để refresh lại trang chủ hoặc form
                anhDaiDienDaThayDoi?.Invoke();
                taiThongTinCaNhan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtHoten_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtDiachi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMatkhaucu_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioBtnnam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioBtnnu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ptbAnhcanhan_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMatkhaumoi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtXacnhan_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnXacthuckhuonmat_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy tên tài khoản từ session
                string tenTaiKhoan = SessionStore.Current.UserName;

                if (string.IsNullOrEmpty(tenTaiKhoan))
                {
                    MessageBox.Show("Không tìm thấy thông tin đăng nhập!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Mở form đăng ký khuôn mặt
                using (DangKyKhuonMat frmDangKy = new DangKyKhuonMat(tenTaiKhoan))
                {
                    frmDangKy.StartPosition = FormStartPosition.CenterParent;

                    if (frmDangKy.ShowDialog() == DialogResult.OK)
                    {
                        // Nếu đăng ký thành công
                        MessageBox.Show("Đăng ký khuôn mặt thành công!\n\nBạn có thể sử dụng Face ID để đăng nhập.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form đăng ký khuôn mặt: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
