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
    public partial class DanhSachThongSo : UserControl
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
        private const string PLACEHOLDER_TEXT = "Tìm kiếm thông số...";

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private BindingList<ThongSo> dsThongSo;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        // Phân trang
        int trangHientai = 1;
        int kichthuocTrang = 15;
        int tongSoBanGhi = 0;
        int tongSoTrang = 0;

        // Cell action rectangles
        private Rectangle editRect;
        private Rectangle deleteRect;
        #endregion

        #region Constructor
        public DanhSachThongSo()
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
                InitializeButtonIcons();
                InitializeButtonStyles();
                InitializeWatermark();
                CalculateLayout();

                // Load dữ liệu phân trang
                taiDanhSachThongSo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void taiDanhSachThongSo()
        {
            tongSoBanGhi = 0;
            taiTrangThongSo();
        }

        private void taiTrangThongSo()
        {
            var bll = new ThongSoBLL();

            // Tính tổng số trang (chỉ cần 1 lần khi load form)
            if (tongSoBanGhi == 0)
            {
                tongSoBanGhi = bll.demSoLuongThongSo();
                tongSoTrang = (int)Math.Ceiling((double)tongSoBanGhi / kichthuocTrang);
            }

            var data = bll.layDanhSachThongSo_PhanTrang(trangHientai, kichthuocTrang);
            dsThongSo = new BindingList<ThongSo>(data);
            dgvDSTS.DataSource = dsThongSo;

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
                    DataPropertyName = "MaTS",
                    HeaderText = "MÃ THÔNG SỐ",
                    Name = "MaTS"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TenTS",
                    HeaderText = "TÊN THÔNG SỐ",
                    Name = "TenTS"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DonVi",
                    HeaderText = "ĐƠN VỊ",
                    Name = "DonVi"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "phuongPhap",
                    HeaderText = "PHƯƠNG PHÁP",
                    Name = "phuongPhap"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "GiaTriToiThieu",
                    HeaderText = "GIÁ TRỊ TỐI THIỂU",
                    Name = "GiaTriToiThieu",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "GiaTriToiDa",
                    HeaderText = "GIÁ TRỊ TỐI ĐA",
                    Name = "GiaTriToiDa",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" },
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                }
            });

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

        private void InitializeButtonIcons()
        {
            if (btnThemuser != null && btnThemuser.Image != null)
            {
                btnThemuser.Image = new Bitmap(btnThemuser.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            btnThemuser.Visible = _isPhongKeHoach;
           

            if (btnThemuser != null)
            {
                btnThemuser.Size = new Size(66, 40);
                BoGocButton(btnThemuser, 20);
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

        private void InitializeWatermark()
        {
            if (Properties.Resources.greenlogo == null || dgvDSTS == null) return;

            try
            {
                Image watermark = Properties.Resources.greenlogo;
                Bitmap bmp = new Bitmap(watermark.Width, watermark.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    ColorMatrix matrix = new ColorMatrix();
                    matrix.Matrix33 = 0.15f;
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(watermark,
                        new Rectangle(0, 0, watermark.Width, watermark.Height),
                        0, 0, watermark.Width, watermark.Height,
                        GraphicsUnit.Pixel,
                        attributes);
                }

                dgvDSTS.BackgroundImage = bmp;
                dgvDSTS.BackgroundImageLayout = ImageLayout.Center;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Watermark error: {ex.Message}");
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
            if (btnThemuser == null || containersearch == null) return;

            int formWidth = this.Width;

            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            // ✅ Kích thước button dựa vào trạng thái maximize
            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;
            int topOffset = 10;

            // ✅ Resize và bo góc button
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnThemuser, btnRadius);
            btnThemuser.Top = topOffset;

            // ✅ Đặt button ở góc phải với margin
            Control btnParent = btnThemuser.Parent; // Thường là panel6
            if (btnParent != null && btnParent != this)
            {
                int parentWidth = btnParent.Width;
                btnThemuser.Left = parentWidth - btnWidth - MARGIN;
            }
            else
            {
                btnThemuser.Left = formWidth - btnWidth - MARGIN;
            }

            // ✅ Tính toán vị trí cho search box
            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = btnThemuser.Left - SPACING;

            // ✅ Nếu có micro icon, trừ thêm khoảng cách
            if (picturemicro != null)
            {
                rightBoundary -= (picturemicro.Width + SPACING);
            }

            int availableWidth = rightBoundary - leftBoundary;

            // ✅ Tính width cho search box
            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            // ✅ Đặt vị trí pictureFilter
            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
            }

            // ✅ Đặt vị trí và kích thước search box
            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            // ✅ Resize textbox bên trong
            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            // ✅ Đặt micro icon sau search box
            if (picturemicro != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
            }

            // ✅ Padding button dựa vào trạng thái maximize
            if (isMaximized)
            {
                btnThemuser.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemuser.Padding = new Padding(5, 3, 5, 3);
            }

            // ✅ Refresh vẽ lại search box
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
            if (!_isPhongKeHoach) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvDSTS.Columns["ThaoTac"].Index) return;

            e.PaintBackground(e.ClipBounds, true);

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
            int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

            editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

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

            var clickPoint = dgvDSTS.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvDSTS.Rows[e.RowIndex];

            if (row?.Cells["MaTS"]?.Value == null) return;

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

            ThongSo tsSource = row.DataBoundItem as ThongSo;

            if (tsSource == null)
            {
                MessageBox.Show("Không thể lấy thông tin thông số!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ThemThongSo frmSua = new ThemThongSo(tsSource);
            currentOpenForm = frmSua;
            CenterFormOnParent(frmSua);

            frmSua.FormClosed += (s, ev) => { currentOpenForm = null; };

            if (frmSua.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                taiDanhSachThongSo();
            }
        }

        private void HandleDelete(DataGridViewRow row)
        {
            string maTS = row.Cells["MaTS"].Value?.ToString();
            string tenTS = row.Cells["TenTS"].Value?.ToString();

            if (string.IsNullOrEmpty(maTS)) return;

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa thông số '{tenTS}' (Mã: {maTS}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    ThongSoBLL bll = new ThongSoBLL();
                    string ketQua;

                    bool success = bll.xoaThongSoMoiTruong(maTS, out ketQua);

                    if (success)
                    {
                        MessageBox.Show(ketQua, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.BeginInvoke(new Action(() =>
                        {
                            taiDanhSachThongSo();
                        }));
                    }
                    else
                    {
                        MessageBox.Show(ketQua, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi xóa thông số: {ex.Message}",
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
                taiDanhSachThongSo();
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
                taiDanhSachThongSo();
                return;
            }

            // ✅ TÌM KIẾM TRÊN TOÀN BỘ DATABASE (không chỉ trang hiện tại)
            try
            {
                ThongSoBLL bll = new ThongSoBLL();
                var allData = bll.layDanhSachThongSo(); // Lấy tất cả thông số

                var filtered = allData
                    .Where(ts =>
                        (ts.MaTS ?? "").ToLower().Contains(keyword) ||
                        (ts.TenTS ?? "").ToLower().Contains(keyword) ||
                        (ts.DonVi ?? "").ToLower().Contains(keyword) ||
                        (ts.phuongPhap ?? "").ToLower().Contains(keyword) ||
                        (ts.GiaTriToiThieu?.ToString() ?? "").Contains(keyword) ||
                        (ts.GiaTriToiDa?.ToString() ?? "").Contains(keyword)
                    )
                    .ToList();

                dgvDSTS.DataSource = new BindingList<ThongSo>(filtered);

                // Ẩn phân trang khi đang search
                if (soTrang != null)
                    soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

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
                taiTrangThongSo();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (trangHientai < tongSoTrang)
            {
                trangHientai++;
                taiTrangThongSo();
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

            ThemThongSo themThongSo = new ThemThongSo();
            currentOpenForm = themThongSo;
            CenterFormOnParent(themThongSo);

            themThongSo.FormClosed += (s, ev) => { currentOpenForm = null; };

            if (themThongSo.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                taiDanhSachThongSo();
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
            // Legacy method - kept for compatibility
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            // Legacy method
        }

        private void dgvDSTS_Paint_1(object sender, PaintEventArgs e)
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
    }
}