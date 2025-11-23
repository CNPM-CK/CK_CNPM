using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class SuaThongSo : Form
    {
        public ThongSo ThongSoHienTai { get; set; }
        public ThongSo ThongSoDaChinhSua { get; private set; }
        public SuaThongSo()
        {
            InitializeComponent();
            this.Load += SuaThongSo_Load;
        }

        public SuaThongSo(ThongSo thongSo) : this()
        {
            ThongSoHienTai = thongSo;
        }
        #region Custom TextBox và Label cho Form Nhân viên
        private void InitializeButtonStyles()
        {

            BoGocButton(btnThem, 25);
            BoGocButton(btnHuy, 25);

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

        private void SuaThongSo_Load(object sender, EventArgs e)
        {
            ApplyRoundedInput(panelDonvi, txtDonvi, 20, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMax, txtMax, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMin, txtMin, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTents, txtTents, 20, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelPhuongphap, txtPhuongphap, 20, 2, Color.FromArgb(0, 152, 70));

            InitializeButtonStyles();
            LoadThongSoData();
        }


        private void LoadThongSoData()
        {
            if (ThongSoHienTai == null) return;

            try
            {
                txtTents.Text = ThongSoHienTai.TenTS;
                txtTents.ReadOnly = true;
                txtTents.BackColor = Color.FromArgb(240, 240, 240); // Màu xám nhạt
                txtTents.ForeColor = Color.Gray;


                txtDonvi.Text = ThongSoHienTai.DonVi ?? "";
                txtPhuongphap.Text = ThongSoHienTai.phuongPhap ?? "";
                txtMin.Text = ThongSoHienTai.GiaTriToiThieu?.ToString("0.##") ?? "";
                txtMax.Text = ThongSoHienTai.GiaTriToiDa?.ToString("0.##") ?? "";
                btnThem.Text = "Cập nhật";
                this.Text = $"Chỉnh sửa thông số - {ThongSoHienTai.TenTS}";

                // Focus vào trường đầu tiên có thể sửa
                txtDonvi.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDonvi.Text))
                {
                    MessageBox.Show("Vui lòng nhập đơn vị!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonvi.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPhuongphap.Text))
                {
                    MessageBox.Show("Vui lòng nhập phương pháp !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhuongphap.Focus();
                    return;
                }

                double? giaTriMin = null;
                double? giaTriMax = null;

                if (!string.IsNullOrWhiteSpace(txtMin.Text))
                {
                    if (!double.TryParse(txtMin.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double min))
                    {
                        MessageBox.Show("Giá trị tối thiểu không hợp lệ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMin.Focus();
                        return;
                    }
                    giaTriMin = min;
                }

                if (!string.IsNullOrWhiteSpace(txtMax.Text))
                {
                    if (!double.TryParse(txtMax.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double max))
                    {
                        MessageBox.Show("Giá trị tối đa không hợp lệ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMax.Focus();
                        return;
                    }
                    giaTriMax = max;
                }

                if (giaTriMin.HasValue && giaTriMax.HasValue && giaTriMin.Value > giaTriMax.Value)
                {
                    MessageBox.Show("Giá trị tối thiểu không được lớn hơn giá trị tối đa!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (giaTriMin.HasValue && giaTriMax.HasValue && giaTriMin.Value == giaTriMax.Value)
                {
                    MessageBox.Show("Giá trị tối thiểu không được bằng với giá trị tối đa!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ThongSoDaChinhSua = new ThongSo
                {
                    MaTS = ThongSoHienTai.MaTS,           
                    TenTS = ThongSoHienTai.TenTS,         
                    DonVi = txtDonvi.Text.Trim(),         
                    phuongPhap = txtPhuongphap.Text.Trim(), 
                    GiaTriToiThieu = giaTriMin,           
                    GiaTriToiDa = giaTriMax             
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông số:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
