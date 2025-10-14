using BLL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class ThemNhanVien : Form
    {
        public event EventHandler SuccesfullyUpdated;

        public ThemNhanVien()
        {
            InitializeComponent();
        }
        private DiaChiBLL diaChiService;


        #region Custom TextBox và Label cho Form Nhân viên
        private void InitializeButtonStyles()
        {

            BoGocButton(buttonAddnew, 18);
            BoGocButton(btnCancel, 18);

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
        #endregion


        private void ThemNhanVien_Load(object sender, EventArgs e)
        {
            diaChiService = new DiaChiBLL();
            var bllPhongBan = new PhongBanBLL();
            var tinhList = diaChiService.LayTinhThanh();
            var list = bllPhongBan.LayDSPhongBan();

            // Load tỉnh thành
            cboTinhThanh.DataSource = tinhList;
            cboTinhThanh.DisplayMember = "name";
            cboTinhThanh.ValueMember = "code";
            cboTinhThanh.SelectedIndex = -1;
            cboTinhThanh.Text = "Tỉnh/Thành phố";

            if (list != null && list.Count > 0)
            {
                cbbPhong.DataSource = list;
                cbbPhong.DisplayMember = "tenPhong";
                cbbPhong.ValueMember = "maPhong";
                cbbPhong.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Không có phòng ban nào trong DB!");
            }
            //Custom textbox
            ApplyRoundedInput(panelHoten, textBoxhoten, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelsdt, textBoxsdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelemail, textBoxemail, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNgaysinh, dateTimengaysinh, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelPhongban, cbbPhong, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTinh, cboTinhThanh, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelHuyen, cbbQuan, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelPhongban, cbbPhong, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelXa, cbbXa, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelSonha, txtDiaChi, 12, 2, Color.FromArgb(0, 152, 70));

            cboTinhThanh.Text = "Tỉnh/Thành phố";
            cbbQuan.Text = "Quận/Huyện";
            cbbXa.Text = "Xã/Phường";

            InitializeButtonStyles();

            cboTinhThanh.SelectedIndexChanged += (s, ev) =>
            {
                if (cboTinhThanh.SelectedValue != null)
                {
                    string maTinh = cboTinhThanh.SelectedValue.ToString();
                    var quanList = diaChiService.LayQuanHuyen(maTinh);
                    cbbQuan.DataSource = quanList;
                    cbbQuan.DisplayMember = "name_with_type";
                    cbbQuan.ValueMember = "code";
                    cbbQuan.SelectedIndex = -1;
                    cbbQuan.Text = "Quận/Huyện";

                }
            };

            cbbQuan.SelectedIndexChanged += (s, ev) =>
            {
                if (cbbQuan.SelectedValue != null)
                {
                    string maQuan = cbbQuan.SelectedValue.ToString();
                    var xaList = diaChiService.LayXaPhuong(maQuan);
                    cbbXa.DataSource = xaList;
                    cbbXa.DisplayMember = "name_with_type";
                    cbbXa.ValueMember = "code";
                    cbbXa.SelectedIndex = -1;
                    cbbXa.Text = "Xã/Phường";
                }
            };


        }


        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBoxhoten_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonAddnew_Click(object sender, EventArgs e)
        {
            //Kiểm tra trường hợp 

            if (string.IsNullOrWhiteSpace(textBoxhoten.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxemail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxemail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxemail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxsdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxsdt.Focus();
                return;
            }

            if (cbbPhong.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn phòng ban!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbPhong.Focus();
                return;
            }

            if (!radioNam.Checked && !radioNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dateTimengaysinh.Checked)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTinhThanh.SelectedIndex < 0 || cboTinhThanh.Text == "Tỉnh/Thành phố")
            {
                MessageBox.Show("Vui lòng chọn Tỉnh/Thành phố!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTinhThanh.Focus();
                return;
            }

            if (cbbQuan.SelectedIndex < 0 || cbbQuan.Text == "Quận/Huyện")
            {
                MessageBox.Show("Vui lòng chọn Quận/Huyện!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbQuan.Focus();
                return;
            }

            if (cbbXa.SelectedIndex < 0 || cbbXa.Text == "Xã/Phường")
            {
                MessageBox.Show("Vui lòng chọn Xã/Phường!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbXa.Focus();
                return;
            }


            string diaChi = "";
            if (cbbXa.SelectedIndex >= 0)
                diaChi += cbbXa.Text + ", ";
            if (cbbQuan.SelectedIndex >= 0)
                diaChi += cbbQuan.Text + ", ";
            if (cboTinhThanh.SelectedIndex >= 0)
                diaChi += cboTinhThanh.Text;
            if (!string.IsNullOrWhiteSpace(txtDiaChi.Text))
                diaChi = txtDiaChi.Text + ", " + diaChi;

            NhanVien nv = new NhanVien
            {
                maPhong = cbbPhong.SelectedValue.ToString(),
                hoTen = textBoxhoten.Text,
                ngaySinh = dateTimengaysinh.Value,
                gioiTinh = radioNam.Checked ? "0" : "1",
                diaChi = diaChi,
                soDienThoai = textBoxsdt.Text,
                email = textBoxemail.Text
            };

            // Kiểm tra radio trưởng phòng
            bool isTruongPhong = checkTruongphong.Checked;

            try
            {
                var bll = new NhanVienBLL();
                bll.ThemNhanVien(nv, isTruongPhong);

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SuccesfullyUpdated?.Invoke(this, EventArgs.Empty); // chỉ khi thành công
                this.DialogResult = DialogResult.OK;
                this.Close(); // chỉ khi thành công
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Không đóng form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Không đóng form
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear TextBox
            textBoxhoten.Clear();
            textBoxsdt.Clear();
            textBoxemail.Clear();
            txtDiaChi.Clear();

            // Reset ComboBox
            cboTinhThanh.SelectedIndex = -1;
            cboTinhThanh.Text = "Tỉnh/Thành phố";

            cbbQuan.DataSource = null;
            cbbQuan.SelectedIndex = -1;
            cbbQuan.Text = "Quận/Huyện";

            cbbXa.DataSource = null;
            cbbXa.SelectedIndex = -1;
            cbbXa.Text = "Xã/Phường";

            cbbPhong.SelectedIndex = -1;
            cbbPhong.Text = "Phòng ban";

            // Reset RadioButton
            radioNam.Checked = false;
            radioNu.Checked = false;
            checkTruongphong.Checked = false;

            // Reset DateTimePicker
            dateTimengaysinh.Value = DateTime.Now;

        }

        private void textBoxemail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
