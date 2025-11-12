using BLL;
using DTO;
using GUI.Common;
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
    public partial class NhapThongSo : Form
    {
        private DTO_DotNenTs ds;
        private NhanVien nv;
        public event EventHandler SuccesfullyUpdated;
        private string maDNTS;
        public NhapThongSo(string maDNTS)
        {
            this.maDNTS = maDNTS;
            InitializeComponent();
            var txt = numericUpDown1.Controls[1] as TextBox;
            if (txt != null)
                txt.TextChanged += NumericTextChanged;
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


        private void ThemHopDong_Load(object sender, EventArgs e)
        {
            var bll = new DotQuanTracNhapLieuBLL();
            this.ds = bll.LayThongSoTheoMaDotNenTS(this.maDNTS);
            textBox2.Text = ds.TenTS;
            textBox1.Text = ds.DonVi;
            textBox4.Text = ds.GiaTriToiThieu.ToString();
            textBox3.Text = ds.GiaTriToiDa.ToString();
            numericUpDown1.DecimalPlaces = 2;   
            numericUpDown1.Increment = 0.1M;   
            numericUpDown1.Minimum = int.Parse(ds.GiaTriToiThieu.ToString());      
            numericUpDown1.Maximum = int.Parse(ds.GiaTriToiDa.ToString());      
            numericUpDown1.ThousandsSeparator = true;

            InitializeButtonStyles();
        }
        private void buttonAddnew_Click(object sender, EventArgs e)
        {
            string? userName = SessionStore.Current.UserName;

            if (string.IsNullOrEmpty(userName))
            {
                //MessageBox.Show("Không tìm thấy mã phòng trong phiên đăng nhập!",
                //    "Lỗi session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
                userName = "ptt@gmail.com";
            }
            var bll = new DotQuanTracNhapLieuBLL();
            this.nv = bll.LayNhanVienTheoTenDN(userName);

            KetQua kq = new KetQua
            {
                maDNTS = this.maDNTS,
                ngayDo = dateTimePicker1.Value,
                giaTriDoDuoc = Convert.ToDouble(numericUpDown1.Value),
                nhanVienNhap = nv.maNV
            };
            try
            {
                bll.ThemKetQua(kq);

                MessageBox.Show("Thêm kết quả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SuccesfullyUpdated?.Invoke(this, EventArgs.Empty);
                this.DialogResult = DialogResult.OK;
                this.Close();
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

            numericUpDown1.Value = 0;
            dateTimePicker1.Value = DateTime.Now;

        }
        private void NumericTextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(numericUpDown1.Text, out decimal val))
            {
                if (val > numericUpDown1.Maximum)
                {
                    MessageBox.Show("Giá trị quá lớn!");
                }
                else if (val < numericUpDown1.Minimum)
                {
                    MessageBox.Show("Giá trị quá nhỏ!");
                }
            }
        }


    }
}
