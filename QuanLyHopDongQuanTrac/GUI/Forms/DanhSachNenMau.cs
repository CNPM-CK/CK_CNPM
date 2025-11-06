using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GUI.Forms
{
    public partial class DanhSachNenMau : UserControl
    {
        #region Fields
        // Search box styling
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm thông số...";

        // Layout constants (giống DanhSachNhanVien)
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private BindingList<NenMau> dsNenmau;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        // Cell action rectangles
        #endregion

        #region Constructor
        public DanhSachNenMau()
        {
            InitializeComponent();
            this.Load += DanhSachThongSo_Load;
            this.Resize += DanhSachThongSo_Resize; // ✅ Thêm resize handler
        }
        #endregion

        #region Initialization
        private void DanhSachThongSo_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                InitializeDataGridView();
                InitializeCustomSearchBox();
                InitializeContextMenu();
                InitializeButtonIcons();
                InitializeButtonStyles();
                CalculateLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            var bll = new NenMauBLL();
            var list = bll.layDSNenMau();
            dsNenmau = new BindingList<NenMau>(list);
        }

        private void InitializeDataGridView()
        {
            dgvDSTS.AutoGenerateColumns = false;
            dgvDSTS.Columns.Clear();
            dgvDSTS.AllowUserToAddRows = false;
            dgvDSTS.ReadOnly = true;
            dgvDSTS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDSTS.MultiSelect = false;
            dgvDSTS.RowTemplate.Height = 50;

            dgvDSTS.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular); // Font chữ
            dgvDSTS.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F); // Font cells
            dgvDSTS.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // Font header
            dgvDSTS.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F); // Font row header

            dgvDSTS.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70); // Màu nền
            dgvDSTS.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                // Màu chữ
            dgvDSTS.EnableHeadersVisualStyles = false; // Bắt buộc để header nhận màu tùy chỉnh


            dgvDSTS.DefaultCellStyle.BackColor = Color.White;          // Màu nền bình thường
            dgvDSTS.DefaultCellStyle.ForeColor = Color.Black;          // Màu chữ
            dgvDSTS.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151); // Màu nền khi chọn
            dgvDSTS.DefaultCellStyle.SelectionForeColor = Color.Black; // Màu chữ khi chọn
            dgvDSTS.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // Define columns
            dgvDSTS.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "maNen",
                    HeaderText = "Mã nền ",
                    Name = "maNen",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "tenNenMau",
                    HeaderText = "Tên nền mẫu ",
                    Name = "tenNenMau",
                    Width = 250
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "moTa",
                    HeaderText = "Mô Tả",
                    Name = "moTa",
                    Width = 100
                }

            });

            // Add action column
            DataGridViewImageColumn actionCol = new DataGridViewImageColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 100
            };
            dgvDSTS.Columns.Add(actionCol);
            dgvDSTS.Columns["ThaoTac"].ReadOnly = false;

            // Bind data
            dgvDSTS.DataSource = dsNenmau;

            // Register events
            dgvDSTS.CellFormatting += DgvDSTS_CellFormatting;
            dgvDSTS.CellPainting += DgvDSTS_CellPainting;
            dgvDSTS.CellClick += DgvDSTS_CellClick;
        }

        private void InitializeCustomSearchBox()
        {
            if (containersearch == null || searchtextbox == null) return;

            containersearch.BackColor = Color.Transparent;
            containersearch.Size = new Size(400, SEARCH_HEIGHT); // Sẽ được điều chỉnh trong CalculateLayout
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

            // Events
            searchtextbox.Enter += Searchtextbox_Enter;
            searchtextbox.Leave += Searchtextbox_Leave;
            searchtextbox.TextChanged += Searchtextbox_TextChanged;
            searchtextbox.KeyDown += Searchtextbox_KeyDown;
            containersearch.Paint += Containersearch_Paint;
        }

        private void InitializeContextMenu()
        {
            if (btnXuatfile == null) return;

            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pdfItem = new ToolStripMenuItem("Xuất PDF");
            pdfItem.Click += (s, ev) => ExportToPDF();

            ToolStripMenuItem excelItem = new ToolStripMenuItem("Xuất Excel");
            excelItem.Click += (s, ev) => ExportToExcel();

            menu.Items.Add(pdfItem);
            menu.Items.Add(excelItem);

            btnXuatfile.Click += (s, ev) =>
            {
                menu.Show(btnXuatfile, new Point(0, btnXuatfile.Height));
            };
        }

        private void InitializeButtonIcons()
        {
            // ✅ Resize icons giống DanhSachNhanVien
            if (btnThemNenMau != null && btnThemNenMau.Image != null)
            {
                btnThemNenMau.Image = new Bitmap(btnThemNenMau.Image, new Size(24, 24));
            }

            if (btnXuatfile != null && btnXuatfile.Image != null)
            {
                btnXuatfile.Image = new Bitmap(btnXuatfile.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            // ✅ Set initial button size và bo góc
            if (btnThemNenMau != null)
            {
                btnThemNenMau.Size = new Size(66, 40);
                BoGocButton(btnThemNenMau, 20);
                //btnThemNenMau.Click += btnThemNenMau_Click;
            }

            if (btnXuatfile != null)
            {
                btnXuatfile.Size = new Size(66, 40);
                BoGocButton(btnXuatfile, 20);
            }
        }
        #endregion

        #region Layout & Resize (✅ COPY TỪ DanhSachNhanVien)
        private void DanhSachThongSo_Resize(object sender, EventArgs e)
        {
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            // Kiểm tra các controls tồn tại
            if (btnXuatfile == null || btnThemNenMau == null || containersearch == null) return;

            int formWidth = this.Width;

            // Kiểm tra parent form có maximize không
            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            // Resize và reposition buttons
            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemNenMau.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemNenMau, btnRadius);

            // Position buttons from right
            btnXuatfile.Left = formWidth - btnWidth - MARGIN;
            btnThemNenMau.Left = btnXuatfile.Left - btnWidth - SPACING;

            // Calculate search box width dynamically
            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = btnThemNenMau.Left - SPACING;

            if (picturemicro != null)
            {
                rightBoundary -= picturemicro.Width + SPACING;
            }

            int availableWidth = rightBoundary - leftBoundary;

            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            // Apply search box sizing
            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
            }

            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            if (picturemicro != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
            }

            // Adjust button padding based on maximize state
            if (isMaximized)
            {
                btnThemNenMau.Padding = new Padding(10, 5, 10, 5);
                btnXuatfile.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemNenMau.Padding = new Padding(5, 3, 5, 3);
                btnXuatfile.Padding = new Padding(5, 3, 5, 3);
            }

            containersearch.Invalidate();
        }
        #endregion

        #region Button Styling (✅ COPY TỪ DanhSachNhanVien)
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

        #region DataGridView Events
        private void DgvDSTS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Highlight min/max values that are null
            if ((dgvDSTS.Columns[e.ColumnIndex].Name == "GiaTriToiThieu" ||
                 dgvDSTS.Columns[e.ColumnIndex].Name == "GiaTriToiDa") &&
                (e.Value == null || e.Value == DBNull.Value))
            {
                e.CellStyle.BackColor = Color.LightYellow;
                e.CellStyle.ForeColor = Color.Gray;
                e.Value = "N/A";
                e.FormattingApplied = true;
            }
        }

        private void DgvDSTS_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDSTS.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 10;
                int totalWidth = (iconWidth * 2) + spacing;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                // ✅ Chỉ vẽ, KHÔNG lưu Rectangle
                Rectangle editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                if (Properties.Resources.edit != null)
                {
                    e.Graphics.DrawImage(Properties.Resources.edit, editRect);
                }

                Rectangle deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);
                if (Properties.Resources.trash_can != null)
                {
                    e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);
                }

                e.Handled = true;
            }
        }


        private void DgvDSTS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Validate indices
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                // Validate ThaoTac column exists
                if (!dgvDSTS.Columns.Contains("ThaoTac"))
                {
                    MessageBox.Show("Cột 'Thao tác' không tồn tại!", "Lỗi");
                    return;
                }

                // Check if clicked column is ThaoTac
                if (e.ColumnIndex != dgvDSTS.Columns["ThaoTac"].Index)
                    return;

                // Validate row index
                if (e.RowIndex >= dgvDSTS.Rows.Count)
                    return;

                DataGridViewRow row = dgvDSTS.Rows[e.RowIndex];

                // Validate maNen cell
                if (row?.DataGridView?.Columns["maNen"] == null) return;
                var value = row.Cells["maNen"]?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return;


                // Get cell bounds
                Rectangle cellBounds = dgvDSTS.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                if (cellBounds.IsEmpty || cellBounds.Width == 0 || cellBounds.Height == 0)
                    return;

                // Calculate icon positions
                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 10;
                int totalWidth = (iconWidth * 2) + spacing;

                int startX = cellBounds.Left + (cellBounds.Width - totalWidth) / 2;
                int startY = cellBounds.Top + (cellBounds.Height - iconHeight) / 2;

                Rectangle editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                Rectangle deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

                // Get click point
                Point clickPoint = dgvDSTS.PointToClient(Cursor.Position);

                // Check which icon was clicked
                if (editRect.Contains(clickPoint))
                {
                    HandleEdit(row);
                }
                else if (deleteRect.Contains(clickPoint))
                {
                    HandleDelete(row);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Lỗi index: {ex.Message}\n\nRow: {e.RowIndex}, Col: {e.ColumnIndex}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region CRUD Operations
        private void HandleEdit(DataGridViewRow row)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy dữ liệu từ dòng được chọn
            string maNen = row.Cells["maNen"].Value?.ToString();
            string tenNenmau = row.Cells["tenNenMau"].Value?.ToString();
            string moTa = row.Cells["moTa"].Value?.ToString();

            // Tạo đối tượng NenMau với đầy đủ thông tin
            NenMau nm = new NenMau
            {
                maNen = maNen,
                tenNenMau = tenNenmau,
                moTa = moTa
            };

            // Khởi tạo form sửa
            ThemNenMau1 frmSua = new ThemNenMau1();
            frmSua.isEditMode = true;
            frmSua.NenMauHienTai = nm;  // Truyền dữ liệu vào form

            currentOpenForm = frmSua;

            // Căn giữa form trên parent
            CenterFormOnParent(frmSua);

            // Khi form đóng thì reset
            frmSua.FormClosed += (s, ev) => { currentOpenForm = null; };

            // Khi form sửa thành công, load lại danh sách
            frmSua.SuccessfullyUpdated += (s, ev) => RefreshDanhSachNenMau();

            // Hiển thị form
            frmSua.Show(this.FindForm());

        }

        private void HandleDelete(DataGridViewRow row)
        {
            string maNen = row.Cells["maNen"].Value.ToString();
            string tenNenMau = row.Cells["tenNenMau"].Value.ToString();


            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nền mẫu '{tenNenMau}' (Mã: {maNen}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    NenMauBLL nvBLL = new NenMauBLL();
                    nvBLL.xoaNenMau(maNen);

                    MessageBox.Show("Xóa nền mẫu thành công !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshDanhSachNenMau();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa nền mẫu : " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Search Functionality
        private void Searchtextbox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                isPlaceholder = false;
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void Searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                dgvDSTS.DataSource = dsNenmau;
                lastSearchKeyword = "";
            }
        }

        private void Searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvDSTS.Rows.Count > 0)
                {
                    dgvDSTS.ClearSelection();
                    dgvDSTS.Rows[0].Selected = true;
                    dgvDSTS.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvDSTS.DataSource = dsNenmau;
                lastSearchKeyword = "";
            }
        }

        private void Searchtextbox_TextChanged(object sender, EventArgs e)
        {
            if (isPlaceholder) return;

            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == lastSearchKeyword) return;

            lastSearchKeyword = currentKeyword;
            PerformSearch();
        }

        private void PerformSearch()
        {
            string keyword = searchtextbox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dgvDSTS.DataSource = dsNenmau;
                return;
            }

            var filtered = dsNenmau
                .Where(ts =>
                    (ts.maNen ?? "").ToLower().Contains(keyword) ||
                    (ts.tenNenMau ?? "").ToLower().Contains(keyword) ||
                    (ts.moTa ?? "").ToLower().Contains(keyword)

                )
                .ToList();

            dgvDSTS.DataSource = new BindingList<NenMau>(filtered);
        }
        #endregion

        #region Custom Paint
        private void Containersearch_Paint(object sender, PaintEventArgs e)
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

        #region Button Events

        private void ExportToPDF()
        {
            MessageBox.Show("Chức năng xuất PDF đang được phát triển.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportToExcel()
        {
            MessageBox.Show("Chức năng xuất Excel đang được phát triển.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Helper Methods
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

        private void dgvDSKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Legacy method - kept for compatibility
        }
        #endregion

        private void btnThemuser_Click_1(object sender, EventArgs e)
        {
            ThemNenMau1 themNenMau1 = new ThemNenMau1();
            themNenMau1.StartPosition = FormStartPosition.CenterParent;
            var result = themNenMau1.ShowDialog();
            if (result == DialogResult.OK)
            {
                RefreshDanhSachNenMau();
            }
        }
        private void RefreshDanhSachNenMau()
        {
            NenMauBLL tsBLL = new NenMauBLL();
            dsNenmau.Clear();
            foreach (var nm in tsBLL.layDSNenMau())
                dsNenmau.Add(nm);
        }
    }
}