using BLL;
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
    public partial class LocThongBao : Form
    {
        public string SelectedLoaiTB { get; private set; }

        public LocThongBao()
        {
            InitializeComponent();
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

       

        private void LocTrangThaiKhachHang_Load(object sender, EventArgs e)
        {

            // Trạng thái
            KhachHangBLL khBLL = new KhachHangBLL();
            cboTrangthai.DataSource = khBLL.layTrangThaiKhachHang();
            cboTrangthai.DisplayMember = "tenTrangThai";
            cboTrangthai.ValueMember = "maTrangThai";
            cboTrangthai.SelectedIndex = -1;
            ApplyRoundedInput(panelTrangthai, cboTrangthai, 15, 2, Color.Gray);
        }

        private void LocThongBao_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ Tạo DataTable thủ công cho loại thông báo
                DataTable dtLoaiTB = new DataTable();
                dtLoaiTB.Columns.Add("loaiTB", typeof(string));

                dtLoaiTB.Rows.Add("Trễ hạn quan trắc");
                dtLoaiTB.Rows.Add("Hợp đồng quá hạn");
                dtLoaiTB.Rows.Add("Nhắc sắp đến hạn trả kết quả");

                // ✅ Bind vào ComboBox
                cboTrangthai.DataSource = dtLoaiTB;
                cboTrangthai.DisplayMember = "loaiTB";
                cboTrangthai.ValueMember = "loaiTB";
                cboTrangthai.SelectedIndex = -1; // Không chọn gì

                // ✅ Apply bo góc
                ApplyRoundedInput(panelTrangthai, cboTrangthai, 15, 2, Color.Gray);

                // ✅ Bo góc button
                BoGocButton(btnApdung, 20);
                BoGocButton(btnHuy, 20);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo bộ lọc: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApdung_Click(object sender, EventArgs e)
        {
            SelectedLoaiTB = cboTrangthai.SelectedIndex >= 0 ? cboTrangthai.SelectedValue.ToString() : null;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
