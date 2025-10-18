using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachKhachHang : Form
    {
        #region Fields

        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm khách hàng...";

        private BindingList<KhachHang> dsKhachhang;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";

        public DanhSachKhachHang()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        #endregion

        #region Form Load
        private void DanhSachNhanVien_Load(object sender, EventArgs e)
        {
            KhachHangBLL khBLL = new KhachHangBLL();
            dsKhachhang = new BindingList<KhachHang>(khBLL.LayDanhSachKH());

            InitializeContextMenu();
            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();
            InitializeSettingMenu();
            InitializeDataGridView();
            CalculateLayout();
        }

        private void InitializeDataGridView()
        {
            dgvDanhsachnhanvien.AutoGenerateColumns = false;
            dgvDanhsachnhanvien.Columns.Clear();

            dgvDanhsachnhanvien.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "maKH", HeaderText = "Mã khách hàng", Name = "maKH" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tenDoanhNghiep", HeaderText = "Tên doanh nghiệp", Name = "tenDoanhNghiep" },
                new DataGridViewTextBoxColumn { DataPropertyName = "kyHieuDN", HeaderText = "Ký hiệu DN", Name = "kyHieuDN" },
                new DataGridViewTextBoxColumn { DataPropertyName = "nguoiDaiDien", HeaderText = "Người đại diện", Name = "nguoiDaiDien" },
                new DataGridViewTextBoxColumn { DataPropertyName = "soDienThoaiKH", HeaderText = "Số điện thoại", Name = "soDienThoaiKH" },
                new DataGridViewTextBoxColumn { DataPropertyName = "diaChi", HeaderText = "Địa chỉ", Name = "diaChi" }
            });

            DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvDanhsachnhanvien.Columns.Add(thaoTacCol);

            dgvDanhsachnhanvien.CellFormatting += dgvDanhsachnhanvien_CellFormatting;

            dgvDanhsachnhanvien.DataSource = dsKhachhang;
            dgvDanhsachnhanvien.ReadOnly = true;
            dgvDanhsachnhanvien.Columns["ThaoTac"].ReadOnly = false;
        }

        private void RefreshDanhSachKhachHang()
        {
            KhachHangBLL khBLL = new KhachHangBLL();
            dsKhachhang.Clear();
            foreach (var kh in khBLL.LayDanhSachKH())
                dsKhachhang.Add(kh);
        }

        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 10;
                int totalWidth = (iconWidth * 2) + spacing;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);

                deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

                e.Handled = true;
            }
        }

        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
                return;

            var clickPoint = dgvDanhsachnhanvien.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvDanhsachnhanvien.Rows[e.RowIndex];

            if (row.Cells["maKH"].Value == null) return;

            if (editRect.Contains(clickPoint))
            {
                HandleEdit(row);
            }
            else if (deleteRect.Contains(clickPoint))
            {
                HandleDelete(row);
            }
        }

        private void HandleEdit(DataGridViewRow row)
        {
            KhachHang kh = new KhachHang
            {
                maKH = row.Cells["maKH"].Value.ToString(),
                soDienThoaiKH = row.Cells["soDienThoaiKH"].Value?.ToString(),
                tenDoanhNghiep = row.Cells["tenDoanhNghiep"].Value?.ToString(),
                nguoiDaiDien = row.Cells["nguoiDaiDien"].Value?.ToString(),
                diaChi = row.Cells["diaChi"].Value?.ToString(),
                kyHieuDN = row.Cells["kyHieuDN"].Value?.ToString()
            };

            SuaKhachHang frmSua = new SuaKhachHang(kh);
            CenterFormOnParent(frmSua);
            frmSua.SuccesfullyUpdated += (s, ev) => RefreshDanhSachKhachHang();
            frmSua.Show(this);
        }

        private void HandleDelete(DataGridViewRow row)
        {
            string maKH = row.Cells["maKH"].Value.ToString();
            string tenDN = row.Cells["tenDoanhNghiep"].Value?.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{tenDN}' (Mã: {maKH}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    KhachHangBLL khBLL = new KhachHangBLL();
                    khBLL.XoaKhachHang(maKH);

                    MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshDanhSachKhachHang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa khách hàng: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CenterFormOnParent(Form childForm)
        {
            childForm.StartPosition = FormStartPosition.Manual;
            childForm.Location = new Point(
                this.Location.X + (this.Width - childForm.Width) / 2,
                this.Location.Y + (this.Height - childForm.Height) / 2
            );
        }

        private void dgvDanhsachnhanvien_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvDanhsachnhanvien.Width;
            int dgvHeight = dgvDanhsachnhanvien.Height;
            Image watermark = Properties.Resources.greenlogo;

            int x = (dgvWidth - watermark.Width) / 2;
            int y = (dgvHeight - watermark.Height) / 2;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.3f;
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(watermark,
                new Rectangle(x, y, watermark.Width, watermark.Height),
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        private void InitializeCustomSearchBox()
        {
            containersearch.BackColor = Color.Transparent;
            containersearch.Size = new Size(400, SEARCH_HEIGHT);
            containersearch.BringToFront();

            searchtextbox.BorderStyle = BorderStyle.None;
            searchtextbox.BackColor = Color.White;
            searchtextbox.Font = new Font("Segoe UI", 10F);
            searchtextbox.ForeColor = Color.Silver;
            searchtextbox.Text = PLACEHOLDER_TEXT;
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            searchtextbox.Size = new Size(containersearch.Width - (borderSize * 2 + 10), 28);
            searchtextbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            containersearch.Controls.Add(searchtextbox);

            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            searchtextbox.KeyDown += searchtextbox_KeyDown;
            containersearch.Paint += containersearch_Paint;
        }

        private void InitializeContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pdfItem = new ToolStripMenuItem("Xuất PDF");
            pdfItem.Click += (s, ev) => { MessageBox.Show("Xuất PDF..."); };

            ToolStripMenuItem excelItem = new ToolStripMenuItem("Xuất Excel");
            excelItem.Click += (s, ev) => { MessageBox.Show("Xuất Excel..."); };

            menu.Items.Add(pdfItem);
            menu.Items.Add(excelItem);

            btnXuatfile.Click += (s, ev) =>
            {
                menu.Show(btnXuatfile, new Point(0, btnXuatfile.Height));
            };
        }

        private void InitializeSettingMenu()
        {
            ContextMenuStrip settingMenu = new ContextMenuStrip();

            ToolStripMenuItem personalItem = new ToolStripMenuItem("Cài đặt cá nhân");
            personalItem.Click += (s, ev) =>
            {
                MessageBox.Show("Mở trang cài đặt cá nhân...", "Thông báo");
            };

            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Đăng xuất");
            logoutItem.Click += (s, ev) =>
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    DangNhap loginForm = new DangNhap();
                    loginForm.Show();
                }
            };

            settingMenu.Items.Add(personalItem);
            settingMenu.Items.Add(new ToolStripSeparator());
            settingMenu.Items.Add(logoutItem);

            pictureBoxSetting.Click += (s, ev) =>
            {
                settingMenu.Show(pictureBoxSetting, new Point(0, pictureBoxSetting.Height));
            };
        }

        private void InitializeButtonIcons()
        {
            if (btnDanhsachnv.Image != null)
            {
                btnDanhsachnv.Image = new Bitmap(btnDanhsachnv.Image, new Size(30, 30));
                btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleRight;
                btnDanhsachnv.Padding = new Padding(0, 0, 4, 0);
            }

            if (btnThemuser.Image != null)
            {
                btnThemuser.Image = new Bitmap(btnThemuser.Image, new Size(24, 24));
            }

            if (btnXuatfile.Image != null)
            {
                btnXuatfile.Image = new Bitmap(btnXuatfile.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            btnThemuser.Size = new Size(66, 40);
            btnXuatfile.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
            BoGocButton(btnXuatfile, 20);
        }
        #endregion

        #region Layout & Resize
        private void DanhSachNhanVien_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            int formWidth = this.ClientSize.Width;
            bool isMaximized = this.WindowState == FormWindowState.Maximized;
            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemuser, btnRadius);

            btnXuatfile.Left = formWidth - btnWidth - MARGIN;
            btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;

            pictureFilter.Left = MARGIN;

            int leftBoundary = pictureFilter.Right + SPACING;
            int rightBoundary = btnThemuser.Left - SPACING - picturemicro.Width - SPACING;
            int availableWidth = rightBoundary - leftBoundary;

            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            picturemicro.Left = containersearch.Right + SPACING;

            if (isMaximized)
            {
                btnThemuser.Padding = new Padding(10, 5, 10, 5);
                btnXuatfile.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemuser.Padding = new Padding(5, 3, 5, 3);
                btnXuatfile.Padding = new Padding(5, 3, 5, 3);
            }

            containersearch.Invalidate();
        }
        #endregion

        #region Button Styling
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
        #endregion

        #region Custom Search Box Paint
        private void containersearch_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float offset = borderSize / 2f;
            RectangleF rect = new RectangleF(
                offset,
                offset,
                containersearch.ClientSize.Width - borderSize,
                containersearch.ClientSize.Height - borderSize
            );

            using (GraphicsPath path = CreateRoundedRectPath(rect, borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            float effectiveRadius = Math.Min(radius, Math.Min(rect.Width / 2f, rect.Height / 2f));
            float diameter = effectiveRadius * 2f;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion

        #region TextBox Events - TÍNH NĂNG TÌM KIẾM REAL-TIME
        private void searchtextbox_Enter(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Enter - isPlaceholder: {isPlaceholder}, Text: '{searchtextbox.Text}'");
            if (isPlaceholder)
            {
                isPlaceholder = false;
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                dgvDanhsachnhanvien.DataSource = dsKhachhang;
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvDanhsachnhanvien.Rows.Count > 0)
                {
                    dgvDanhsachnhanvien.ClearSelection();
                    dgvDanhsachnhanvien.Rows[0].Selected = true;
                    dgvDanhsachnhanvien.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvDanhsachnhanvien.DataSource = dsKhachhang;
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_TextChanged_1(object sender, EventArgs e)
        {
            if (isPlaceholder)
                return;
            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == lastSearchKeyword)
                return;
            lastSearchKeyword = currentKeyword;

            PerformSearch();
        }

        private void PerformSearch()
        {
            string keyword = searchtextbox.Text?.Trim().ToLower() ?? "";
            if (string.IsNullOrEmpty(keyword))
            {
                dgvDanhsachnhanvien.DataSource = dsKhachhang;
                return;
            }

            var filtered = dsKhachhang
                .Where(kh =>
                    (kh.tenDoanhNghiep ?? "").ToLower().Contains(keyword) ||
                    (kh.nguoiDaiDien ?? "").ToLower().Contains(keyword) ||
                    (kh.kyHieuDN ?? "").ToLower().Contains(keyword) ||
                    (kh.soDienThoaiKH ?? "").Contains(keyword) ||
                    (kh.diaChi ?? "").ToLower().Contains(keyword) ||
                    kh.maKH.ToString().ToLower().Contains(keyword)
                )
                .ToList();
            dgvDanhsachnhanvien.DataSource = new BindingList<KhachHang>(filtered);
        }
        #endregion

        #region Button Events
        private void btnThemuser_Click(object sender, EventArgs e)
        {
            ThemKhachHang frmThem = new ThemKhachHang();
            CenterFormOnParent(frmThem);
            frmThem.SuccesfullyUpdated += (s, ev) => RefreshDanhSachKhachHang();
            frmThem.Show(this);
        }

        private void dgvDanhsachnhanvien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        #endregion

        #region Unused Events
        private void DanhSachNhanVien_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void dgvDanhsachnhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void containersearch_Paint_1(object sender, PaintEventArgs e) { }
        private void btnDanhsachnv_Click(object sender, EventArgs e) { }
        private void searchtextbox_TextChanged(object sender, EventArgs e) { }
        #endregion
    }
}