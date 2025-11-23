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
    public partial class ThemThongSo : Form
    {
        private bool isEditMode = false;
        private ThongSo thongSoCanSua = null;
        
        public ThemThongSo()
        {
            InitializeComponent();
            isEditMode = false;
        }

        public ThemThongSo(ThongSo thongSo)
        {
            InitializeComponent();
            isEditMode = true;
            thongSoCanSua = thongSo;
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

        private void ThemThongSo_Load(object sender, EventArgs e)
        {

            ApplyRoundedInput(panelDonvi, txtDonvi, 20, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMax, txtMax, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelMin, txtMin, 15, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTents, txtTents, 20, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelPhuongphap, txtPhuongphap, 20, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();

            if (isEditMode)
            {
                this.Text = "Sửa thông số môi trường";
                //Nếu có label header, đổi text của nó
                label.Text = "Sửa thông số môi trường";
                btnThem.Text = "Cập nhật";

                // Load dữ liệu vào các textbox
                if (thongSoCanSua != null)
                {
                    txtTents.Text = thongSoCanSua.TenTS;
                    txtDonvi.Text = thongSoCanSua.DonVi;
                    txtPhuongphap.Text = thongSoCanSua.phuongPhap;
                    txtMin.Text = thongSoCanSua.GiaTriToiThieu?.ToString();
                    txtMax.Text = thongSoCanSua.GiaTriToiDa?.ToString();
                }
            }

            else
            {
                this.Text = "Thêm thông số môi trường";
                label.Text = "Thêm thông số môi trường";
                btnThem.Text = "Thêm";
            }

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtTents.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thông số!", "Thông báo");
                    txtTents.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDonvi.Text))
                {
                    MessageBox.Show("Vui lòng nhập đơn vị!", "Thông báo");
                    txtDonvi.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPhuongphap.Text))
                {
                    MessageBox.Show("Vui lòng nhập phương pháp!", "Thông báo");
                    txtDonvi.Focus();
                    return;
                }

                // Parse giá trị min/max
                double? giaTriMin = null;
                double? giaTriMax = null;

                if (!string.IsNullOrEmpty(txtMin.Text))
                {
                    if (!double.TryParse(txtMin.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double min))
                    {
                        MessageBox.Show("Giá trị tối thiểu không hợp lệ!", "Lỗi");
                        txtMin.Focus();
                        return;
                    }
                    giaTriMin = min;
                }

                if (!string.IsNullOrEmpty(txtMax.Text))
                {
                    if (!double.TryParse(txtMax.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double max))
                    {
                        MessageBox.Show("Giá trị tối đa không hợp lệ!", "Lỗi");
                        txtMax.Focus();
                        return;
                    }
                    giaTriMax = max;
                }

                // Kiểm tra logic min < max
                if (giaTriMin.HasValue && giaTriMax.HasValue && giaTriMin.Value > giaTriMax.Value)
                {
                    MessageBox.Show("Giá trị tối thiểu không được lớn hơn giá trị tối đa!", "Lỗi");
                    return;
                }

                var bll = new ThongSoBLL();
                if (isEditMode)
                {
                    // Chế độ sửa
                    thongSoCanSua.TenTS = txtTents.Text.Trim();
                    thongSoCanSua.DonVi = txtDonvi.Text.Trim();
                    thongSoCanSua.phuongPhap = txtPhuongphap.Text.Trim();
                    thongSoCanSua.GiaTriToiDa = giaTriMax;
                    thongSoCanSua.GiaTriToiThieu = giaTriMin;

                    if (bll.suaThongSoMoiTruong(thongSoCanSua))
                    {
                        MessageBox.Show("Cập nhật thông số thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại! Vui lòng kiểm tra dữ liệu.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var ts = new ThongSo
                    {
                        TenTS = txtTents.Text.Trim(),
                        DonVi = txtDonvi.Text.Trim(),
                        phuongPhap = txtPhuongphap.Text.Trim(),
                        GiaTriToiDa = giaTriMax,
                        GiaTriToiThieu = giaTriMin
                    };

                    if (bll.themThongSo(ts))
                    {
                        MessageBox.Show("Thêm thông số thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra dữ liệu.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //    var ts = new ThongSo
            //    {
            //        TenTS = txtTents.Text.Trim(),
            //        DonVi = txtDonvi.Text.Trim(),
            //        phuongPhap = txtPhuongphap.Text.Trim(),
            //        GiaTriToiDa = giaTriMax,
            //        GiaTriToiThieu = giaTriMin
            //    };

            //    if (bll.ThemThongSo(ts))
            //    {
            //        MessageBox.Show("Thêm thông số thành công!", "Thành công",
            //            MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        this.DialogResult = DialogResult.OK;
            //        this.Close();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra dữ liệu.", "Lỗi",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Lỗi khi thêm thông số:\n{ex.Message}", "Lỗi",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
