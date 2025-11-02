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
        public KhachHang KhachHangHienTai { get; set; }
        public bool isEditMode = false;


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
            ApplyRoundedInput(panelTenDN, textBoxTenDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelsdt, textBoxsdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelKHDN, textBoxKHDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTenDD, textBoxTenDD, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMaildn, txtEmaildn, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMaildd, txtMaildd, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMsthue, txtMsthue, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTrangthai, cboTrangthai, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelDiachi, txtDiachi, 12, 2, Color.FromArgb(0, 152, 70));

            InitializeButtonStyles();
            LoadComboBoxTrangThai();
            if (isEditMode)
            {
                this.Text = "Chỉnh Sửa Khách Hàng";
                buttonAddnew.Text = "Lưu Thay Đổi";
                label.Text = " CHỈNH SỬA THÔNG TIN ";
            }
            if (isEditMode && KhachHangHienTai != null)
            {
                // Load dữ liệu vào các control
                textBoxTenDN.Text = KhachHangHienTai.tenDoanhNghiep;
                textBoxKHDN.Text = KhachHangHienTai.kyHieuDN;
                textBoxTenDD.Text = KhachHangHienTai.nguoiDaiDien;
                textBoxsdt.Text = KhachHangHienTai.soDienThoaiKH;
                txtMsthue.Text = KhachHangHienTai.maSoThue;
                txtMaildd.Text = KhachHangHienTai.emailNguoiDaiDien;
                txtEmaildn.Text = KhachHangHienTai.emailDoanhNghiep;
                txtDiachi.Text = KhachHangHienTai.diaChi;
                cboTrangthai.SelectedValue = KhachHangHienTai.trangThai;
            }
            else
            {
                this.Text = "Thêm Khách Hàng Mới";
                buttonAddnew.Text = "Thêm khách hàng";
                label.Text = " THÊM KHÁCH HÀNG ";

            }
        }

        private void LoadComboBoxTrangThai()
        {
            KhachHangBLL bll = new KhachHangBLL();
            var listTrangThai = bll.LayTrangThaiKhachHang();

            cboTrangthai.DataSource = listTrangThai;
            cboTrangthai.DisplayMember = "tenTrangThai";
            cboTrangthai.ValueMember = "maTrangThai";
        }

        private void buttonAddnew_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTenDN.Text))
            {
                MessageBox.Show("Vui lòng nhập tên doanh nghiệp !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTenDN.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxTenDD.Text))
            {
                MessageBox.Show("Vui lòng nhập kí hiệu doanh nghiệp !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxKHDN.Focus();
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
                MessageBox.Show("Vui lòng nhập tên người đại diện  tên đại diện !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTenDD.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiachi.Text))
            {
                MessageBox.Show("Vui lòng nhập email doanh nghiệp  !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmaildn.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxTenDD.Text))
            {
                MessageBox.Show("Vui lòng nhập mã số thuế !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMsthue.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxTenDD.Text))
            {
                MessageBox.Show("Vui lòng nhập email người đại diện !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaildd.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiachi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ !", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }

            KhachHang kh = new KhachHang
            {
                tenDoanhNghiep = textBoxTenDN.Text.Trim(),
                kyHieuDN = textBoxKHDN.Text.Trim(),
                nguoiDaiDien = textBoxTenDD.Text.Trim(),
                soDienThoaiKH = textBoxsdt.Text.Trim(),
                maSoThue = txtMsthue.Text.Trim(),
                emailNguoiDaiDien = txtMaildd.Text.Trim(),
                emailDoanhNghiep = txtEmaildn.Text.Trim(),
                diaChi = txtDiachi.Text.Trim(),
                trangThai = Convert.ToInt32(cboTrangthai.SelectedValue)

            };

            try
            {
                var bll = new KhachHangBLL();

                if (isEditMode)
                {
                    kh.maKH = KhachHangHienTai.maKH; // Giữ lại mã KH
                    bll.SuaKhachHang(kh); // Method này cần có trong KhachHangBLL
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo");
                    SuccesfullyUpdated?.Invoke(this, EventArgs.Empty); // ✅ Quan trọng
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    bll.ThemKhachHang(kh);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo");
                }
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            textBoxTenDD.Clear();
            textBoxsdt.Clear();
            textBoxKHDN.Clear();
            textBoxTenDN.Clear();
            txtDiachi.Clear();
            txtEmaildn.Clear();
            txtMaildd.Clear();
            txtMsthue.Clear();
        }

        private void panel5_Paint(object sender, PaintEventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void textBoxhoten_TextChanged(object sender, EventArgs e) { }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }

        private void label10_Click(object sender, EventArgs e) { }

        private void panel3_Paint(object sender, PaintEventArgs e) { }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void textBoxemail_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }
    }
}
