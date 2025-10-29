using DTO;
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
    public partial class NenMauConTrol : UserControl
    {
        private BindingList<ChiTietQuanTracView> dsThongSo;
        public string MaNen { get; private set; }

        public string MaDN { get; private set; }
        public string TenNenMau
        {
            get { return txtTennenmau.Text; }
            set { txtTennenmau.Text = value; }
        }

        public string TenViTri { get; set; }
        public string ToaDo { get; set; }
        public string GhiChu { get; set; }


        public string TenNenMauDaChon { get; private set; }

        #region Bo góc button
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
        #endregion
        private void InitializeButtonStyles()
        {
            BoGocButton(btnSua, 20);
            BoGocButton(btnXoa, 20);
        }

        public NenMauConTrol()
        {
            InitializeComponent();
            InitializeGridColumns();
        }

        private void InitializeGridColumns()
        {
            dgvThongso.AutoGenerateColumns = false;
            dgvThongso.Columns.Clear();
            dgvThongso.AllowUserToAddRows = false;
            dgvThongso.ReadOnly = true;
            dgvThongso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongso.MultiSelect = false;
            dgvThongso.RowTemplate.Height = 50;

            // Font settings
            dgvThongso.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvThongso.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvThongso.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvThongso.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F);

            // Header styling
            dgvThongso.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvThongso.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvThongso.EnableHeadersVisualStyles = false;

            // Cell styling
            dgvThongso.DefaultCellStyle.BackColor = Color.White;
            dgvThongso.DefaultCellStyle.ForeColor = Color.Black;
            dgvThongso.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvThongso.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvThongso.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenTS",
                HeaderText = "Tên Thông Số",
                Name = "TenTS"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonVi",
                HeaderText = "Đơn vị",
                Name = "DonVi"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiThieu",
                HeaderText = "Giá trị tối thiểu",
                Name = "GiaTriToiThieu"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiDa",
                HeaderText = "Giá trị tối đa",
                Name = "GiaTriToiDa"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenPhong",
                HeaderText = "Phòng",
                Name = "TenPhong"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PhuongPhap",
                HeaderText = "Phương pháp",
                Name = "PhuongPhap"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaTS",
                DataPropertyName = "MaTS",
                Visible = false
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaPhong",
                DataPropertyName = "MaPhong",
                Visible = false
            });
        }

        public List<ChiTietQuanTracView> GetDanhSachThongSo()
        {
            return dsThongSo?.ToList() ?? new List<ChiTietQuanTracView>();
        }

        public string GetMoTa()
        {
            return txtMota.Text;
        }

        public void LoadNenMau(string maDN, string maNen, string tenNenMau, string moTaNen, List<ChiTietQuanTracView> chiTiet, string viTri = "", string toaDo = "", string ghiChu = "")
        {
            InitializeButtonStyles();
            try
            {
                this.MaDN = maDN;
                this.MaNen = maNen;
                this.TenNenMauDaChon = tenNenMau;
                this.TenViTri = viTri;       
                this.ToaDo = toaDo;
                this.GhiChu = ghiChu;
                txtTennenmau.Text = tenNenMau ?? string.Empty;
                txtMota.Text = moTaNen ?? string.Empty;

                if (chiTiet == null || chiTiet.Count == 0)
                {
                    chiTiet = new List<ChiTietQuanTracView>();
                }

                dsThongSo = new BindingList<ChiTietQuanTracView>(chiTiet);
                dgvThongso.DataSource = null;
                dgvThongso.DataSource = dsThongSo;
                dgvThongso.Refresh();

                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi LoadNenMau: {ex.Message}");
            }
        }


        public void SetIndex(int index)
        {
            lblNenmau.Text = $"Nền mẫu {index}";
        }

        public event EventHandler XoaNenMauClicked;

        public event EventHandler SuaNenMauClicked;

        private void btnSua_Click(object sender, EventArgs e)
        {
            SuaNenMauClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            XoaNenMauClicked?.Invoke(this, EventArgs.Empty);
        }
        

        private void NenMauConTrol_Load(object sender, EventArgs e)
        {

            ApplyRoundedInput(panelMota, txtMota, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTennenmau, txtTennenmau, 12, 2, Color.FromArgb(0, 152, 70));
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void panel2_Paint(object sender, PaintEventArgs e) { }

        private void dgvThongso_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void txtTennenmau_TextChanged(object sender, EventArgs e) { }



        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }


    }
}