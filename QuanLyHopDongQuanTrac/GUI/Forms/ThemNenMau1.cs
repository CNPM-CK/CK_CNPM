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
    public partial class ThemNenMau1 : Form
    {
        // Property để nhận nền mẫu cần sửa
        public NenMau NenMauHienTai { get; set; }

        // Biến lưu mã nền đang chọn
        private string maNenDangChon = "";

        public bool isEditMode = false;

        // Event thông báo cập nhật thành công
        public event EventHandler SuccessfullyUpdated;

        public ThemNenMau1()
        {
            InitializeComponent();
        }

        public ThemNenMau1(NenMau nm)
        {
            InitializeComponent();
            isEditMode = true;
            NenMauHienTai = nm;
        }

        #region Custom TextBox và Label cho Form Nhân viên

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

            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));

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

        private void InitializeButtonStyles()
        {
            BoGocButton(btnThemm, 25);
        }
        #endregion

        private void ThemNenMau1_Load(object sender, EventArgs e)
        {
            ApplyRoundedInput(panelMota, txtMota, 20, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNenmau, txtTennenmau, 20, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();

            if (isEditMode && NenMauHienTai != null)
            {
                // Chế độ sửa: hiển thị dữ liệu hiện tại
                this.Text = "Chỉnh sửa nền mẫu";
                btnThemm.Text = "Lưu";
                label.Text = "Chỉnh sửa nền mẫu";

                // Load dữ liệu lên form
                maNenDangChon = NenMauHienTai.maNen;
                txtTennenmau.Text = NenMauHienTai.tenNenMau;
                txtMota.Text = NenMauHienTai.moTa;

                // Disable textbox tên nền mẫu khi sửa (nếu không cho phép sửa tên)
                txtTennenmau.Enabled = false;
                txtTennenmau.BackColor = Color.LightGray;
            }
            else
            {
                // Chế độ thêm mới
                this.Text = "Thêm nền mẫu";
                btnThemm.Text = "Thêm";
                label.Text = "Thêm nền mẫu";
                txtTennenmau.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTennenmau.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nền mẫu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTennenmau.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMota.Text))
                {
                    MessageBox.Show("Vui lòng nhập mô tả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMota.Focus();
                    return;
                }

                var bll = new NenMauBLL();

                if (isEditMode)
                {
                    // Chế độ sửa
                    bool result = bll.SuaNenMau(maNenDangChon, txtMota.Text.Trim());

                    if (result)
                    {
                        MessageBox.Show("Cập nhật mô tả nền mẫu thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Kích hoạt event thông báo cập nhật thành công
                        SuccessfullyUpdated?.Invoke(this, EventArgs.Empty);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Chế độ thêm mới
                    string maNenMoi = bll.ThemNenMau(txtTennenmau.Text.Trim(), txtMota.Text.Trim());

                    if (!string.IsNullOrEmpty(maNenMoi))
                    {
                        MessageBox.Show("Thêm nền mẫu thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Tag = maNenMoi;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại! Kiểm tra lại dữ liệu.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm/sửa nền mẫu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Có thể dùng cho nút hủy/đóng form
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}