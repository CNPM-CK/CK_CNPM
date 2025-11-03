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
    public partial class ThemHopDong : Form
    {
        public event EventHandler SuccesfullyUpdated;

        public ThemHopDong()
        {
            InitializeComponent();
        }


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
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
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
            var bllKhachHang = new KhachHangBLL();
            var list = bllKhachHang.layDanhSachKH();
            cbbKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbKhachHang.DisplayMember = "tenDoanhNghiep";
            cbbKhachHang.ValueMember = "maKH";

            if (list != null && list.Count > 0)
            {
                var data = new List<KhachHang>();
                data.Add(new KhachHang { maKH = null, tenDoanhNghiep = "— Chọn khách hàng —" });
                data.AddRange(list);

                cbbKhachHang.DataSource = data;
                cbbKhachHang.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không có khách hàng nào trong DB!");
            }
            //Custom textbox
            //ApplyRoundedInput(panelKhachHang, cbbKhachHang, 12, 2, Color.FromArgb(0, 152, 70));
            //ApplyRoundedInput(panelKhachHang, cbbKhachHang, 12, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();


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

            if (cbbKhachHang.SelectedIndex <= 0)
            {
                MessageBox.Show("Khách hàng là bắt buộc!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbKhachHang.Focus();
                return;
            }


            HopDong hd = new HopDong
            {
                maKH = cbbKhachHang.SelectedValue.ToString(),
                ngayKy = dateTimePicker1.Value,
                ngayKetThucHD = dateTimePicker2.Value
            };

            try
            {
                var bll = new HopDongBLL();
                bll.ThemHopDong(hd);

                MessageBox.Show("Thêm hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

            cbbKhachHang.SelectedIndex = 0;

            // Reset DateTimePicker
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

        }

        private void textBoxemail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
