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
    public partial class ThemKhachHang : Form
    {
        public event EventHandler SuccesfullyUpdated;

        public ThemKhachHang()
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
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;

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

            ctrl.Location = new Point(borderSize + 5, (panel.Height - ctrl.Height) / 2);
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

            cboTinhThanh.DataSource = tinhList;
            cboTinhThanh.DisplayMember = "name";
            cboTinhThanh.ValueMember = "code";
            cboTinhThanh.SelectedIndex = -1;
            cboTinhThanh.Text = "Tỉnh/Thành phố";

            ApplyRoundedInput(panelTenDN, textBoxTenDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelsdt, textBoxsdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelKHDN, textBoxKHDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTenDD, textBoxTenDD, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTinh, cboTinhThanh, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelHuyen, cbbQuan, 12, 2, Color.FromArgb(0, 152, 70));
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


        private void panel5_Paint(object sender, PaintEventArgs e){}

        private void label4_Click(object sender, EventArgs e){}

        private void textBoxhoten_TextChanged(object sender, EventArgs e){}

        private void radioButton2_CheckedChanged(object sender, EventArgs e){}

        private void label10_Click(object sender, EventArgs e){}

        private void panel3_Paint(object sender, PaintEventArgs e){}

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e){}

        private void label1_Click(object sender, EventArgs e){}

        private void textBoxemail_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear TextBox
            textBoxTenDD.Clear();
            textBoxsdt.Clear();
            textBoxKHDN.Clear();
            txtDiaChi.Clear();
            textBoxTenDN.Clear();

            // Reset ComboBox
            cboTinhThanh.SelectedIndex = -1;
            cboTinhThanh.Text = "Tỉnh/Thành phố";

            cbbQuan.DataSource = null;
            cbbQuan.SelectedIndex = -1;
            cbbQuan.Text = "Quận/Huyện";

            cbbXa.DataSource = null;
            cbbXa.SelectedIndex = -1;
            cbbXa.Text = "Xã/Phường";
        }



        private void buttonAddnew_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBoxTenDN.Text))
            {
                MessageBox.Show("Vui lòng nhập tên doanh nghiệp !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTenDN.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxsdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxsdt.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxTenDD.Text))
            {
                MessageBox.Show("Vui lòng tên đại diện !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTenDD.Focus();
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

            KhachHang kh = new KhachHang
            {
                tenDoanhNghiep = textBoxTenDN.Text.Trim(),
                kyHieuDN = textBoxKHDN.Text.Trim(),
                nguoiDaiDien = textBoxTenDD.Text.Trim(),
                diaChi = diaChi,
                soDienThoaiKH = textBoxsdt.Text.Trim()
            };

            try
            {
                var bll = new KhachHangBLL();
                bll.ThemKhachHang(kh);

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SuccesfullyUpdated?.Invoke(this, EventArgs.Empty);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Lỗi SQL:\n{ex.Message}\n\nNumber: {ex.Number}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
