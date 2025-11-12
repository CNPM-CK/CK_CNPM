using BLL;
using DTO;
using GUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class NenMauNhapLieuConTrol : UserControl
    {
        private DotNenThongSoNhapLieuDTO ds;
        private readonly DotQuanTracNhapLieuBLL _bll = new DotQuanTracNhapLieuBLL();
        private readonly BindingSource _bs = new BindingSource();
        private Form currentOpenForm = null;

        public string MaDN { get; private set; }

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
        }

        public NenMauNhapLieuConTrol(string maDN)
        {
            InitializeComponent();
            this.MaDN = maDN;
            var bll = new DotQuanTracNhapLieuBLL();
            this.ds = bll.LayDotNenTheoMaDotNen(maDN);
            txtTennenmau.Text = ds.TenNen;
            txtVitri.Text = ds.TenViTri;
            textBox1.Text = ds.ToaDo;
            txtMota.Text = ds.GhiChu;
            InitializeGridColumns();
            LoadNenMau();
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
            dgvThongso.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongso.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "TRẠNG THÁI",
                Name = "TrangThai"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenTS",
                HeaderText = "TÊN THÔNG SỐ",
                Name = "TenTS"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonVi",
                HeaderText = "ĐƠN VỊ",
                Name = "DonVi"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiThieu",
                HeaderText = "GIÁ TRỊ TỐI THIỂU",
                Name = "GiaTriToiThieu"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiDa",
                HeaderText = "GIÁ TRỊ TỐI ĐA",
                Name = "GiaTriToiDa"
            });
            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PhuongPhap",
                HeaderText = "PHƯƠNG PHÁP",
                Name = "PhuongPhap"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriDoDuoc",
                HeaderText = "GIÁ TRỊ ĐO ĐƯỢC",
                Name = "GiaTriDoDuoc"
            });

            DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
            {
                Name = "ThaoTac",
                HeaderText = "THAO TÁC",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            };
            dgvThongso.Columns.Add(thaoTacCol);
            dgvThongso.ReadOnly = true;
            dgvThongso.Columns["ThaoTac"].ReadOnly = false;

            dgvThongso.CellPainting += DgvDsdotquantrac_CellPainting;
            dgvThongso.CellClick += DgvDsdotquantrac_CellClick;
        }
        private Rectangle editRect;
        public string GetMoTa()
        {
            return txtMota.Text;
        }

        public void LoadNenMau()
        {
            InitializeButtonStyles();
            try
            {
                string? maPhong = SessionStore.Current.MaPhong;
                if (string.IsNullOrEmpty(maPhong))
                    maPhong = "P003"; // tạm

                var lst = _bll.LayDanhSachThongSoTheoDotNenVaPhong(this.MaDN, maPhong)
                          ?? new List<ThongSoNhapLieuDTO>();

                _bs.DataSource = lst;
                dgvThongso.AutoGenerateColumns = false;
                dgvThongso.DataSource = _bs; 
                dgvThongso.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách thông số: " + ex.Message);
            }
        }


        public void SetIndex(int index)
        {
            lblNenmau.Text = $"Nền mẫu {index}";
        }

        public event EventHandler nhanXoaNenMau;

        public event EventHandler nhanSuaNenMau;

        private void btnSua_Click(object sender, EventArgs e)
        {
            nhanSuaNenMau?.Invoke(this, EventArgs.Empty);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            nhanXoaNenMau?.Invoke(this, EventArgs.Empty);
        }


        private void NenMauConTrol_Load(object sender, EventArgs e)
        {

            ApplyRoundedInput(panelMota, txtMota, 12, 2, Color.FromArgb(0, 152, 70));

        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void panel2_Paint(object sender, PaintEventArgs e) { }

        private void dgvThongso_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void txtTennenmau_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {}
        private void DgvDsdotquantrac_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvThongso.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);
                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 15;
                int totalWidth = (iconWidth) + spacing;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                if (Properties.Resources.edit != null)
                {
                    e.Graphics.DrawImage(Properties.Resources.edit, editRect);
                }
                e.Handled = true;
            }
        }

        private void DgvDsdotquantrac_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvThongso.Columns["ThaoTac"].Index) return;

            var item = dgvThongso.Rows[e.RowIndex].DataBoundItem as ThongSoNhapLieuDTO;
            if (item == null) return;

            HandleEdit(item);
        }

        private void HandleEdit(ThongSoNhapLieuDTO item)
        {
            using (var frm = new NhapThongSo(item.MaDNTS))
            {
                CenterFormOnParent(frm);
                frm.ShowDialog(this.FindForm());
            }

        }
        private void CenterFormOnParent(Form childForm)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                childForm.StartPosition = FormStartPosition.Manual;
                childForm.Location = new Point(
                    parentForm.Location.X + (parentForm.Width - childForm.Width) / 2,
                    parentForm.Location.Y + (parentForm.Height - childForm.Height) / 2
                );
            }
        }
    }

}