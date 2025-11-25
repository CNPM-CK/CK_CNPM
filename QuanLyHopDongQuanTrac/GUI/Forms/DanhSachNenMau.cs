using BLL;
using BLL.Speech;
using DTO;
using GUI.Common;
using GUI.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachNenMau : UserControl
    {
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;
        private readonly bool _isPhongKeHoach = SessionStore.Current.MaPhong == "P002";

        #region Fields
        // Search box styling
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm nền mẫu...";

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private BindingList<NenMau> dsNenmau;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        // Phân trang
        int trangHientai = 1;
        int kichthuocTrang = 15;
        int tongSoBanGhi = 0;
        int tongSoTrang = 0;
        #endregion

        #region Constructor
        public DanhSachNenMau()
        {
            InitializeComponent();
            this.Load += DanhSachThongSo_Load;
            this.Resize += DanhSachThongSo_Resize;
        }
        #endregion

        #region Initialization
        private async void DanhSachThongSo_Load(object sender, EventArgs e)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string modelPath = Path.Combine(baseDir, "Model", "ggml-tiny.bin");
                _wavPath = Path.Combine(baseDir, "TempAudio", "search.wav");

                _recorder = new VoiceRecorder(_wavPath);
                string appId = "ga825cbd";
                string apiKey = "55774f42c55202232e1b4d8ebfc314c5";
                string apiSecret = "c1cb5fc788d78ec4b808e8cc4beb4a3d";

                var iatService = new IATService(appId, apiKey, apiSecret);

                _whisper = new WhisperService(modelPath, iatService);

                picturemicro.Enabled = false;
                picturemicro.Text = "Đang tải model...";

                try
                {
                    await _whisper.InitAsync();
                    _ready = true;
                    picturemicro.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Lỗi khởi tạo Whisper:\n\n" + ex.ToString(),
                        "Whisper Init Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                InitializeDataGridView();
                InitializeCustomSearchBox();
                InitializeContextMenu();
                InitializeButtonIcons();
                InitializeButtonStyles();
                CalculateLayout();

                // Load dữ liệu phân trang
                taiDanhSachNenMau();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void taiDanhSachNenMau()
        {
            tongSoBanGhi = 0;
            taiTrangNenMau();
        }

        private void taiTrangNenMau()
        {
            var bll = new NenMauBLL();

            // Tính tổng số trang
            if (tongSoBanGhi == 0)
            {
                tongSoBanGhi = bll.demSoLuongNenMau();
                tongSoTrang = (int)Math.Ceiling((double)tongSoBanGhi / kichthuocTrang);
            }

            var data = bll.layDanhSachNenMau_PhanTrang(trangHientai, kichthuocTrang);
            dsNenmau = new BindingList<NenMau>(data);
            dgvDSTS.DataSource = dsNenmau;

            // Cập nhật label trang
            soTrang.Text = $"Trang {trangHientai}/{tongSoTrang}";

            // Disable nút nếu đang ở biên
            btnTruoc.Enabled = trangHientai > 1;
            btnSau.Enabled = trangHientai < tongSoTrang;
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

            dgvDSTS.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvDSTS.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvDSTS.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDSTS.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F);

            dgvDSTS.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvDSTS.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDSTS.EnableHeadersVisualStyles = false;

            dgvDSTS.DefaultCellStyle.BackColor = Color.White;
            dgvDSTS.DefaultCellStyle.ForeColor = Color.Black;
            dgvDSTS.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvDSTS.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDSTS.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDSTS.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Define columns
            dgvDSTS.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "maNen",
                    HeaderText = "MÃ NỀN",
                    Name = "maNen",
                    Visible = false

                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "tenNenMau",
                    HeaderText = "TÊN NỀN MẪU",
                    Name = "tenNenMau",
                    Width= 200
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "moTa",
                    HeaderText = "MÔ TẢ",
                    Name = "moTa",
                }
            });

            dgvDSTS.Columns["tenNenMau"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvDSTS.Columns["tenNenMau"].Width = 300;

            dgvDSTS.Columns["moTa"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Add action column
            if (_isPhongKeHoach)
            {
                DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvDSTS.Columns.Add(thaoTacCol);
            }

            // Register events
            dgvDSTS.CellFormatting += DgvDSTS_CellFormatting;

            if (_isPhongKeHoach)
            {
                dgvDSTS.CellPainting += DgvDSTS_CellPainting;
                dgvDSTS.CellClick += DgvDSTS_CellClick;
            }
        }

        private void InitializeCustomSearchBox()
        {
            if (containersearch == null || searchtextbox == null) return;

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
            btnThemNenMau.Visible = _isPhongKeHoach;
            btnXuatfile.Visible = _isPhongKeHoach;

            if (btnThemNenMau != null)
            {
                btnThemNenMau.Size = new Size(66, 40);
                BoGocButton(btnThemNenMau, 20);
            }

            if (btnXuatfile != null)
            {
                btnXuatfile.Size = new Size(66, 40);
                BoGocButton(btnXuatfile, 20);
            }

            if (btnTruoc != null)
            {
                BoGocButton(btnTruoc, 20);
            }

            if (btnSau != null)
            {
                BoGocButton(btnSau, 20);
            }
        }
        #endregion

        #region Layout & Resize
        private void DanhSachThongSo_Resize(object sender, EventArgs e)
        {
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            if (btnXuatfile == null || btnThemNenMau == null || containersearch == null) return;

            int formWidth = this.Width;
            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemNenMau.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemNenMau, btnRadius);

            Control btnParent = btnXuatfile.Parent;
            if (btnParent != null && btnParent != this)
            {
                int parentWidth = btnParent.Width;
                btnXuatfile.Left = parentWidth - btnWidth - MARGIN;
                btnThemNenMau.Left = btnXuatfile.Left - btnWidth - SPACING;
            }
            else
            {
                btnXuatfile.Left = formWidth - btnWidth - MARGIN;
                btnThemNenMau.Left = btnXuatfile.Left - btnWidth - SPACING;
            }

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

        #region DataGridView Events
        private void DgvDSTS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Custom formatting nếu cần
        }

        private void DgvDSTS_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!_isPhongKeHoach) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvDSTS.Columns["ThaoTac"].Index) return;

            e.PaintBackground(e.ClipBounds, true);

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
            int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

            Rectangle editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            Rectangle deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

            if (Properties.Resources.edit != null)
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);
            if (Properties.Resources.trash_can != null)
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

            e.Handled = true;
        }

        private void DgvDSTS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongKeHoach) return;
            if (e.RowIndex < 0 || e.RowIndex >= dgvDSTS.Rows.Count) return;
            if (e.ColumnIndex != dgvDSTS.Columns["ThaoTac"].Index) return;

            DataGridViewRow row = dgvDSTS.Rows[e.RowIndex];
            if (row?.Cells["maNen"]?.Value == null) return;

            Rectangle cellBounds = dgvDSTS.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            if (cellBounds.IsEmpty) return;

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = cellBounds.Left + (cellBounds.Width - totalWidth) / 2;
            int startY = cellBounds.Top + (cellBounds.Height - iconHeight) / 2;

            Rectangle editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            Rectangle deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

            Point clickPoint = dgvDSTS.PointToClient(Cursor.Position);

            if (editRect.Contains(clickPoint)) HandleEdit(row);
            else if (deleteRect.Contains(clickPoint)) HandleDelete(row);
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

            NenMau nmSource = row.DataBoundItem as NenMau;

            if (nmSource == null)
            {
                MessageBox.Show("Không thể lấy thông tin nền mẫu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ThemNenMau1 frmSua = new ThemNenMau1();
            frmSua.isEditMode = true;
            frmSua.NenMauHienTai = nmSource;

            currentOpenForm = frmSua;
            CenterFormOnParent(frmSua);

            frmSua.FormClosed += (s, ev) => { currentOpenForm = null; };
            frmSua.SuccessfullyUpdated += (s, ev) => taiDanhSachNenMau();

            frmSua.Show(this.FindForm());
        }

        private void HandleDelete(DataGridViewRow row)
        {
            string maNen = row.Cells["maNen"].Value?.ToString();
            string tenNenMau = row.Cells["tenNenMau"].Value?.ToString();

            if (string.IsNullOrEmpty(maNen)) return;

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
                    NenMauBLL bll = new NenMauBLL();
                    bll.xoaNenMau(maNen);

                    MessageBox.Show("Xóa nền mẫu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.BeginInvoke(new Action(() =>
                    {
                        taiDanhSachNenMau();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi xóa nền mẫu: {ex.Message}",
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

                // Reset về phân trang
                trangHientai = 1;
                tongSoBanGhi = 0;
                taiDanhSachNenMau();
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

                // Reset về phân trang
                trangHientai = 1;
                tongSoBanGhi = 0;
                taiDanhSachNenMau();
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
                // Reset về phân trang
                trangHientai = 1;
                tongSoBanGhi = 0;
                taiDanhSachNenMau();
                return;
            }

            // ✅ TÌM KIẾM TRÊN TOÀN BỘ DATABASE
            try
            {
                NenMauBLL bll = new NenMauBLL();
                var allData = bll.layDSNenMau(); // Lấy tất cả nền mẫu

                var filtered = allData
                    .Where(nm =>
                        (nm.maNen ?? "").ToLower().Contains(keyword) ||
                        (nm.tenNenMau ?? "").ToLower().Contains(keyword) ||
                        (nm.moTa ?? "").ToLower().Contains(keyword)
                    )
                    .ToList();

                dgvDSTS.DataSource = new BindingList<NenMau>(filtered);

                // Hiển thị số kết quả
                if (soTrang != null)
                    soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

                // Disable nút phân trang khi search
                if (btnTruoc != null) btnTruoc.Enabled = false;
                if (btnSau != null) btnSau.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        #region Pagination Events
        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (trangHientai > 1)
            {
                trangHientai--;
                taiTrangNenMau();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (trangHientai < tongSoTrang)
            {
                trangHientai++;
                taiTrangNenMau();
            }
        }
        #endregion

        #region Button Events
        private void btnThemuser_Click_1(object sender, EventArgs e)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ThemNenMau1 themNenMau1 = new ThemNenMau1();
            currentOpenForm = themNenMau1;
            CenterFormOnParent(themNenMau1);

            themNenMau1.FormClosed += (s, ev) => { currentOpenForm = null; };

            if (themNenMau1.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                taiDanhSachNenMau();
            }
        }

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
            // Legacy method
        }

        private void dgvDSTS_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvDSTS.Width;
            int dgvHeight = dgvDSTS.Height;
            Image watermark = Properties.Resources.greenlogo;

            int x = (dgvWidth - watermark.Width) / 2;
            int y = (dgvHeight - watermark.Height) / 2;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.08f;
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(watermark,
                new Rectangle(x, y, watermark.Width, watermark.Height),
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                attributes);
        }
        #endregion

        private async void BtnMic_Click(object sender, EventArgs e)
        {
            if (!_ready) return;

            if (!_recorder.IsRecording)
            {
                try
                {
                    _recorder.Start();
                    picturemicro.Image = Properties.Resources.microphone_hoatdong;
                    searchtextbox.ForeColor = Color.Silver;
                    searchtextbox.Text = "Đang nghe...";
                    isPlaceholder = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể ghi âm: " + ex.Message);
                }
            }

            else
            {
                picturemicro.Enabled = false;
                picturemicro.Image = Properties.Resources.microphone;

                _recorder.Stop();
                await Task.Delay(300);

                try
                {
                    string text;
                    try
                    {
                        text = await _whisper.TranscribeIFlytekAsync(_wavPath);

                    }
                    catch
                    {
                        text = await _whisper.TranscribeAsync(_wavPath);
                    }

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Không nghe được nội dung");
                        return;
                    }

                    // Đảm bảo bỏ trạng thái placeholder
                    isPlaceholder = false;
                    searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);

                    searchtextbox.Text = text.Trim();
                    searchtextbox.SelectionStart = searchtextbox.Text.Length;

                    // Cho tự động lọc luôn nếu muốn
                    PerformSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhận dạng giọng nói: " + ex.Message);
                }
                finally
                {
                    picturemicro.Enabled = true;
                }

            }

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}