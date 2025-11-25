using BLL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class ThemHopDongForm : Form
    {
        public event EventHandler SuccesfullyUpdated;

        public ThemHopDongForm()
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


        private void ThemHopDong_Load(object sender, EventArgs e)
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

            var bllTanSuatQT = new TanSuatQTBLL();
            var list_tsqt = bllTanSuatQT.LayDanhSachTSQT();
            cbbTanSuatQT.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbTanSuatQT.DisplayMember = "tenTSQT";
            cbbTanSuatQT.ValueMember = "maTSQT";

            if (list != null && list.Count > 0)
            {
                var data = new List<TanSuatQTDTO>();
                data.Add(new TanSuatQTDTO { maTSQT = null, tenTSQT = "— Chọn tần suất quan trắc —" });
                data.AddRange(list_tsqt);

                cbbTanSuatQT.DataSource = data;
                cbbTanSuatQT.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không có khách hàng nào trong DB!");
            }
            ;
            var bllTrangThai = new TrangThaiBLL();
            var list_tt = bllTrangThai.layDanhSachTrangThaiHD();
            cbbTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbTrangThai.DisplayMember = "tenTT";
            cbbTrangThai.ValueMember = "maTT";

            if (list != null && list.Count > 0)
            {
                var data = new List<TrangThaiHDDTO>();
                data.Add(new TrangThaiHDDTO { maTT = null, tenTT = "— Chọn trạng thái quan trắc —" });
                data.AddRange(list_tt);

                cbbTrangThai.DataSource = data;
                cbbTrangThai.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không có trạng thái nào trong DB!");
            }
            InitializeButtonStyles();
            ApplyRoundedInput(panelKhachhang, cbbKhachHang, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTansuat, cbbTanSuatQT, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTrangthai, cbbTrangThai, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelHopdong, textBox1, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNgayki, dateTimePicker1, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNgayketthuc, dateTimePicker2, 15, 2, Color.FromArgb(0, 152, 70));
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
            if (cbbTanSuatQT.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn tần suất quan trắc!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbKhachHang.Focus();
                return;
            }
            if (cbbTrangThai.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn trạng thái quan trắc!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbKhachHang.Focus();
                return;
            }
            HopDongDTO hd = new HopDongDTO
            {
                maKH = cbbKhachHang.SelectedValue.ToString(),
                soHD = textBox1.Text.ToString(),
                tanSuatQuanTrac = cbbTanSuatQT.SelectedValue.ToString(),
                ngayKy = dateTimePicker1.Value,
                ngayKetThucHD = dateTimePicker2.Value,
                trangThai = cbbTrangThai.SelectedValue.ToString(),
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
            cbbTanSuatQT.SelectedIndex = 0;
            cbbTrangThai.SelectedIndex = 0;
            textBox1.Clear();

            // Reset DateTimePicker
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
