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


        private void ApplyRoundedTextbox(Panel panel, TextBox txt, int borderRadius, int borderSize, Color borderColor)
        {
            // Hủy đăng ký các event cũ trước (nếu có)
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;

            panel.BackColor = Color.White;
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.White;
            txt.Location = new Point(borderSize + 5, (panel.Height - txt.Height) / 2);
            txt.Width = panel.Width - (borderSize + 5) * 2;
            txt.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            // Tạo event handler riêng để có thể hủy đăng ký
            void Panel_Paint(object s, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                {
                    // Vẽ nền
                    using (SolidBrush brush = new SolidBrush(panel.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    // Vẽ viền - điều chỉnh path để viền không bị cắt
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

                    panel.Region = new Region(path);
                }
            }

            void Panel_Resize(object s, EventArgs e)
            {
                panel.Invalidate();
            }

            // Đăng ký event mới
            panel.Paint += Panel_Paint;
            panel.Resize += Panel_Resize;

            // Vẽ lại ngay lập tức
            panel.Invalidate();
        }

        //Hàm helper để tạo rounded rectangle path
        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

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

            //Custom textbox
            ApplyRoundedTextbox(panelHoten, textBoxhoten, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedTextbox(panelsdt, textBoxsdt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedTextbox(panelemail, textBoxemail, 12, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();

            // Load tỉnh thành
            cboTinhThanh.DataSource = tinhList;
            cboTinhThanh.DisplayMember = "name";
            cboTinhThanh.ValueMember = "code";
            cboTinhThanh.SelectedIndex = -1;
            cboTinhThanh.Text = "Tỉnh/Thành phố"; // Set placeholder


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
                    cbbQuan.Text = "Quận/Huyện"; // Set placeholder

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
                    cbbXa.Text = "Xã/Phường"; // Set placeholder
                }
            };

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
         

            if (cbbPhong.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn phòng ban!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbPhong.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxhoten.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxhoten.Focus();
                return;
            }

            if (!radioNam.Checked && !radioNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxsdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxsdt.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxemail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxemail.Focus();
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

            // Tạo đối tượng nhân viên
            NhanVien nv = new NhanVien
            {
                //tenTK = textBoxtentk.Text,
                maPhong = cbbPhong.SelectedValue.ToString(), // combobox phòng ban
                hoTen = textBoxhoten.Text,
                ngaySinh = dateTimePicker1.Value,
                gioiTinh = radioNam.Checked ? "0" : "1",  // 0 = Nam, 1 = Nữ
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
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dateTimePicker1.Value = DateTime.Now;

        }
    }
}
