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
    public partial class SuaKhachHang : Form
    {
        public event EventHandler SuccesfullyUpdated;
        private KhachHang khachHangHienTai;
        private KhachHang khachHangBanDau;



        public SuaKhachHang(KhachHang kh)
        {
            InitializeComponent();
            khachHangHienTai = kh;

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
            var tinhList = diaChiService.LayTinhThanh();

            cboTinhThanh.DataSource = tinhList;
            cboTinhThanh.DisplayMember = "name";
            cboTinhThanh.ValueMember = "code";
            cboTinhThanh.SelectedIndex = -1;


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
                }
            };

            if (khachHangHienTai != null)
            {
                khachHangBanDau = new KhachHang
                {
                    maKH = khachHangHienTai.maKH,
                    kyHieuDN = khachHangHienTai.kyHieuDN,
                    tenDoanhNghiep = khachHangHienTai.tenDoanhNghiep,
                    nguoiDaiDien = khachHangHienTai.nguoiDaiDien,
                    soDienThoaiKH = khachHangHienTai.soDienThoaiKH,
                    diaChi = khachHangHienTai.diaChi,
                };

                textBoxKHDN.Text = khachHangHienTai.kyHieuDN;
                textBoxTenDD.Text = khachHangHienTai.nguoiDaiDien;
                textBoxTenDN.Text = khachHangHienTai.tenDoanhNghiep;
                textBoxsdt.Text = khachHangHienTai.soDienThoaiKH;

                //Địa chỉ 
                if (!string.IsNullOrEmpty(khachHangHienTai.diaChi))
                {
                    string[] parts = khachHangHienTai.diaChi.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                           .Select(p => p.Trim())
                                                           .ToArray();

                    int len = parts.Length;

                    if (len >= 1) cboTinhThanh.Text = parts[len - 1]; // Tỉnh
                    if (len >= 2) cbbQuan.Text = parts[len - 2];      // Quận/Huyện
                    if (len >= 3) cbbXa.Text = parts[len - 3];        // Xã/Phường
                    if (len >= 4) txtDiaChi.Text = parts[len - 4];    // Số nhà (nếu có)

                }
            }

            ApplyRoundedInput(panelTenDN, textBoxTenDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelsdt, textBoxsdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelKHDN, textBoxKHDN, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTenDD, textBoxTenDD, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTinh, cboTinhThanh, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelHuyen, cbbQuan, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelXa, cbbXa, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelSonha, txtDiaChi, 12, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();

        }

        public void HienThiThongTinKH(KhachHang kh) 
        {
            textBoxTenDN.Text = kh.tenDoanhNghiep;
            textBoxKHDN.Text = kh.kyHieuDN;
            textBoxTenDD.Text = kh.nguoiDaiDien;
            textBoxsdt.Text = kh.soDienThoaiKH;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(khachHangBanDau != null) 
            {
                HienThiThongTinKH(khachHangBanDau);
            }
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

            List<string> diaChiParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(txtDiaChi.Text))
                diaChiParts.Add(txtDiaChi.Text.Trim());
            if (cbbXa.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(cbbXa.Text))
                diaChiParts.Add(cbbXa.Text.Trim());
            if (cbbQuan.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(cbbQuan.Text))
                diaChiParts.Add(cbbQuan.Text.Trim());
            if (cboTinhThanh.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(cboTinhThanh.Text))
                diaChiParts.Add(cboTinhThanh.Text.Trim());

            string diaChi = string.Join(", ", diaChiParts);


            KhachHang kh = new KhachHang
            {
                maKH = khachHangHienTai.maKH,
                tenDoanhNghiep = textBoxTenDN.Text.Trim(),
                kyHieuDN = textBoxKHDN.Text.Trim(),
                nguoiDaiDien = textBoxTenDD.Text.Trim(),
                diaChi = diaChi,
                soDienThoaiKH = textBoxsdt.Text.Trim()
            };


            try
            {
                var bll = new KhachHangBLL();
                var (daThayDoi, logThayDoi) = bll.KiemTraThayDoi(kh, khachHangBanDau);
                if (!daThayDoi)
                {
                    MessageBox.Show("Không có thông tin nào thay đổi!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bll.SuaKhachHang(kh);

                MessageBox.Show("Sửa thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            SuccesfullyUpdated?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
